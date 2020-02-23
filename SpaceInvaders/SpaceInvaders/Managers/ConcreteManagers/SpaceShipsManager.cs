using Infrastructure;
using Invaders;
using Microsoft.Xna.Framework;
using SpaceInvaders.Infrastructure.Managers;
using SpaceInvaders.Infrastructure.ObjectModel;
using SpaceInvaders.Infrastructure.ObjectModel.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using static Invaders.Utils;

namespace SpaceInvaders.SpaceInvaders.Managers
{
    public class SpaceShipsManager : SpaceInvaderManager
    {
        private const string k_AssteNameSpaceShip1 = @"Sprites/Ship01_32x32";
        private const string k_AssteNameSpaceShip2 = @"Sprites/Ship02_32x32";
        private const int k_SpaceShipSize = 32;
        private Vector2 m_SpaceShipPlayer1SoulsPosition;
        private Vector2 m_SpaceShipPlayer2SoulsPosition;
        private Dictionary<string, SpaceShip> m_SpaceShips;
        public Dictionary<string, SpaceShip> SpaceShips
        {
            get { return m_SpaceShips; }
            set { m_SpaceShips = value; }
        }

        private int m_PlayersCount;
        public int PlayersCount
        {
            get { return m_PlayersCount; }
            set { m_PlayersCount = value;
                setNumberOfPlayers(); }
        }

        private bool m_ShouldIDispose;
        public bool ShouldIDispose
        {
            get { return m_ShouldIDispose; }
            set { m_ShouldIDispose = value; }
        }

        public event Action NotifiyGameOver;

        public SpaceShipsManager(GameStructure i_Game, GameScreen i_GameScreen) : base(i_Game, i_GameScreen)
        {
            SpaceShips = new Dictionary<string, SpaceShip>();
            this.BlendState = Microsoft.Xna.Framework.Graphics.BlendState.NonPremultiplied;
            m_SpaceShipPlayer1SoulsPosition = new Vector2(Game.GraphicsDevice.Viewport.Width - 130, 0);
            m_SpaceShipPlayer2SoulsPosition = new Vector2(Game.GraphicsDevice.Viewport.Width - 130, 20);
            SpaceShip spaceShip1 = new SpaceShip(Game as GameStructure, ePlayerNumber.P1, k_AssteNameSpaceShip1, this)
            { SoulsPosition = m_SpaceShipPlayer1SoulsPosition, InitPosition = new Vector2(0, Game.GraphicsDevice.Viewport.Height - k_SpaceShipSize), ScorePosition = Vector2.Zero, TextColor = Color.Blue };
            spaceShip1.WhenIDie += onWhenSpacehipDie;
            SpaceShips.Add("SpaceShip1", spaceShip1);
            spaceShip1.Add();
            SpaceShip spaceShip2 = new SpaceShip(Game as GameStructure, ePlayerNumber.P2, k_AssteNameSpaceShip2, this)
            { SoulsPosition = m_SpaceShipPlayer2SoulsPosition, InitPosition = new Vector2(0 + k_SpaceShipSize, Game.GraphicsDevice.Viewport.Height - k_SpaceShipSize), ScorePosition = new Vector2(0, 20), TextColor = Color.Green };
            spaceShip2.Enabled = spaceShip2.Visible = false;
            SpaceShips.Add("SpaceShip2", spaceShip2);
        }

        public override void Initialize()
        {
            (Game.Services.GetService(typeof(SettingsManager)) as SettingsManager).NotifyNumberOfPlayesChanged += (i_NumOfPlayers) => PlayersCount = i_NumOfPlayers;
            base.Initialize();
        }

        private void onWhenSpacehipDie(TextAsSprite i_ScoreAsText) 
        {
            SettingsManager.AddPlayerScore(i_ScoreAsText);
            if (SpaceShips.Where(spaceShip => spaceShip.Value.Enabled).Count() == 0)
            {
                if (NotifiyGameOver != null)
                {
                    NotifiyGameOver();
                }
            }
        }

        private void findAndNotifyBulletShooter(int i_EnemyScore, Bullet i_Bullet)
        {
            foreach (SpaceShip spaceShip in SpaceShips.Values)
            {
                if (spaceShip.Bullets.Contains(i_Bullet))
                {
                    spaceShip.UpdateScore(i_EnemyScore);
                }
            }
        }

        internal void ResetBeforeNextLevel()
        {
            foreach (SpaceShip spaceShip in SpaceShips.Values)
            {
                spaceShip.ResetBeforeNextLevel();
            }          
        }

        private void setNumberOfPlayers()
        {
            int i = SpaceShips.Where(spaceShip => spaceShip.Value.Enabled).Count();
            if (PlayersCount > SpaceShips.Where(spaceShip => spaceShip.Value.Enabled).Count())
            {
                SpaceShips["SpaceShip2"].Enabled = SpaceShips["SpaceShip2"].Visible = true;
                SpaceShips["SpaceShip2"].WhenIDie += onWhenSpacehipDie;
                SpaceShips["SpaceShip2"].Add();
            }
            else
            {
                SpaceShips["SpaceShip2"].Enabled = SpaceShips["SpaceShip2"].Visible = false;
                SpaceShips["SpaceShip2"].WhenIDie -= onWhenSpacehipDie;
                SpaceShips["SpaceShip2"].Remove();
            }
        }

        public void onWhenEnemyDie(int i_score, Bullet i_bullet)
        {
            findAndNotifyBulletShooter(i_score, i_bullet);
        }

        public void onWhenMotherShipDie(int i_score, Bullet i_Bullet)
        {
            findAndNotifyBulletShooter(i_score, i_Bullet);
        }

        protected override void Dispose(bool disposing)
        {
            if(ShouldIDispose)
            {
                base.Dispose(disposing);
            }
            else
            {
                ResetBeforeNextLevel();
            }
        }
    }
}