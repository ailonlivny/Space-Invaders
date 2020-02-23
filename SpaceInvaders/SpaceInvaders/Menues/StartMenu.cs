using SpaceInvaders.SpaceInvaders.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using SpaceInvaders.Infrastructure.ObjectModel.Screens;
using SpaceInvaders.SpaceInvaders.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders.SpaceInvaders.Menues
{
    public class StartMenu : Menu
    {
        public event Action NotifyGoToMainMenu;

        public event Action NotifyStartGame;

        public event Action NotifyToExitGame;

        private GameScreen m_GameScreen;
        private MenuItem Enter;
        private MenuItem Esc;
        private MenuItem M;
       
        public StartMenu(GameStructure i_Game, GameScreen i_GameScreen) : base(i_Game, i_GameScreen)
        {
            m_GameScreen = i_GameScreen;
            Enter = new MenuItem(i_Game) { Text = "Press Enter To Play" };
            Esc = new MenuItem(i_Game) { Text = "Press Esc To Exit Game" }; 
            M = new MenuItem(i_Game) { Text = "Press M To Main Menu" }; 
            Add(Enter);
            Add(Esc);
            Add(M);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.KeyPressed(Keys.Enter))
            {
                EnterWasChosen();
            }
            else if (InputManager.KeyPressed(Keys.Escape))
            {
                EscWasChosen();
            }
            else if (InputManager.KeyPressed(Keys.M))
            {
                MWasChosen();
            }
        }

        private void EscWasChosen()
        {
            if (NotifyToExitGame != null)
            {
                NotifyToExitGame();
            }
        }

        private void EnterWasChosen()
        {
            if (NotifyStartGame != null)
            {
                NotifyStartGame();
            }
        }

        private void MWasChosen()
        {
            if (NotifyGoToMainMenu != null)
            {
                NotifyGoToMainMenu();
            }
        }
    }
}