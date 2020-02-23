using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Invaders;
using SpaceInvaders.Infrastructure.ObjectModel.Screens;

namespace Infrastructure
{
    public class Sprite : LoadableDrawableComponent
    {
        protected GameScreen m_GameScreen;
        public GameScreen GameScreen
        {
            get { return m_GameScreen; }
            set { m_GameScreen = value; }
        }
        protected CompositeAnimator m_Animations;
        public CompositeAnimator Animations
        {
            get { return m_Animations; }
            set { m_Animations = value; }
        }

        private Texture2D m_Texture;
        public Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }

        public float Width
        {
            get { return m_WidthBeforeScale * m_Scales.X; }
            set { m_WidthBeforeScale = value / m_Scales.X; }
        }

        public float Height
        {
            get { return m_HeightBeforeScale * m_Scales.Y; }
            set { m_HeightBeforeScale = value / m_Scales.Y; }
        }

        protected float m_WidthBeforeScale;
        public float WidthBeforeScale
        {
            get { return m_WidthBeforeScale; }
            set { m_WidthBeforeScale = value; }
        }

        protected float m_HeightBeforeScale;
        public float HeightBeforeScale
        {
            get { return m_HeightBeforeScale; }
            set { m_HeightBeforeScale = value; }
        }


        protected Vector2 m_Position = Vector2.Zero;
        /// <summary>
        /// Represents the location of the sprite's origin point in screen coorinates
        /// </summary>
        public Vector2 Position
        {
            get { return m_Position; }
            set
            {
                if (m_Position != value)
                {
                    m_Position = value;
                    OnPositionChanged();
                }
            }
        }

        public Color[] m_SpriteTexureData;
        public Color[] SpriteTextureData
        {
            get { return m_SpriteTexureData; }
            set { m_SpriteTexureData = value; }
        }

        public Vector2 m_PositionOrigin;
        public Vector2 PositionOrigin
        {
            get { return m_PositionOrigin; }
            set { m_PositionOrigin = value; }
        }

        public Vector2 m_RotationOrigin = Vector2.Zero;
        public Vector2 RotationOrigin
        {
            get { return m_RotationOrigin; }// r_SpriteParameters.RotationOrigin; }
            set { m_RotationOrigin = value; }
        }

        private Vector2 PositionForDraw
        {
            get { return this.Position - this.PositionOrigin + this.RotationOrigin; }
        }

        public Vector2 TopLeftPosition
        {
            get { return this.Position - this.PositionOrigin; }
            set { this.Position = value + this.PositionOrigin; }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)TopLeftPosition.X,
                    (int)TopLeftPosition.Y,
                    (int)this.Width,
                    (int)this.Height);
            }
        }

        public Rectangle BoundsBeforeScale
        {
            get
            {
                return new Rectangle(
                    (int)TopLeftPosition.X,
                    (int)TopLeftPosition.Y,
                    (int)this.WidthBeforeScale,
                    (int)this.HeightBeforeScale);
            }
        }

        protected Rectangle m_SourceRectangle;
        public Rectangle SourceRectangle
        {
            get { return m_SourceRectangle; }
            set { m_SourceRectangle = value; }
        }

        public Vector2 TextureCenter
        {
            get
            {
                return new Vector2((float)(m_Texture.Width / 2), (float)(m_Texture.Height / 2));
            }
        }

        public Vector2 SourceRectangleCenter
        {
            get { return new Vector2((float)(m_SourceRectangle.Width / 2), (float)(m_SourceRectangle.Height / 2)); }
        }

        protected float m_Rotation = 0;
        public float Rotation
        {
            get { return m_Rotation; }
            set { m_Rotation = value; }
        }

        protected Vector2 m_Scales = Vector2.One;
        public Vector2 Scales
        {
            get { return m_Scales; }
            set
            {
                if (m_Scales != value)
                {
                    m_Scales = value;
                    // Notify the Collision Detection mechanism:
                    OnPositionChanged();
                }
            }
        }

        protected Color m_TintColor = Color.White;
        public Color TintColor
        {
            get { return m_TintColor; }
            set { m_TintColor = value; }
        }

        public float Opacity
        {
            get { return (float)m_TintColor.A / (float)byte.MaxValue; }
            set { m_TintColor.A = (byte)(value * (float)byte.MaxValue); }
        }

        protected float m_LayerDepth;
        public float LayerDepth
        {
            get { return m_LayerDepth; }
            set { m_LayerDepth = value; }
        }

        protected SpriteEffects m_SpriteEffects = SpriteEffects.None;
        public SpriteEffects SpriteEffects
        {
            get { return m_SpriteEffects; }
            set { m_SpriteEffects = value; }
        }

        protected Vector2 m_Velocity = Vector2.Zero;
        /// <summary>
        /// Pixels per Second on 2 axis
        /// </summary>
        public Vector2 Velocity
        {
            get { return m_Velocity; }
            set { m_Velocity = value; }
        }

        protected IInputManager InputManager
        {
            get { return Game.Services.GetService(typeof(IInputManager)) as IInputManager; }
        }

        private float m_AngularVelocity = 0;
        /// <summary>
        /// Radians per Second on X Axis
        /// </summary>
        public float AngularVelocity
        {
            get { return m_AngularVelocity; }
            set { m_AngularVelocity = value; }
        }

        public bool m_IsAnimationsActive { get; set; }

        public Sprite(string i_AssetName, Game i_Game, int i_UpdateOrder, int i_DrawOrder)
            : base(i_AssetName, i_Game, i_UpdateOrder, i_DrawOrder)
        { }

        public Sprite(string i_AssetName, Game i_Game, int i_CallsOrder)
            : base(i_AssetName, i_Game, i_CallsOrder)
        { }

        public Sprite(string i_AssetName, Game i_Game)
            : base(i_AssetName, i_Game, int.MaxValue)
        { }

        /// <summary>
        /// Default initialization of bounds
        /// </summary>
        /// <remarks>
        /// Derived classes are welcome to override this to implement their specific boudns initialization
        /// </remarks>
        protected override void InitBounds()
        {
            m_WidthBeforeScale = m_Texture.Width;
            m_HeightBeforeScale = m_Texture.Height;
            //m_Position = Vector2.Zero
            InitSourceRectangle();
            InitOrigins();
        }

        protected virtual void InitOrigins()
        {
                        
        }

        protected virtual void InitSourceRectangle()
        {
            m_SourceRectangle = new Rectangle(0, 0, (int)m_WidthBeforeScale, (int)m_HeightBeforeScale);
        }

        protected virtual void InitAnimations()
        {

        }

        private bool m_UseSharedBatch = false;

        public Color[] m_ColorDummy;

        protected SpriteBatch m_SpriteBatch;
        public SpriteBatch SpriteBatch
        {
            get { return m_SpriteBatch; }
            set
            {
                m_SpriteBatch = value;
                m_UseSharedBatch = true;
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            m_Animations = new CompositeAnimator(this);

            InitAnimations();
        }

        protected override void LoadContent()
        {
            m_Texture = Game.Content.Load<Texture2D>(m_AssetName);

            if (m_SpriteBatch == null)
            {
                m_SpriteBatch =
                    Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

                if (m_SpriteBatch == null)
                {
                    m_SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
                    m_UseSharedBatch = false;
                }
            }

            if (this is ICollidable)
            {
                m_SpriteTexureData = new Color[m_Texture.Width * m_Texture.Height];
                //m_ColorDummy = new Color[m_Texture.Width * m_Texture.Height];
                m_Texture.GetData(m_SpriteTexureData);
                //m_Texture.GetData(m_ColorDummy);
            }

            base.LoadContent();
        }


        /// <summary>
        /// Basic movement logic (position += velocity * totalSeconds)
        /// </summary>
        /// <param name="gameTime"></param>
        /// <remarks>
        /// Derived classes are welcome to extend this logic.
        /// </remarks>
        public override void Update(GameTime gameTime)
        {
            float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.Position += this.Velocity * totalSeconds;
            this.Rotation += this.AngularVelocity * totalSeconds;

            base.Update(gameTime);

            this.Animations.Update(gameTime); 
        }

        /// <summary>
        /// Basic texture draw behavior, using a shared/own sprite batch
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.Begin();
            }

            m_SpriteBatch.Draw(m_Texture, this.PositionForDraw,
                 this.SourceRectangle, this.TintColor,
                this.Rotation, this.RotationOrigin, this.Scales,
                SpriteEffects.None, this.LayerDepth);

            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.End();
            }

            base.Draw(gameTime);
        }

        #region Collision Handlers
        protected override void DrawBoundingBox()
        {
            // not implemented yet
        }

        public virtual bool CheckRectangelCollision(ICollidable i_Source)
        {
            bool collided = false;
            ICollidable2D source = i_Source as ICollidable2D;

            if (source != null)
            {
                collided = source.Bounds.Intersects(this.Bounds) || source.Bounds.Contains(this.Bounds);
            }

            return collided;
        }

        //public abstract Check(ICollidable i_Source);

        public virtual bool CheckCollision(ICollidable i_Source)
        {
            bool collided = false;
            ICollidable2D source = i_Source as ICollidable2D;

            if (source != null)
            {
                if (CheckRectangelCollision(i_Source))
                {
                    if (CheckSpecificCollisionDemands(i_Source))
                    {
                        collided = true;
                    }
                }
            }

            return collided;
        }

        public virtual bool CheckSpecificCollisionDemands(ICollidable i_Source)
        {
            return true; // defulat implemention
        }

        public virtual bool CheckPixelCollision(ICollidable i_Source)
        {
            bool isCollided = false;
            Rectangle myRectangel = Bounds;
            Rectangle sourceRectangel = (i_Source as Sprite).Bounds;
            Color[] myTexureData = SpriteTextureData;
            Color[] sourceTexureData = (i_Source as Sprite).SpriteTextureData;
            int top = Math.Max(myRectangel.Top, sourceRectangel.Top);
            int bottom = Math.Min(myRectangel.Bottom, sourceRectangel.Bottom);
            int left = Math.Max(myRectangel.Left, sourceRectangel.Left);
            int right = Math.Min(myRectangel.Right, sourceRectangel.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color myColor = myTexureData[(x - myRectangel.Left) + (y - myRectangel.Top) * myRectangel.Width];
                    Color sourceColor = sourceTexureData[(x - sourceRectangel.Left) + (y - sourceRectangel.Top) * sourceRectangel.Width];

                    if (myColor.A != 0 && sourceColor.A != 0)
                    {
                        isCollided = true;
                    }
                }
            }

            return isCollided;
        }

        public virtual void Collided(ICollidable i_Collidable)
        {
        }
        #endregion //Collision Handlers

        public Sprite ShallowClone()
        {
            return this.MemberwiseClone() as Sprite;
        }

        public void ProcessPixelBaseColisionOnTexture(ICollidable i_Source)
        {
            Rectangle myRectangel = Bounds;
            Rectangle sourceRectangel = (i_Source as Sprite).Bounds;
            Color[] sourceTexureData = (i_Source as Sprite).SpriteTextureData;
            int top = Math.Max(myRectangel.Top, sourceRectangel.Top);
            int bottom = Math.Min(myRectangel.Bottom, sourceRectangel.Bottom);
            int left = Math.Max(myRectangel.Left, sourceRectangel.Left);
            int right = Math.Min(myRectangel.Right, sourceRectangel.Right);
            int length = m_SpriteTexureData.Length;

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color myColor = m_SpriteTexureData[(x - myRectangel.Left) + (y - myRectangel.Top) * myRectangel.Width];
                    Color sourceColor = sourceTexureData[(x - sourceRectangel.Left) + (y - sourceRectangel.Top) * sourceRectangel.Width];

                    if (myColor.A != 0 && sourceColor.A != 0)
                    {
                        m_SpriteTexureData[(x - myRectangel.Left) + (y - myRectangel.Top) * myRectangel.Width].A = 0;
                    }
                }
            }

            Texture.SetData(0, new Rectangle(0, 0, (int)Width, (int)Height), m_SpriteTexureData, 0, length);
        }
    }
}
