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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders.SpaceInvaders.Menues
{
    public class ScreenSettingsMenu : Menu
    {
        private MultiItem m_WindowResizing;
        private MultiItem m_FullScreen;
        private MultiItem m_MouseVisability;
        private MenuItem m_Done;

        public ScreenSettingsMenu(GameStructure i_Game, GameScreen i_GameScreen) : base(i_Game, i_GameScreen)
        {
            m_FullScreen = new MultiItem(i_Game, "Off", "On")
            {
                FunctionToExecute = i_Game.GraphicsDeviceManager.ToggleFullScreen,
                MainText = "Full Screen Mode:"
            };

            m_MouseVisability = new MultiItem(i_Game, "Invisible", "Visible" )
                {
                FunctionToExecute = () => { Game.IsMouseVisible = !Game.IsMouseVisible; },
                MainText = "Mouse Visability:"
                };

            m_WindowResizing = new MultiItem(i_Game, "Off", "On")
                {
                FunctionToExecute = () => { i_Game.Window.AllowUserResizing = !i_Game.Window.AllowUserResizing; },
                MainText = "Allow Window Resizing:"
                };

            m_Done = new MenuItem(i_Game)
            {
                FunctionToExecute = GoToMainMenu,
                Text = "Done"
            };

            Add(m_FullScreen);
            Add(m_MouseVisability);
            Add(m_WindowResizing);
            Add(m_Done);
        }

        private void GoToMainMenu()
        {
            ResetAndNotifyToGoBack();
        }
    }
}