using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Invaders;

namespace Infrastructure
{
    public abstract class GameStructure : Game
    {
        private GraphicsDeviceManager m_Graphics { get; set; }
        public GraphicsDeviceManager GraphicsDeviceManager
        {
            get { return m_Graphics; }
        }

        protected SpriteBatch m_SpriteBatch;

        //public GameStructure gameStructure
        //{
        //    get { return this; }
        //}

        public bool m_IsGameOver { get; set; }

        public GameStructure()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            m_IsGameOver = false;
        }
    }
}
