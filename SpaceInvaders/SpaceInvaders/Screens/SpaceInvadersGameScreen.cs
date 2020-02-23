
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using static Invaders.Utils;
using Infrastructure;
using Infrastructure.ObjectModel;
using SpaceInvaders.Infrastructure.ObjectModel.Screens;
using SpaceInvaders.SpaceInvaders.Screens;
using SpaceInvaders.SpaceInvaders.Managers;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Infrastructure.Managers;
using System.Linq;
using SpaceInvaders.SpaceInvaders.Services;
using static SpaceInvaders.SpaceInvaders.Services.SoundFactoryMethod;
using SpaceInvaders.SpaceInvaders.Sprites;

namespace Invaders
{
    public class SpaceInvadersGameScreen : GameScreen
    {
        private const string k_AssteNameForBackground = @"Sprites/BG_Space01_1024x768";
        private Sound m_BackGroundMusic;
        private Sound m_LevelWinSound;
        private Sound m_GameOverSound;
        private EnemyMotherShip m_MotherShip;
        private BarricadeManager m_BarricadeManager;
        private EnemyMatrixManager m_enemyMatrixManager;
        private SpaceShipsManager m_SpaceShipsManager;
        private bool m_IsGameStarted;
        private int m_Rows;
        private int m_Coll;
        private Vector2 m_BarricadeVelocity;
        private int m_ExtraEnemyPointsForLevel;
        private int m_MaxNumOfActiveBulletsForEnemy;
        private int m_CurrentLevel;
        private CollisionsManager m_CollisionsManager;
        private GamePauseScreen m_GamePauseScreen;

        public GameStructure GameStructure
        {
            get { return Game as GameStructure; }
        }

        public SpaceInvadersGameScreen(GameStructure i_Game) : base(i_Game)
        {
            this.BlendState = Microsoft.Xna.Framework.Graphics.BlendState.NonPremultiplied;
            m_IsGameStarted = false;
            initDefaultData();
            m_CurrentLevel = 1;
            m_CollisionsManager = Game.Services.GetService(typeof(ICollisionsManager)) as CollisionsManager;
            m_GamePauseScreen = new GamePauseScreen(Game as GameStructure);
            m_GameOverSound = SoundFactoryMethod.CreateSound(Game as GameStructure, eSoundName.GameOver);
            m_LevelWinSound = SoundFactoryMethod.CreateSound(Game as GameStructure, eSoundName.LevelWin);
            m_BackGroundMusic = SoundFactoryMethod.CreateSound(Game as GameStructure, eSoundName.BackgroundMusic);
        }

        private void initDefaultData()
        {
            m_Rows = 5;
            m_Coll = 9;
            m_BarricadeVelocity = Vector2.Zero;
            m_ExtraEnemyPointsForLevel = 0;
            m_MaxNumOfActiveBulletsForEnemy = 1;
        }

        public override void Initialize()
        {
            base.Initialize();
            Background = new Background(Game, k_AssteNameForBackground, 1) { DrawOrder = int.MaxValue };
            if (m_CurrentLevel == 1)
            {
                m_SpaceShipsManager = new SpaceShipsManager(GameStructure, this) { ShouldIDispose = false };
                m_SpaceShipsManager.NotifiyGameOver += GameOver;
                Add(m_SpaceShipsManager);
            }
            else
            {
                Add(m_SpaceShipsManager);
            }
            m_MotherShip = new EnemyMotherShip(GameStructure) { GameScreen = this };
            m_enemyMatrixManager = new EnemyMatrixManager(GameStructure, this) { Rows = m_Rows, Cols = m_Coll, ExtraEnemyPointsForLevel = m_ExtraEnemyPointsForLevel, MaxNumOfActiveBulletsForEnemy = m_MaxNumOfActiveBulletsForEnemy };
            m_BarricadeManager = new BarricadeManager(GameStructure, this) { Velocity = m_BarricadeVelocity, DrawOrder = int.MinValue };
            m_MotherShip.WhenIDie += m_SpaceShipsManager.onWhenMotherShipDie;
            m_enemyMatrixManager.NotifyEnemyDiedToGameManager += m_SpaceShipsManager.onWhenEnemyDie;
            m_enemyMatrixManager.NotifyAllEnemysAreDead += goToNextLevel;
            m_enemyMatrixManager.NotifyGameOver += GameOver;

            Add(m_MotherShip);
            Add(m_enemyMatrixManager);
            Add(m_BarricadeManager);
            Add(Background);
        }

        private void goToNextLevel()
        {
            m_BackGroundMusic.Stop();
            m_LevelWinSound.Play();
            m_IsGameStarted = false;
            Dispose(true);
            Clear();
            m_CurrentLevel++;
            resetLevelData();
            Initialize();
        }

        private void resetLevelData()
        {
            if (m_CurrentLevel % 5 == 0)
            {
                initDefaultData();
            }
            else
            {
                initNextLevelData();
            }

            m_enemyMatrixManager.NotifyEnemyDiedToGameManager -= m_SpaceShipsManager.onWhenEnemyDie;
            m_enemyMatrixManager.NotifyAllEnemysAreDead -= goToNextLevel;
            m_enemyMatrixManager.NotifyGameOver -= GameOver;
        }

        private void initNextLevelData()
        {
            m_Coll++;
            m_BarricadeVelocity = m_BarricadeVelocity == Vector2.Zero ?
                new Vector2(45, 0) : new Vector2(m_BarricadeVelocity.X * 1.04f, m_BarricadeVelocity.Y);
            m_ExtraEnemyPointsForLevel += 140;
            m_MaxNumOfActiveBulletsForEnemy += 1;
        }

        private void GameOver()
        {
            m_BackGroundMusic.Stop();
            m_GameOverSound.Play();
            m_SpaceShipsManager.ShouldIDispose = true;
            Dispose(true);
            Clear();
            ExitScreen();
        }

        public override void Draw(GameTime gameTime)
        {
            if (m_IsGameStarted)
            {
                base.Draw(gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!m_IsGameStarted)
            {
                ShowIntroAndStartLevel();
            }
            else
            {
                base.Update(gameTime);

                if (InputManager.KeyPressed(Keys.P))
                {
                    ScreensManager.SetCurrentScreen(m_GamePauseScreen);
                }
            }
        }

        private void ShowIntroAndStartLevel()
        {
            ScreensManager.SetCurrentScreen(new IntroToLevelScreen(Game as GameStructure) { HeadLine = string.Format("Level {0}", m_CurrentLevel), CountDown = 3 });
            m_IsGameStarted = true;
            m_BackGroundMusic.Play();
        }
    }
}