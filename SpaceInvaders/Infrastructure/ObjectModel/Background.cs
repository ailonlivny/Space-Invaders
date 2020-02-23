using Infrastructure;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.SpaceInvaders.Sprites
{
    public class Background : Sprite
    {
        public Background(Game i_Game, string i_AssetName, int i_Opacity)
            : base(i_AssetName, i_Game)
        {
            this.Opacity = i_Opacity;
        }

        protected override void InitBounds()
        {
            base.InitBounds();

            this.DrawOrder = int.MinValue;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        internal void LoadContentTemp()
        {
            LoadContent();
        }
    }
}
