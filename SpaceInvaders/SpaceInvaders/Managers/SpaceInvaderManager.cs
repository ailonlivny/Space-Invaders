using Infrastructure;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using SpaceInvaders.Infrastructure.Managers;
using SpaceInvaders.Infrastructure.ObjectModel.Screens;

namespace SpaceInvaders.SpaceInvaders.Managers
{
    public abstract class SpaceInvaderManager : CompositeDrawableComponent<LoadableDrawableComponent>
    {
        protected GameScreen m_GameScreen;
        public GameScreen GameScreen
        {
            get { return m_GameScreen; }
            set { m_GameScreen = value; }
        }

        private Vector2 m_Velocity;
        public Vector2 Velocity
        {
            get { return m_Velocity; }
            set { m_Velocity = value; }
        }

        public SettingsManager SettingsManager
        {
            get { return GameScreen.SettingsManager; }
        }

        public SpaceInvaderManager(GameStructure i_Game, GameScreen i_GameScreen) : base(i_Game)
        {
            GameScreen = i_GameScreen;
        }
    }
}