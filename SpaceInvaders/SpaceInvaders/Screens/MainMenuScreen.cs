using SpaceInvaders.Infrastructure.ObjectModel.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SpaceInvaders.SpaceInvaders.Sprites;
using SpaceInvaders.SpaceInvaders.Menues;
using Infrastructure;

namespace SpaceInvaders.SpaceInvaders.Screens
{
    public class MainMenuScreen : GameScreen
    {
        private const string k_AssteNameForBackground = @"Sprites/BG_Space01_1024x768";
        private MainMenu m_MainMenu;
        private SoundsSettingsScreen m_SoundsSettingsScreen;
        private ScreenSettingsScreen m_ScreenSettingsScreen;

        public MainMenuScreen(GameStructure i_Game) : base(i_Game)
        {
            m_MainMenu = new MainMenu(i_Game, this);
            m_Background = new Background(i_Game, k_AssteNameForBackground, 1);
            m_SoundsSettingsScreen = new SoundsSettingsScreen(i_Game);
            m_ScreenSettingsScreen = new ScreenSettingsScreen(i_Game);
            m_MainMenu.notifyScreenSettingsWasChecked += Screen;
            m_MainMenu.notifySoundsSettingsWasChecked += Sound;
            m_MainMenu.notifyPlayWasChosen += ExitScreen;
            Add(m_Background);
            Add(m_MainMenu);
        }

        private void Sound()
        {
            ScreensManager.SetCurrentScreen(m_SoundsSettingsScreen);
        }

        private void Screen()
        {
            ScreensManager.SetCurrentScreen(m_ScreenSettingsScreen);
        }
    }
}
