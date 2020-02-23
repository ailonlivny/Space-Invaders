using System;
using Microsoft.Xna.Framework;
using Infrastructure;
using static SpaceInvaders.SpaceInvaders.Services.SoundFactoryMethod;
using SpaceInvaders.SpaceInvaders.Services;

namespace Invaders
{
    public class EnemyMotherShip : Sprite , ICollidable2D
    {
        private const string k_AssteName = @"Sprites/MotherShip_32x120";
        public event Action<int,Bullet> WhenIDie;
        private double m_TimeToNextRandom;
        private const int k_NumForRandomizeApperance = 4;
        private const int k_Range = 10;
        private const int k_ThreeSec = 3;
        private int m_Speed;
        private Sound m_KillSound;

        public Random M_Random;
        public Random Random
        {
            get { return M_Random; }
        }

        public bool m_IsAlive;
        public bool IsAlive
        {
            get { return m_IsAlive; }
        }

        public int m_Score;
        public int Score
        {
            get { return m_Score; }
        }

        public EnemyMotherShip(GameStructure i_Game) : base(k_AssteName, i_Game)
        {
            m_Speed = 100;
            m_IsAlive = true;
            m_TimeToNextRandom = 0;
            m_Score = 800;
            M_Random = new Random();
            m_TintColor = Color.Red;
        }

        public override void Initialize()
        {
            base.Initialize();
            m_KillSound = SoundFactoryMethod.CreateSound(Game as GameStructure, eSoundName.MotherShipKill);
        }

        protected override void InitBounds()
        {
            base.InitBounds();
            Position = new Vector2(-32 ,32);
        }

        public override void Update(GameTime i_GameTime)
        {
            this.Animations.Update(i_GameTime);

            float newX;
            float newY;

            if (base.Game.GraphicsDevice.Viewport.Width <= Position.X)
            {
                m_IsAlive = false;

            }

            if (m_IsAlive)
            {
                newX = Position.X + (m_Speed * (float)i_GameTime.ElapsedGameTime.TotalSeconds);
                newY = Position.Y;
                Position = new Vector2(newX, newY);
            }
            else
            {
                randomizeNewApperance(i_GameTime);
            }
        }

        private void randomizeNewApperance(GameTime i_GameTime)
        {
            m_TimeToNextRandom += i_GameTime.ElapsedGameTime.TotalSeconds;
            if (m_TimeToNextRandom >= k_ThreeSec)
            {
                if (M_Random.Next(k_Range) > k_ThreeSec)
                {
                    Position = new Vector2(-32, 32);
                    Visible = true;
                    m_IsAlive = true;
                }

                m_TimeToNextRandom -= k_ThreeSec;
            }
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet)
            {
                processBulletColision(i_Collidable as Bullet);
            }

            base.Collided(i_Collidable);
        }

        private void processBulletColision(Bullet i_Bullet)
        {
            m_IsAlive = false;
            (this.Animations["BlinkAnimation"] as BlinkAnimator).Enabled = true;
            (this.Animations["ShrinkAnimation"] as ShrinkAnimator).Enabled = true;
            (this.Animations["FadeOutAnimation"] as FadeOutAnimator).Enabled = true;

            if (WhenIDie != null)
            {
                WhenIDie(Score,i_Bullet);
                m_KillSound.Play();
            }
        }

        protected override void InitAnimations()
        {
            BlinkAnimator blinkAnimator = new BlinkAnimator(TimeSpan.FromSeconds(0.3), TimeSpan.FromSeconds(2.2));
            ShrinkAnimator shrinkAnimator = new ShrinkAnimator(TimeSpan.FromSeconds(2.2));
            FadeOutAnimator fadeOutAnimator = new FadeOutAnimator(TimeSpan.FromSeconds(2.2));

            this.Animations.Add(blinkAnimator);
            this.Animations.Add(shrinkAnimator);
            this.Animations.Add(fadeOutAnimator);

            (this.Animations["BlinkAnimation"] as BlinkAnimator).Enabled = false;
            (this.Animations["ShrinkAnimation"] as ShrinkAnimator).Enabled = false;
            (this.Animations["FadeOutAnimation"] as FadeOutAnimator).Enabled = false;

            fadeOutAnimator.Finished += onFadeOutAnimatorFinished;
            shrinkAnimator.Finished += onShrinkAnimatorFinished;
            blinkAnimator.Finished += onBlinkAnimatorFinished;
            this.Animations.Enabled = true;
        }

        private void onBlinkAnimatorFinished(object sender, EventArgs e)
        {
            (sender as BlinkAnimator).Restart();
            (sender as BlinkAnimator).Enabled = false;
        }

        private void onShrinkAnimatorFinished(object sender, EventArgs e)
        {
            (sender as ShrinkAnimator).Restart();
            (sender as ShrinkAnimator).Enabled = false;
        }

        private void onFadeOutAnimatorFinished(object sender, EventArgs e)
        {
            (sender as FadeOutAnimator).Restart();
            (sender as FadeOutAnimator).Enabled = false;
            Visible = false;
        }
    }
}