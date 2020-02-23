using Infrastructure;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Infrastructure.Interfaces;
using SpaceInvaders.Infrastructure.Managers;
using SpaceInvaders.Infrastructure.ObjectModel.Screens;
using SpaceInvaders.SpaceInvaders.Services;
using SpaceInvaders.SpaceInvaders.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpaceInvaders.SpaceInvaders.Services.SoundFactoryMethod;

namespace SpaceInvaders.SpaceInvaders.Managers
{
    public class Menu : CompositeDrawableComponent<MenuItem>
    {
        protected Sound m_MenuMoveSound;
        public event Action NotifyToGoBack;
        private Vector2 m_StartPosition;
        public Vector2 StartPosition
        {
            get { return m_StartPosition; }
            set { m_StartPosition = value; InitBounds(); }
        }
        private int m_FocusedMenuItemIdx;
        public int FocusedMenuItemIdx
        {
            get { return m_FocusedMenuItemIdx; }
            set {
                MenuItems[m_FocusedMenuItemIdx].IsFocused = false;
                m_FocusedMenuItemIdx = value;
                MenuItems[value].IsFocused = true;
                }
        }
        private List<MenuItem> m_MenuItems;
        public List<MenuItem> MenuItems
        {
            get { return m_MenuItems; }
            set { m_MenuItems = value; }
        }
        private GameScreen m_GameScreen;
        protected GameScreen GameScreen
        {
            get { return m_GameScreen; }
            set { m_GameScreen = value;}
        }
        protected IInputManager InputManager
        {
            get { return GameScreen.InputManager; }
        }
        protected SoundsManager SoundsManager
        {
            get { return GameScreen.SoundsManager; }
        }
        protected SettingsManager SettingsManager
        {
            get { return GameScreen.SettingsManager; }
        }
        public Menu(GameStructure i_Game, GameScreen i_GameScreen) : base(i_Game)
        {
            GameScreen = i_GameScreen;
            m_MenuItems = new List<MenuItem>();
        }

        public override void Add(MenuItem i_Component)
        {
            base.Add(i_Component);
            m_MenuItems.Add(i_Component as MenuItem);          
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.KeyPressed(Keys.Down))
            {
                if (FocusedMenuItemIdx % (MenuItems.Count - 1) == 0 && FocusedMenuItemIdx != 0)
                {
                    FocusedMenuItemIdx = 0;
                }
                else
                {
                    FocusedMenuItemIdx++;
                }
                m_MenuMoveSound.Play();
            }
            else if (InputManager.KeyPressed(Keys.Up))
            {

                if (FocusedMenuItemIdx == 0)
                {
                    FocusedMenuItemIdx = m_MenuItems.Count - 1;
                }
                else
                {
                    FocusedMenuItemIdx--;
                }
                m_MenuMoveSound.Play();
            }
            base.Update(gameTime);

        }

        protected override void InitBounds()
        {
            base.InitBounds();
            Vector2 Currentposition = StartPosition;
            foreach (MenuItem item in m_MenuItems)
            {
                item.Position = Currentposition;
                Currentposition = new Vector2(Currentposition.X, Currentposition.Y + 70);
            }
        }

        public override void Initialize()
        {
            
            base.Initialize();
            StartPosition = new Vector2(250, 110);
            //Show();
            FocusedMenuItemIdx = 0;
            m_MenuItems[FocusedMenuItemIdx].IsFocused = true;
            m_MenuMoveSound = SoundFactoryMethod.CreateSound(Game as GameStructure, eSoundName.MenuMove);

        }

        protected void Reset()
        {
            FocusedMenuItemIdx = 0;
        }

        protected virtual void ResetAndNotifyToGoBack()
        {
            Reset();
            if(NotifyToGoBack != null)
            {
                NotifyToGoBack();
            }
        }
    }
}