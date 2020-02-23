using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using static Invaders.Utils;
using Infrastructure;
using SpaceInvaders.Infrastructure.ObjectModel.Screens;
using SpaceInvaders.SpaceInvaders.Services;
using static SpaceInvaders.SpaceInvaders.Services.SoundFactoryMethod;

namespace Invaders
{
    public class Enemy : Sprite, IShooter, ICollidable2D
    {
        private Sound m_ShootSound;
        private Sound m_KillSound;
        private const string k_AssteName = @"Sprites/EnemiesSpirteSheet192X32";
        private const int k_MinBoundX = 0;
        private const int k_NumberToPassForShooting = 4;
        private const int k_FunNumForRandomize = 450;
        private const float k_DeltaForCorrectBulletPosiotionX = 13;
        private const float k_DeltaForCorrectBulletPosiotionY = 12;
        private const float k_HalfSec = 0.5f;
        public event Action<Enemy> IntercertWithMonitorBoundries;
        public event Action<float> DeltaIsShorterFromNextStep;
        public event Action<Enemy,Bullet> WhenIDie;
        private double m_TimeToNextjump;
        private bool m_IsEnemyShouldNotifyToSwitchJumpDirction;
        private bool m_IsEnemyTouchLeftBoundFirstTime;
        private int m_MaxNumOfActiveBullets;
        public int MaxNumOfActiveBullets
        {
            get { return m_MaxNumOfActiveBullets; }
            set { m_MaxNumOfActiveBullets = value; }
        }

        public bool IsShootableActive { get; set; }

        private bool m_IsAlive; 
        public bool IsAlive
        {
            get { return m_IsAlive; }
        }

        private int m_Score;
        public int Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }

        private int m_CellIndex { get; set; }
        public int CellIndex
        {
            get { return m_CellIndex; }
            set { m_CellIndex = value; }
        }

        private float m_DeltaToJump { get;  set; }
        public float DeltaToJump
        {
            get { return m_DeltaToJump; }
            set { m_DeltaToJump = value; }
        }

        private double m_TimeToNextShoot;
        public double TimeToNextShoot
        {
            get { return m_TimeToNextShoot; }
            set { m_TimeToNextShoot = value; }
        }

        private int m_RandomToShot;
        public int RandomToShot
        {
            get { return m_RandomToShot; }
            set { m_RandomToShot = value; }
        }

        public List<Bullet> Bullets { get; set; }

        public event Action NotifyGameOver;

        public Enemy(GameStructure i_Game) : base(k_AssteName, i_Game)
        {
            m_DeltaToJump = 0;
            m_TimeToNextjump = 0.5;
            m_IsEnemyShouldNotifyToSwitchJumpDirction = false;
            m_IsEnemyTouchLeftBoundFirstTime = true;
            m_IsAlive = true;
            Bullets = new List<Bullet>();
        }

        public Enemy(GameStructure i_Game,int i_Score, int i_CellIndex, Color i_TintColor) : base(k_AssteName, i_Game)
        {
            m_CellIndex = i_CellIndex;
            m_Score = i_Score;
            TintColor = i_TintColor;
            m_DeltaToJump = 0;
            m_TimeToNextjump = 0.5;
            m_IsEnemyShouldNotifyToSwitchJumpDirction = false;
            m_IsEnemyTouchLeftBoundFirstTime = true;
            m_IsAlive = true;
            Bullets = new List<Bullet>();
        }

        public override void Initialize()
        {
            base.Initialize();
            (this.Animations["CellAnimation"] as CellAnimator).FirstCellIndex = m_CellIndex;
            (this.Animations["CellAnimation"] as CellAnimator).CurrCellIdx = m_CellIndex;
            m_ShootSound = SoundFactoryMethod.CreateSound(Game as GameStructure, eSoundName.EnemyGunShot);
            m_KillSound = SoundFactoryMethod.CreateSound(Game as GameStructure, eSoundName.EnemyKill);
        }

        public void Shoot()
        {
            if (Enabled && Bullets.Count < MaxNumOfActiveBullets)
            {
                Vector2 bulletPosition = new Vector2(m_Position.X + k_DeltaForCorrectBulletPosiotionX, m_Position.Y + k_DeltaForCorrectBulletPosiotionY);
                Bullet bullet = new Bullet(bulletPosition, eDirection.Down, Game as GameStructure, GameScreen);
                bullet.TintColor = Color.Blue;
                Bullets.Add(bullet);
                GameScreen.Add(bullet);
                m_ShootSound.Play();
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            this.Rotation += this.AngularVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            this.Animations.Update(i_GameTime);
            Vector2 maxBounds = new Vector2(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
            maxBounds.X -= this.Bounds.Width;
            maxBounds.Y -= this.Bounds.Height;

            if ((this.Position.X >= maxBounds.X || this.Position.X <= 0 || this.Position.Y >= maxBounds.Y 
                || this.Position.Y <= 0) && m_IsEnemyShouldNotifyToSwitchJumpDirction 
                && !m_IsEnemyTouchLeftBoundFirstTime)
            {
                if (IntercertWithMonitorBoundries != null)
                {
                    IntercertWithMonitorBoundries(this);
                }
                m_IsEnemyShouldNotifyToSwitchJumpDirction = false;
            }



            m_TimeToNextjump += i_GameTime.ElapsedGameTime.TotalSeconds;

            if (m_TimeToNextjump >= k_HalfSec) 
            {
                calculateAndUpdateEnemyStep(maxBounds.X);
            }

            m_TimeToNextShoot += i_GameTime.ElapsedGameTime.TotalSeconds;

            if (m_TimeToNextShoot > Utils.CalculateRandom(k_FunNumForRandomize))
            {
                if (DoINeedToShoot())
                {
                    Shoot();
                }
                m_TimeToNextShoot = 0;
            }
            Bullets = Bullets.Where(x => x.Enabled == true).ToList();
        }

        private void calculateAndUpdateEnemyStep(double i_MaxBoundX)
        {
            float newX;
            float newY;
            float deltaX = ifIAmGoingRight() ? (float)i_MaxBoundX - Position.X : k_MinBoundX - Position.X;

            if (Math.Abs(Velocity.X) > Math.Abs(deltaX) && Math.Abs(deltaX) >= 0)
            {
                if (DeltaIsShorterFromNextStep != null)
                {
                    DeltaIsShorterFromNextStep(deltaX);
                }
            }

            newX = Position.X + Velocity.X;
            newY = Position.Y + m_DeltaToJump;
            m_DeltaToJump = 0;
            Position = new Vector2(newX, newY);
            m_TimeToNextjump -= k_HalfSec;
            m_IsEnemyShouldNotifyToSwitchJumpDirction = true;
            m_IsEnemyTouchLeftBoundFirstTime = false;
        }

        private bool ifIAmGoingRight()
        {
            return Velocity.X > 0;
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is SpaceShip)
            {
                if (NotifyGameOver != null)
                {
                    NotifyGameOver();
                }
            }
            else if (i_Collidable is Bullet)
            {
                if ((i_Collidable as Bullet).Direction == eDirection.Up)
                {
                    m_IsAlive = false;
                    (this.Animations["RotateAnimation"] as RotateAnimator).Enabled = true;
                    (this.Animations["ShrinkAnimation"] as ShrinkAnimator).Enabled = true;
                    if (WhenIDie != null)
                    {
                        WhenIDie(this, i_Collidable as Bullet);
                        m_KillSound.Play();
                    }
                }
            }
        }

        protected override void InitBounds()
        {
            m_WidthBeforeScale = Texture.Width/6;
            m_HeightBeforeScale = Texture.Height;
            m_Position = Vector2.Zero;
            InitSourceRectangle();
            InitOrigins();
        }

        protected override void InitOrigins()
        {
            this.RotationOrigin = new Vector2(Width / 2, Height / 2);
        }

        protected override void InitSourceRectangle()
        {
            base.InitSourceRectangle();

            this.SourceRectangle = new Rectangle(
                0 + m_CellIndex * ((int)(SourceRectangle.Width)),
                0,
                (int)(m_SourceRectangle.Width),
                (int)m_HeightBeforeScale);
        }

        internal bool DoINeedToShoot()
        {
            bool isGoingToShot = false;
            int rangeForRandom = 35;
            int getRandom = Utils.CalculateRandom(rangeForRandom);
            if (funcForRandomizeShooting(getRandom))
            {
                isGoingToShot = true;
            }

            return isGoingToShot;
        }

        private bool funcForRandomizeShooting(int getRandom)
        {
            return getRandom % 3 == 0 && getRandom % 7 == 0 && getRandom % 5 == 0;
        }

        protected override void InitAnimations()
        {
            float sixRounds = (MathHelper.TwoPi ) * 6;
            CellAnimator celAnimation = new CellAnimator(TimeSpan.FromSeconds(k_HalfSec), TimeSpan.Zero);
            RotateAnimator rotateAnimator = new RotateAnimator(sixRounds, TimeSpan.FromSeconds(1.2));
            ShrinkAnimator shrinkAnimator = new ShrinkAnimator(TimeSpan.FromSeconds(1.2));
            rotateAnimator.Finished += onRotatorAnimatorFinished;
            this.Animations.Add(celAnimation);
            this.Animations.Add(rotateAnimator);
            this.Animations.Add(shrinkAnimator);
            (this.Animations["RotateAnimation"] as RotateAnimator).Enabled = false;
            (this.Animations["ShrinkAnimation"] as ShrinkAnimator).Enabled = false;
            this.Animations.Enabled = true;
        }

        private void onRotatorAnimatorFinished(object sender, EventArgs e)
        {
            this.Enabled = false;
            this.Visible = false;
        }

        public override bool CheckSpecificCollisionDemands(ICollidable i_Source)
        {
            bool isCollided = false;
            bool isSourceSpaceShip = (i_Source is SpaceShip);
            bool isSourceSpaceShipBullet = ((i_Source is Bullet) 
                && (i_Source as Bullet).Direction == eDirection.Up);
            bool isSourceBarricade = (i_Source is Barricade);

            if ((isSourceSpaceShip || isSourceSpaceShipBullet || isSourceBarricade) && CheckPixelCollision(i_Source))
            {
                isCollided = true;
            }

            return isCollided;
        }
    }
}