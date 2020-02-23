using Infrastructure;
using SpaceInvaders.Infrastructure.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Infrastructure.Managers
{
    public class SettingsManager : GameService
    {
        public event Action<int> NotifyNumberOfPlayesChanged;
        public event Action<TextAsSprite> NotifyPlayerScoreHaveSet;
        private bool m_IsGameOver;
        public bool IsGameOver
        {
            get { return m_IsGameOver; }
            set { m_IsGameOver = value; m_IsStartANewGame = m_IsGoToMainMenu = false; }
        }

        private bool m_IsStartANewGame;
        public bool IsStartANewGame
        {
            get { return m_IsStartANewGame; }
            set { m_IsStartANewGame = value; m_IsGoToMainMenu = m_IsGameOver = false; }
        }

        private bool m_IsGoToMainMenu;
        public bool IsGoToMainMenu
        {
            get { return m_IsGoToMainMenu; }
            set { m_IsGoToMainMenu = value; m_IsStartANewGame = m_IsGameOver = false; }
        }
        private int m_NumOfPlayes;
        public int NumOfPlayes
        {
            get { return m_NumOfPlayes; }
            set { m_NumOfPlayes = value; NotifyNumberOfPlayesChanged(value);  }
        }
        private List<TextAsSprite> m_PlayerScores;
        public List<TextAsSprite> PlayerScores
        {
            get { return m_PlayerScores; }
            set { m_PlayerScores = value; }
        }

        public void AddPlayerScore(TextAsSprite i_PlayrScore)
        {
            if(NotifyPlayerScoreHaveSet != null)
            {
                NotifyPlayerScoreHaveSet(i_PlayrScore);
            }
        }
        public SettingsManager(GameStructure i_Game) : base(i_Game)
        {
            m_NumOfPlayes = 1;
            m_IsGameOver = false;
            m_IsStartANewGame = false;
            m_IsGoToMainMenu = false;
            PlayerScores = new List<TextAsSprite>();
        }
    }
}
