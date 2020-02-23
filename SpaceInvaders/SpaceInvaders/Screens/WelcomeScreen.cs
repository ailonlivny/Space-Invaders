using SpaceInvaders.Infrastructure.ObjectModel.Screens;
using SpaceInvaders.SpaceInvaders.Sprites;
using Microsoft.Xna.Framework;
using Infrastructure;
using SpaceInvaders.SpaceInvaders.Menues;

namespace SpaceInvaders.SpaceInvaders.Screens
{
    public class WelcomeScreen : GameScreen
    {
        private MainMenuScreen m_MainMenuScreen;
        private StartMenu m_WelcomeMenu;
        private const string k_AssteNameForBackground = @"Sprites/BG_Space01_1024x768"; 

        public WelcomeScreen(GameStructure i_Game) : base(i_Game)
        {
            m_WelcomeMenu = new StartMenu(i_Game, this);
            m_Background = new Background(i_Game, k_AssteNameForBackground, 1);
            m_MainMenuScreen = new MainMenuScreen(i_Game);
            Add(m_Background);
            Add(m_WelcomeMenu);
            m_WelcomeMenu.NotifyGoToMainMenu += GoToMainMenu;
            m_WelcomeMenu.NotifyStartGame += ExitScreen;
            m_WelcomeMenu.NotifyToExitGame += () => Game.Exit();
        }

        private void GoToMainMenu()
        {
            this.ExitScreen();
            ScreensManager.SetCurrentScreen(m_MainMenuScreen);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}