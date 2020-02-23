using SpaceInvaders.Infrastructure.ObjectModel.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Infrastructure;
using SpaceInvaders.SpaceInvaders.Sprites;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Infrastructure.ObjectModel;
using Invaders;

namespace SpaceInvaders.SpaceInvaders.Screens
{
    public class IntroToLevelScreen : GameScreen
    {
        private const string k_AssteNameForBackground = @"Sprites/BG_Space01_1024x768";
        private int m_CountDown;

        public int CountDown
        {
            get { return m_CountDown; }
            set { m_CountDown = value;
                m_CountDownText.Text = m_CountDown.ToString(); }
        }

        private double m_TimeToCountDown;

        private TextAsSprite m_HeadLine;
        public string HeadLine
        {
            set { m_HeadLine.Text = value; }
        }

        private TextAsSprite m_CountDownText;

        public IntroToLevelScreen(GameStructure i_Game) : base(i_Game)
        {
            m_Background = new Background(i_Game, k_AssteNameForBackground, 1);
            m_HeadLine = new TextAsSprite(i_Game) { Position = new Vector2(250, 100), Scales = new Vector2(2, 2) };
            m_CountDownText = new TextAsSprite(i_Game) { Position = new Vector2(265, 130), Scales = new Vector2(3, 3) };
            m_TimeToCountDown = 0;
            ActivationLength = TimeSpan.FromSeconds(4);

            Add(m_Background);
            Add(m_HeadLine);
            Add(m_CountDownText);   
        }

        public override void Update(GameTime i_GameTime)
        {
            if (m_CountDown < 1)
            {
                ExitScreen();
            }

            base.Update(i_GameTime);

            m_TimeToCountDown += i_GameTime.ElapsedGameTime.TotalSeconds;

            if (m_TimeToCountDown >= 1)
            {
                CountDown--;
                m_TimeToCountDown -= 1;
            }
        }
    }
}
