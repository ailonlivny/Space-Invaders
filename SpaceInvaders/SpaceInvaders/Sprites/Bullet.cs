using System;
using Microsoft.Xna.Framework;
using static Invaders.Utils;
using Infrastructure;
using SpaceInvaders.Infrastructure.ObjectModel.Screens;

namespace Invaders
{
    public class Bullet : Sprite, ICollidable2D
    {
        private const int k_NumberToPassForBulletsCollision = 7;
        private const float k_SeventyPrecents = 0.7f;
        private const int k_Zero = 0;
        private const int k_Range = 10;
        private const int k_EnemySize = 32;
        private const string k_AssteName = @"Sprites/Bullet";
        private Vector2 m_HitBarricadeFinishPotision;

        private eDirection m_Direction;
        public eDirection Direction
        {
            get { return m_Direction; }
        }

        public Bullet(Vector2 i_Position, eDirection i_Diraction, GameStructure i_Game) : this(i_Position, i_Diraction, i_Game,null)
        {

        }

        public Bullet(Vector2 i_Position, eDirection i_Diraction, GameStructure i_Game, GameScreen i_GameScreen) : base(k_AssteName, i_Game)
        {
            this.GameScreen = i_GameScreen;
            m_Direction = i_Diraction;
            Position = i_Position;
            Velocity = new Vector2(0, 160);
            m_HitBarricadeFinishPotision = new Vector2(-1, -1);
        }

        public override void Update(GameTime i_gameTime)
        {
            Vector2 maxBounds = new Vector2(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
            maxBounds.X -= this.Bounds.Width;
            maxBounds.Y -= this.Bounds.Height;

            if (this.Position.X >= maxBounds.X || this.Position.X <= k_Zero || this.Position.Y >= maxBounds.Y || this.Position.Y <= k_Zero)
            {
                Enabled = false;
                Visible = false;
            }
            else
            {
                Position = calculateBulletPosition(i_gameTime);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void InitBounds()
        {
            m_WidthBeforeScale = Texture.Width;
            m_HeightBeforeScale = Texture.Height;
            InitSourceRectangle();
            InitOrigins();
        }

        public override void Collided(ICollidable i_Collidable)
        {
            bool didIHitMyOwner = (i_Collidable is SpaceShip && m_Direction == eDirection.Up)
                || (i_Collidable is Enemy && m_Direction == eDirection.Down);

            if (i_Collidable is Bullet)
            {
                if (m_Direction != (i_Collidable as Bullet).m_Direction)
                {
                    processBulletsCollision(i_Collidable as Bullet);
                }
            }
            else if (i_Collidable is Barricade)
            {
                processBarricadeColision();
            }
            else if (!didIHitMyOwner)
            {
                Enabled = false;
                Visible = false;
            }
        }

        private Vector2 calculateBulletPosition(GameTime i_gameTime)
        {
            float newX = Position.X;
            float newY;
            if (this.m_Direction == eDirection.Down)
            {
                newY = Position.Y + (Velocity.Y * (float)i_gameTime.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                newY = Position.Y - (Velocity.Y * (float)i_gameTime.ElapsedGameTime.TotalSeconds);
            }

            return new Vector2(newX, newY);
        }

        private void processBarricadeColision()
        {
            if (m_Direction == eDirection.Up)
            {
                processBarricadeColisionForSpaceShipBullet();
            }
            else if (m_Direction == eDirection.Down)
            {
                processBarricadeColisionForEnemypBullet();
            }
        }

        private void processBarricadeColisionForSpaceShipBullet()
        {
            if (m_HitBarricadeFinishPotision.X < Vector2.Zero.X)
            {
                m_HitBarricadeFinishPotision = new Vector2(Position.X, Position.Y - Height * k_SeventyPrecents);
            }
            else if (m_HitBarricadeFinishPotision.Y >= Position.Y)
            {
                Enabled = false;
                Visible = false;
            }
        }

        private void processBarricadeColisionForEnemypBullet()
        {
            if (m_HitBarricadeFinishPotision.X < Vector2.Zero.X)
            {
                m_HitBarricadeFinishPotision = new Vector2(Position.X, Position.Y + Height * k_SeventyPrecents);
            }
            else if (m_HitBarricadeFinishPotision.Y <= Position.Y)
            {
                Enabled = false;
                Visible = false;
            }
        }

        private void processBulletsCollision(Bullet i_Bullet)
        {
            int randomNumberforShooting;

            if (i_Bullet.m_Direction != this.m_Direction && this.m_Direction == eDirection.Down)
            {
                randomNumberforShooting = Utils.CalculateRandom(k_Range);

                if (k_NumberToPassForBulletsCollision <= randomNumberforShooting)
                {
                    Enabled = false;
                    Visible = false;
                }
            }
            else
            {
                Enabled = false;
                Visible = false;
            }
        }

        public override bool CheckSpecificCollisionDemands(ICollidable i_Source)
        {
            bool isCollided = false;

            if (CheckPixelCollision(i_Source))
            {
                isCollided = true;
            }

            return isCollided;
        }
    }
}

