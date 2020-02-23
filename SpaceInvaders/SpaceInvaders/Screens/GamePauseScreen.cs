using Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Infrastructure.ObjectModel;
using SpaceInvaders.Infrastructure.ObjectModel.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.SpaceInvaders.Screens
{
    public class GamePauseScreen : GameScreen
    {
        private TextAsSprite m_Text;

        public GamePauseScreen(Game i_Game)
            : base(i_Game)
        {
            IsModal = true;
            IsOverlayed = true;
            UseGradientBackground = true;
            BlackTintAlpha = 0.55f;
            UseFadeTransition = true;
            m_Text = new TextAsSprite(i_Game as GameStructure);
            m_Text.Position = new Vector2(100, 300);
            m_Text.Text = @"
[ Game Paused ]
R - Resume Game";
            this.ActivationLength = TimeSpan.FromSeconds(0.5f);
            this.DeactivationLength = TimeSpan.FromSeconds(0);
            Add(m_Text);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.R))
            {
                this.ExitScreen();
            }
        }
    }
}
