using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static Invaders.Utils;
using Infrastructure;
using SpaceInvaders.Infrastructure.ObjectModel;
using SpaceInvaders.SpaceInvaders.Managers;
using static SpaceInvaders.SpaceInvaders.Services.SoundFactoryMethod;
using SpaceInvaders.SpaceInvaders.Services;

namespace Invaders
{
    public class SpaceShip : Sprite, IShooter, ICollidable2D
    {
        private const int k_MaxNumOfBullets = 2;
        private const int k_DecreaseSoulPoint = 1200;
        private const int k_SpaceShipSize = 32;
        private const int k_NumberOfStartSoulsForEachPlayer = 3;
        private const float k_DeltaForCorrectBulletPosiotionX = 13;
        private const float k_DeltaForCorrectBulletPosiotionY = 12;
        private string m_AssteName;
        public event Action<TextAsSprite> WhenIDie;
        private Sound m_ShootSound;
        private Sound m_LifeDie;
        private TextAsSprite m_ScoreAsText;
        public TextAsSprite ScoreAsText
        {
            get { return m_ScoreAsText; }
            set { m_ScoreAsText = value; }
        }

        public Vector2 ScorePosition
        {
            get { return m_ScoreAsText.Position; }
            set { m_ScoreAsText.Position = value; }
        }

        private Vector2 m_SoulsPosition;
        public Vector2 SoulsPosition
        {
            get { return m_SoulsPosition; }
            set { m_SoulsPosition = value; }
        }

        private Vector2 m_InitPosition;
        public Vector2 InitPosition
        {
            get { return m_InitPosition; }
            set { m_InitPosition = value; }
        }

        public Color TextColor
        {
            set { m_ScoreAsText.TintColor = value; }
        }

        private List<Sprite> m_SpaceShipSouls;
        public List<Sprite> SpaceShipSouls
        {
            get { return m_SpaceShipSouls; }
        }

        private ePlayerNumber m_PlayerNumber;
        public ePlayerNumber PlayerNumber
        {
            get { return m_PlayerNumber; }
            set { m_PlayerNumber = value; }
        }

        private int m_Score;
        public int Score
        {
            get { return m_Score; }
            set { m_Score = value; m_ScoreAsText.Text = string.Format("Player {0} Score : {1}", PlayerNumber.ToString(), value.ToString()); }
        }

        private int m_Speed;
        public int Speed
        {
            get { return m_Speed; }
        }

        private int m_LeftWidth;
        public int LeftWidth
        {
            get { return m_LeftWidth; }
        }

        private int m_RightWidth;
        public int RightWidth
        {
            get { return m_RightWidth; }
        }

        public List<Bullet> Bullets { get; set; }

        private float m_OriginalMouseState;
        public float OriginalMouseState
        {
            get { return m_OriginalMouseState; }
            set { m_OriginalMouseState = value; }
        }

        public bool IsShootableActive { get; set; }

        SpaceShipsManager m_SpaceShipManager;
        public SpaceShipsManager SpaceShipManager
        {
            get { return m_SpaceShipManager; }
            set { m_SpaceShipManager = value; }
        }

        public SpaceShip(GameStructure i_Game, ePlayerNumber i_PlayerNumber, string i_AssteName, SpaceShipsManager i_SpaceShipManager) : base(i_AssteName, i_Game)
        {
            SpaceShipManager = i_SpaceShipManager;
             Bullets = new List<Bullet>();
            ScoreAsText = new TextAsSprite(i_Game);
            m_PlayerNumber = i_PlayerNumber;
            Score = 0;
            m_Speed = 145;
            m_AssteName = i_AssteName;
            IsShootableActive = true;
            m_SpaceShipSouls = new List<Sprite>();
            initSouls();
            m_ShootSound = SoundFactoryMethod.CreateSound(i_Game, eSoundName.SSGunShot);
            m_LifeDie = SoundFactoryMethod.CreateSound(Game as GameStructure, eSoundName.LifeDie);
        }

        private void initSouls()
        {
            for (int idx = 0; idx < k_NumberOfStartSoulsForEachPlayer; idx++)
            {
                Sprite item = new Sprite(m_AssteName, this.Game);
                SpaceShipSouls.Add(item);
            }
        }

        protected override void InitBounds()
        {
            base.InitBounds();

            m_LeftWidth = 0;
            m_RightWidth = this.GraphicsDevice.Viewport.Width;
            m_OriginalMouseState = InputManager.MouseState.X;
            m_Position = InitPosition;
            InitOrigins();
            InitSoulsBounds();
        }
       
        public override void Update(GameTime i_GameTime)
        {
            this.Animations.Update(i_GameTime);
            this.Rotation += this.AngularVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            if (m_PlayerNumber == ePlayerNumber.P1)
            {
                bool isRight = false;
                MouseState mouseState = InputManager.MouseState;

                if (InputManager.KeyHeld(Keys.K))
                {
                    isRight = true;
                    calculateKeyboardPosition(i_GameTime, isRight);
                }
                else if (InputManager.KeyHeld(Keys.H))
                {
                    calculateKeyboardPosition(i_GameTime, isRight);
                }
                else if (m_OriginalMouseState != mouseState.X)
                {
                    CalculateMousePosition(m_LeftWidth, m_RightWidth, mouseState.X);
                }

                m_OriginalMouseState = mouseState.X;

                if (InputManager.KeyPressed(Keys.U) ||
                    (InputManager.ButtonPressed(eInputButtons.Left)))
                {
                    Shoot();
                }
            }
            else if (m_PlayerNumber == ePlayerNumber.P2)
            {
                bool isRight = false;

                if (InputManager.KeyHeld(Keys.D))
                {
                    isRight = true;
                    calculateKeyboardPosition(i_GameTime, isRight);
                }
                else if (InputManager.KeyHeld(Keys.A))
                {
                    calculateKeyboardPosition(i_GameTime, isRight);
                }

                if (InputManager.KeyPressed(Keys.W))
                {
                    Shoot();
                }
            }

            Bullets = Bullets.Where(x => x.Enabled == true).ToList();
        }

        internal void DeleteSoulAndUpdateScore()
        {
            if (m_SpaceShipSouls.Count > 0)
            {
                m_SpaceShipSouls[0].Enabled = false;
                m_SpaceShipSouls[0].Visible = false;
                m_SpaceShipSouls.RemoveAt(0);
            }

            m_Position = new Vector2(0, Game.GraphicsDevice.Viewport.Height - k_SpaceShipSize);

            if (Score > k_DecreaseSoulPoint)
            {
                Score -= k_DecreaseSoulPoint;
            }
            else
            {
                Score = 0;
            }
        }

        private void calculateKeyboardPosition(GameTime i_gameTime, bool i_isRight)
        {
            float newX;

            if (m_LeftWidth >= m_Position.X)
            {
                if (i_isRight)
                {
                    newX = m_Position.X + (m_Speed * (float)i_gameTime.ElapsedGameTime.TotalSeconds);
                    m_Position = new Vector2(newX, m_Position.Y);
                }
            }
            else if (m_RightWidth <= m_Position.X + k_SpaceShipSize)
            {
                if (!i_isRight)
                {
                    newX = m_Position.X - (m_Speed * ((float)i_gameTime.ElapsedGameTime.TotalSeconds));
                    m_Position = new Vector2(newX, m_Position.Y);
                }
            }
            else if (i_isRight)
            {
                newX = m_Position.X + (m_Speed * ((float)i_gameTime.ElapsedGameTime.TotalSeconds));
                m_Position = new Vector2(newX, m_Position.Y);
            }
            else if (!i_isRight)
            {
                newX = m_Position.X - (m_Speed * ((float)i_gameTime.ElapsedGameTime.TotalSeconds));
                m_Position = new Vector2(newX, m_Position.Y);
            }
        }

        internal void CalculateMousePosition(int i_leftWidth, int i_rightWidth, float i_mouseStateX)
        {
            if (i_leftWidth + k_SpaceShipSize <= i_mouseStateX && i_mouseStateX <= i_rightWidth)
            {
                m_Position = new Vector2(i_mouseStateX - k_SpaceShipSize, m_Position.Y);
            }
        }

        public void Shoot()
        {
            if (Bullets.Count < k_MaxNumOfBullets && IsShootableActive)
            {
                Vector2 bulletPosition = new Vector2(m_Position.X + k_DeltaForCorrectBulletPosiotionX, m_Position.Y - k_DeltaForCorrectBulletPosiotionY);
                Bullet bullet = new Bullet(bulletPosition, eDirection.Up, Game as GameStructure);
                bullet.TintColor = Color.Red;
                Bullets.Add(bullet);
                SpaceShipManager.Add(bullet);
                m_ShootSound.Play();
            }
        }

        public void UpdateScore(int i_Score)
        {
            Score += i_Score;
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet)
            {
                if ((i_Collidable as Bullet).Direction == eDirection.Down)
                {
                    processBulletHit();
                    m_LifeDie.Play();
                }
            }
        }

        private void processBulletHit()
        {
            DeleteSoulAndUpdateScore();
            processSoulState();
        }

        private void processSoulState()
        {
            if (m_SpaceShipSouls.Count > 0)
            {
                (this.Animations["BlinkAnimation"] as BlinkAnimator).Enabled = true;
            }
            else
            {
                IsShootableActive = false;
                (this.Animations["RotateAnimation"] as RotateAnimator).Enabled = true;
                (this.Animations["FadeOutAnimation"] as FadeOutAnimator).Enabled = true;
            }
        }

        protected override void InitOrigins()
        {
            this.RotationOrigin = new Vector2(Width / 2, Height / 2);
        }

        protected override void InitAnimations()
        {
            BlinkAnimator blinkAnimator = new BlinkAnimator(TimeSpan.FromSeconds(0.16), TimeSpan.FromSeconds(2.5));
            RotateAnimator rotateAnimator = new RotateAnimator((MathHelper.Pi * 2) * 4, TimeSpan.FromSeconds(2.5));
            FadeOutAnimator fadeOutAnimator = new FadeOutAnimator(TimeSpan.FromSeconds(2.5));

            this.Animations.Add(blinkAnimator);
            this.Animations.Add(rotateAnimator);
            this.Animations.Add(fadeOutAnimator);
            (this.Animations["BlinkAnimation"] as BlinkAnimator).Enabled = false;
            (this.Animations["RotateAnimation"] as RotateAnimator).Enabled = false;
            (this.Animations["FadeOutAnimation"] as FadeOutAnimator).Enabled = false;

            blinkAnimator.Finished += onBlinkAnimatorFinished;
            fadeOutAnimator.Finished += onFadeOutAnimatorFinished;

            this.Animations.Enabled = true;
        }

        private void onFadeOutAnimatorFinished(object sender, EventArgs e)
        {
            onWhenSpaceShipDie();
        }

        private void onWhenSpaceShipDie()
        {
            this.Enabled = false;
            this.Visible = false;
            if (WhenIDie != null)
            {
                WhenIDie(ScoreAsText);
            }
        }

        private void onBlinkAnimatorFinished(object sender, EventArgs e)
        {
            (sender as BlinkAnimator).Restart();
            (sender as BlinkAnimator).Enabled = false;
        }

        internal string GetScoreToDraw()
        {
            string scoreToDraw = "";

            if (m_PlayerNumber == ePlayerNumber.P1)
            {
                scoreToDraw = string.Format("P1 Score:{0}", m_Score);
            }
            else if (m_PlayerNumber == ePlayerNumber.P2)
            {
                scoreToDraw = string.Format("P2 Score:{0}", m_Score);
            }

            return scoreToDraw;
        }

        public void InitSoulsBounds()
        {
            Vector2 maxBounds = new Vector2(Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
            int idx = 1;

            foreach (Sprite Soul in SpaceShipSouls)
            {
                float setScale = 0.7f;
                Soul.Scales = new Vector2(setScale, setScale);
                Soul.Opacity = 0.7f;
                Soul.Position = new Vector2(SoulsPosition.X + idx * this.Width * setScale * 1.5f, SoulsPosition.Y);
                idx++;
            }
        }

        public void ResetBeforeNextLevel()
        {
     
           foreach(Bullet bullet in Bullets)
            {
                SpaceShipManager.Remove(bullet);
                bullet.Dispose();
            }
            Bullets.Clear();
            Position = InitPosition;
            ScoreAsText.Text = string.Format("Player {0} Score : {1}", PlayerNumber.ToString(), Score.ToString());
        }

        internal void Add()
        {
            SpaceShipManager.Add(this);
            SpaceShipManager.Add(ScoreAsText);
            foreach (Sprite Soul in SpaceShipSouls)
            {
                SpaceShipManager.Add(Soul);
            }
        }

        internal void Remove()
        {
            SpaceShipManager.Remove(this);
            SpaceShipManager.Remove(ScoreAsText);
            foreach (Sprite Soul in SpaceShipSouls)
            {
                SpaceShipManager.Remove(Soul);
            }
        }
    }
}