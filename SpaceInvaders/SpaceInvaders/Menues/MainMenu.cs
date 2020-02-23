using SpaceInvaders.SpaceInvaders.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using SpaceInvaders.Infrastructure.ObjectModel.Screens;
using SpaceInvaders.SpaceInvaders.Sprites;
using SpaceInvaders.Infrastructure.ObjectModel.Menues.MenuItems;
using SpaceInvaders.Infrastructure.Managers;
using SpaceInvaders.SpaceInvaders.Screens;

namespace SpaceInvaders.SpaceInvaders.Menues
{
    public class MainMenu : Menu
    {
        public event Action notifyScreenSettingsWasChecked;

        public event Action notifySoundsSettingsWasChecked;

        public event Action notifyPlayWasChosen;

        private MultiItem m_NumOfPlayers;
        private MenuItem m_SoundSettings;
        private MenuItem m_ScreenSettings;
        private MenuItem m_Play;
        private MenuItem m_Quit;

        public MainMenu(GameStructure i_Game, GameScreen i_GameScreen) : base(i_Game, i_GameScreen)
        {
            m_NumOfPlayers = new MultiItem(i_Game, "One", "Two")
            {
                FunctionToExecute = notifyNumberOfPlayerChanges,
                MainText = "Players:"
            };

            m_SoundSettings = new MenuItem(i_Game)
            {
                FunctionToExecute = GotoSoundsSettingsMenu,
                Text = "Sound Settings"
            };

            m_ScreenSettings = new MenuItem(i_Game)
            {
                FunctionToExecute = GotoScreenSettingsMenu,
                Text = "Screen Settings"
            };

            m_Play = new MenuItem(i_Game)
            {
                FunctionToExecute = GoToGameScreen,
                Text = "Play"
            };

            m_Quit = new MenuItem(i_Game)
            {
                FunctionToExecute = Game.Exit,
                Text = "Quit"
            };

            Add(m_NumOfPlayers);
            Add(m_SoundSettings);
            Add(m_ScreenSettings);
            Add(m_Play);
            Add(m_Quit);
        }

        private void GoToGameScreen()
        {
            if (notifyPlayWasChosen != null)
            {
                notifyPlayWasChosen();
            }
        }

        private void GotoSoundsSettingsMenu()
        {
            if (notifySoundsSettingsWasChecked != null)
            {
                notifySoundsSettingsWasChecked();
            }
        }

        private void GotoScreenSettingsMenu()
        {
            if (notifyScreenSettingsWasChecked != null)
            {
                notifyScreenSettingsWasChecked();
            }
        }

        private void notifyNumberOfPlayerChanges()
        {     
            SettingsManager settingsManager = Game.Services.GetService(typeof(SettingsManager)) as SettingsManager;
            settingsManager.NumOfPlayes = settingsManager.NumOfPlayes == 1 ? 2 : 1;
        }
    }
}