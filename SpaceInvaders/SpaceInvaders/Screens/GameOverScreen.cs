using Infrastructure;
using Invaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Infrastructure.Managers;
using SpaceInvaders.Infrastructure.ObjectModel;
using SpaceInvaders.Infrastructure.ObjectModel.Screens;
using SpaceInvaders.SpaceInvaders.Menues;
using SpaceInvaders.SpaceInvaders.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.SpaceInvaders.Screens
{
    public class GameOverScreen : GameScreen
    {
        private Sprite m_GameOverMessage;
        private StartMenu m_WelcomeMenu;
        private Vector2 m_PlayersScoresPositions;
        private Vector2 PlayersScoresPositions
        {
            get { return m_PlayersScoresPositions; }
            set { m_PlayersScoresPositions = value; }
        }

        private List<TextAsSprite> m_PlayersScoresSprites;
        private List<TextAsSprite> PlayersScoresSprites
        {
            get { return m_PlayersScoresSprites; }
            set { m_PlayersScoresSprites = value; }
        }

        public GameOverScreen(Game i_Game)
            : base(i_Game)
        {
            Background = new Background(i_Game, @"Sprites\BG_Space01_1024x768", 1);
            PlayersScoresSprites = new List<TextAsSprite>();
            PlayersScoresPositions = new Vector2(250, 50);
            m_WelcomeMenu = new StartMenu(i_Game as GameStructure, this);
            m_GameOverMessage = new Sprite(@"Sprites\GameOverMessage", this.Game);
            Add(m_GameOverMessage);
            Add(m_WelcomeMenu);
            Add(m_Background);
            this.ActivationLength = TimeSpan.FromSeconds(1);
            m_WelcomeMenu.NotifyGoToMainMenu += resetToMainMenu;
            m_WelcomeMenu.NotifyStartGame += resetToStartGame;
            m_WelcomeMenu.NotifyToExitGame += resetToExitGame;
        }

        private void AddPlayerScore(TextAsSprite i_PlayerScore)
        {
            TextAsSprite PlayerScoreClone = i_PlayerScore.ShallowClone() as TextAsSprite;
            PlayerScoreClone.Position = PlayersScoresPositions;
            PlayersScoresPositions = new Vector2(PlayersScoresPositions.X, PlayersScoresPositions.Y + 25 );
            Add(PlayerScoreClone);
            PlayersScoresSprites.Add(PlayerScoreClone);
        }

        private void resetToExitGame()
        {
            ExitScreen();
        }

        private void resetToStartGame()
        {
            RemoveScores();
            ScreensManager.SetCurrentScreen(new SpaceInvadersGameScreen(Game as GameStructure));
        }

        private void resetToMainMenu()
        {
            RemoveScores();
            SettingsManager.IsGoToMainMenu = true;
            ScreensManager.Push(new SpaceInvadersGameScreen(Game as GameStructure));
            ScreensManager.SetCurrentScreen(new WelcomeScreen(Game as GameStructure));
        }

        public override void Initialize()
        {
            base.Initialize();
            m_WelcomeMenu.StartPosition = new Vector2(250, 120);
            m_GameOverMessage.Position = new Vector2(230, 0);
            SettingsManager.NotifyPlayerScoreHaveSet += (i_PlayerScore) => AddPlayerScore(i_PlayerScore);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.Escape))
            {
                ExitScreen();
            }
        }

        private void RemoveScores()
        {
            foreach(TextAsSprite score in PlayersScoresSprites)
            {
                Remove(score);
                PlayersScoresPositions = new Vector2(PlayersScoresPositions.X, PlayersScoresPositions.Y - 25);
            }
        }
    }
}