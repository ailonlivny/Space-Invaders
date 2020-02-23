using Microsoft.Xna.Framework;
using Infrastructure;
using SpaceInvaders.Infrastructure.ObjectModel.Screens;
using SpaceInvaders.SpaceInvaders.Managers;

namespace Invaders
{
    public class BarricadeManager : SpaceInvaderManager
    {
        private const int k_NumOfBarricades = 4;
        private const int k_SpaceShipHeight = 32;

        public BarricadeManager(GameStructure i_Game, GameScreen i_GameScreen) : base(i_Game, i_GameScreen)
        {
            initBarricades();
            this.BlendState = Microsoft.Xna.Framework.Graphics.BlendState.NonPremultiplied;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private void initBarricades()
        {
            for (int idx = 0; idx < k_NumOfBarricades; idx++)
            {
                Barricade newBarricade = new Barricade(Game as GameStructure) { GameScreen = this.GameScreen };
                Add(newBarricade);
            }
        }

        protected override void InitBounds()
        {
            float spaceShipPositionY = Game.GraphicsDevice.Viewport.Height - (k_SpaceShipHeight * 3);
            float barricadePositionX = Game.GraphicsDevice.Viewport.Width / 3;

            foreach (Barricade barricade in m_Sprites)
            {
                barricade.OffSetPosition = barricade.Position = new Vector2(barricadePositionX, spaceShipPositionY);
                barricadePositionX += barricade.Width * 2;
                barricade.Velocity = Velocity;
            }
        }
    }
}