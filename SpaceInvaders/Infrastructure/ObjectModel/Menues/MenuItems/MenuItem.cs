using Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Infrastructure.Interfaces;
using SpaceInvaders.Infrastructure.ObjectModel;
using SpaceInvaders.Infrastructure.ObjectModel.Animations.ConcreteAnimators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.SpaceInvaders.Sprites
{
  public class MenuItem : TextAsSprite
    {
        protected Action m_FunctionToExecute;
        public Action FunctionToExecute
        {
            get { return m_FunctionToExecute; }
            set { m_FunctionToExecute = value; }
        }
        private bool m_IsFocused;
        public bool IsFocused
        {
            get { return m_IsFocused; }
            set { m_IsFocused = value; }
        }

        public MenuItem(GameStructure i_Game, Action i_FunctionToExecute) : base(i_Game)
        {
            m_FunctionToExecute = i_FunctionToExecute;
            IsFocused = false;
         //   InputManager = i_Game.Services.GetService(typeof(IInputManager)) as InputManager;
        }

        public MenuItem(GameStructure i_Game) : base(i_Game)
        {
            IsFocused = false;
            //InputManager = i_Game.Services.GetService(typeof(IInputManager)) as InputManager;
        }

        public override void Update(GameTime i_GameTime)
        {
            this.Animations.Update(i_GameTime);
            if (IsFocused)
                {
                    m_TintColor = Color.Red;
                (this.Animations["PulseAnimation"] as PulseAnimator).Enabled = true;

                if (Test())
                    {
                        ActicvateIfTestIsTrue();
                    }
                }
                else
                {
                    m_TintColor = Color.White;
                (this.Animations["PulseAnimation"] as PulseAnimator).Enabled = false;
            }

        }

        protected virtual bool Test()
        {
            return InputManager.KeyPressed(Keys.Enter);
        }

        protected virtual void ActicvateIfTestIsTrue()
        {
            if(m_FunctionToExecute != null)
            {
                m_FunctionToExecute();
            }
        }

        protected override void InitAnimations()
        {
            PulseAnimator pulseAnimator = new PulseAnimator(TimeSpan.Zero, 1.15f, 0.7f);

            this.Animations.Add(pulseAnimator);
            (this.Animations["PulseAnimation"] as PulseAnimator).Enabled = false;
            this.Animations.Enabled = true;
        }

    }
}