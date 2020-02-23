using Microsoft.Xna.Framework.Graphics;
using Infrastructure;
using SpaceInvaders.Infrastructure.Managers;
using SpaceInvaders.SpaceInvaders.Screens;

namespace Invaders
{
    public class SpaceInvaders : GameStructure
    {
        ScreensMananger m_ScreensMananger;

        public SpaceInvaders() : base()
        {
            m_ScreensMananger = new ScreensMananger(this);
            InputManager inputManager = new InputManager(this);
            CollisionsManager collisionsManager = new CollisionsManager(this);
            SettingsManager settingsManager = new SettingsManager(this);
            SoundsManager soundsManager = new SoundsManager(this);
            m_ScreensMananger.Push(new GameOverScreen(this));
            m_ScreensMananger.Push(new SpaceInvadersGameScreen(this));
            m_ScreensMananger.SetCurrentScreen(new WelcomeScreen(this));
        }

        protected override void Initialize()
        {
            this.Window.Title = "Space Invaders";
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), m_SpriteBatch);
            base.Initialize();
        }
    }
}