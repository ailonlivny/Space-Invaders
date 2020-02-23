using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure;
using SpaceInvaders.SpaceInvaders.Services;
using static SpaceInvaders.SpaceInvaders.Services.SoundFactoryMethod;

namespace Invaders
{
    public class Barricade : Sprite, ICollidable2D
    {
        private const string k_AssteName = @"Sprites/Barrier_44x32";
        private Sound m_HitSound;
        private Vector2 m_OffSetPosition;
        public Vector2 OffSetPosition
        {
            get { return m_OffSetPosition; }
            set { m_OffSetPosition = value; }
        }

        public Barricade(GameStructure i_Game) : base(k_AssteName, i_Game)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            Texture2D textureClone = new Texture2D(Game.GraphicsDevice, Texture.Width, Texture.Height);
            textureClone.SetData<Color>(m_SpriteTexureData);
            this.Texture = textureClone;
            m_HitSound = SoundFactoryMethod.CreateSound(Game as GameStructure, eSoundName.BarrierHit);
        }

        public override void Update(GameTime i_GameTime)
        {
            if (reachedBoundries())
            {
                Velocity = -Velocity;
            }

            float totalSeconds = (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            this.Position += this.Velocity * totalSeconds;
        }

        private bool reachedBoundries()
        {
            return m_OffSetPosition.X + Width / 2 <= Position.X || m_OffSetPosition.X - Width / 2 >= Position.X;
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet)
            {
                ProcessPixelBaseColisionOnTexture(i_Collidable);
                m_HitSound.Play();
            }
            else if (i_Collidable is Enemy)
            {
                ProcessPixelBaseColisionOnTexture(i_Collidable);
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