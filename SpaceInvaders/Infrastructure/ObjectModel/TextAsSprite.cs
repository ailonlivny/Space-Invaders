using Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Infrastructure.ObjectModel
{
    public class TextAsSprite : Sprite
    {
        protected SpriteFont m_Font;
        public SpriteFont Font
        {
            get { return m_Font; }
            set { m_Font = value; }
        }

        protected string m_Text;
        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }


        protected string m_FontName;
        public string FontName
        {
            get { return m_FontName; }
            set { m_FontName = value; }
        }



        public TextAsSprite(GameStructure i_Game, string i_FontName) : base(i_FontName, i_Game, int.MaxValue)
        {
            m_FontName = i_FontName;
            TintColor = Color.White;
        }

        public TextAsSprite(GameStructure i_Game) : this(i_Game, @"Fonts\ComicSansMS"){}

        protected override void LoadContent()
        {
            m_SpriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            m_Font = Game.Content.Load<SpriteFont>(m_FontName);
        }

        public override void Draw(GameTime i_GameTime)
        {
            m_SpriteBatch.DrawString(Font, Text, Position, TintColor,Rotation,RotationOrigin,Scales, SpriteEffects.None,0); 

        }

        protected override void InitBounds()
        {
            if(Position == null)
            {
                Position = Vector2.Zero;
            }
        }
    }
}
