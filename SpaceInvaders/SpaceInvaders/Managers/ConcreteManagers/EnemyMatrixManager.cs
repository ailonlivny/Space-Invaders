using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.ObjectModel;
using SpaceInvaders.Infrastructure.ObjectModel.Screens;
using SpaceInvaders.SpaceInvaders.Managers;

namespace Invaders
{
    public class EnemyMatrixManager : SpaceInvaderManager
    {
        public event Action NotifyGameOver;

        public event Action<int, Bullet> NotifyEnemyDiedToGameManager;

        public event Action NotifyAllEnemysAreDead;

        public Enemy[,] m_Enemys;
        private Enemy m_LeftEdgeEnemy;
        private Enemy m_RightEdgeEnemy;
        private int m_NumOfDeadEnemys;
        private const int k_EnemySize = 32;
        private const float k_IncreaseSpeedBy3Percents = 1.03f;
        private const float k_IncreaseSpeedBy5Percents = 1.05f;
        private int m_NumOfEnemys;
        private Vector2 m_InitialPosition;
        private int m_ExtraEnemyPointsForLevel;

        public int ExtraEnemyPointsForLevel
        {
            get { return m_ExtraEnemyPointsForLevel; }
            set { m_ExtraEnemyPointsForLevel = value; }
        }

        private int m_MaxNumOfActiveBulletsForEnemy;
        public int MaxNumOfActiveBulletsForEnemy
        {
            get { return m_MaxNumOfActiveBulletsForEnemy; }
            set { m_MaxNumOfActiveBulletsForEnemy = value; }
        }

        private int m_Rows;
        public int Rows
        {
            get { return m_Rows; }
            set { m_Rows = value; }
        }

        private int m_Cols;
        public int Cols
        {
            get { return m_Cols; }
            set { m_Cols = value; }
        }

        private bool m_ShoulrefreshUpdateOrder;
        private bool ShoulrefreshUpdateOrder
        {
            get { return m_ShoulrefreshUpdateOrder; }
            set { m_ShoulrefreshUpdateOrder = value; }
        }

        public EnemyMatrixManager(GameStructure i_Game, GameScreen i_GameScreen) : base(i_Game, i_GameScreen)
        {
            m_InitialPosition = new Vector2(0, k_EnemySize * 3);
            Velocity = new Vector2(16, 0);
            m_NumOfDeadEnemys = 0;
            MaxNumOfActiveBulletsForEnemy = 1;
            ShoulrefreshUpdateOrder = false;
        }

        public override void Initialize()
        {
            m_Enemys = new Enemy[Rows, Cols];
            m_NumOfEnemys = Rows * Cols;
            InitMatrix();
            base.Initialize();
        }

        protected override void InitBounds()
        {
            Vector2 enemyPosition = m_InitialPosition;

            for (int R = 0; R < m_Enemys.GetLength(0); R++)
            {
                for (int C = 0; C < m_Enemys.GetLength(1); C++)
                {
                    m_Enemys[R, C].Position = enemyPosition;
                    m_Enemys[R, C].Velocity = Velocity;
                    enemyPosition.X += 32f + (32f * 0.6f);
                }

                enemyPosition.X = m_InitialPosition.X;
                enemyPosition.Y += 32f + (32f * 0.6f);
            }
        }

        public void InitMatrix()
        {
            int EnemyScore = 0;
            Color EnemyColor = Color.White;
            int EnemyCellIdx = 0;

            for (int R = 0; R < Rows; R++)
            {
                for (int C = 0; C < Cols; C++)
                {       
                    if (R == 0)
                    {
                        EnemyScore = 250 + ExtraEnemyPointsForLevel;
                        EnemyColor = Color.Pink;
                        EnemyCellIdx = 0;
                    }
                    else if (R < 3)
                    {
                        EnemyScore = 150 + ExtraEnemyPointsForLevel;
                        EnemyColor = Color.LightBlue;
                        EnemyCellIdx = 2;
                    }
                    else
                    {
                        EnemyScore = 100 + ExtraEnemyPointsForLevel;
                        EnemyColor = Color.Wheat;
                        EnemyCellIdx = 4;
                    }

                    m_Enemys[R, C] = new Enemy(Game as GameStructure)
                    {
                        Score = EnemyScore,
                        TintColor = EnemyColor,
                        CellIndex = EnemyCellIdx,
                        MaxNumOfActiveBullets = MaxNumOfActiveBulletsForEnemy,
                        GameScreen = this.GameScreen,
                    };
                    m_Enemys[R, C].WhenIDie += onWhenEnemyDie;
                    m_Enemys[R, C].NotifyGameOver += NotifyGameOver;
                    Add(m_Enemys[R, C]);
                }
            }

            m_LeftEdgeEnemy = m_Enemys[0, 0];
            m_RightEdgeEnemy = m_Enemys[0, Cols - 1];
            m_RightEdgeEnemy.UpdateOrder = int.MinValue;
            m_LeftEdgeEnemy.IntercertWithMonitorBoundries += onIntercertWithMonitorBoundries;
            m_RightEdgeEnemy.IntercertWithMonitorBoundries += onIntercertWithMonitorBoundries;
            m_RightEdgeEnemy.DeltaIsShorterFromNextStep += onDeltaIsShorterFromNextStep;
            m_LeftEdgeEnemy.DeltaIsShorterFromNextStep += onDeltaIsShorterFromNextStep;
        }

        public override void Update(GameTime gameTime)
        {
            if(ShoulrefreshUpdateOrder)
            {
                refreshUpdateOrderForEdgeEnemys();
                ShoulrefreshUpdateOrder = false;
            }

            base.Update(gameTime);
        }

        private void refreshUpdateOrderForEdgeEnemys()
        {
            Vector2 currentVelocityOfMatrixEnemys = m_LeftEdgeEnemy.Velocity;

            if (currentVelocityOfMatrixEnemys.X > 0)
            {
                m_LeftEdgeEnemy.UpdateOrder = int.MaxValue;
                m_RightEdgeEnemy.UpdateOrder = int.MinValue;
            }
            else if (currentVelocityOfMatrixEnemys.X < 0)
            {
                m_LeftEdgeEnemy.UpdateOrder = int.MinValue;
                m_RightEdgeEnemy.UpdateOrder = int.MaxValue;
            }
        }

        private void chooseNewRightEdgeEnemy()
        {
            for (int C = m_Enemys.GetLength(1) - 1; C >= 0; C--)
            {
                for (int R = 0; R < m_Enemys.GetLength(0); R++)
                {
                    if (m_Enemys[R, C].IsAlive
                        && m_Enemys[R, C] != m_LeftEdgeEnemy)
                    {
                        m_Enemys[R, C].IntercertWithMonitorBoundries += onIntercertWithMonitorBoundries;
                        m_Enemys[R, C].DeltaIsShorterFromNextStep += onDeltaIsShorterFromNextStep;
                        m_RightEdgeEnemy = m_Enemys[R, C];
                        refreshUpdateOrderForEdgeEnemys();
                        return;
                    }
                }
            }
        }

        private void chooseNewLeftEdgeEnemy()
        {
            for (int C = 0; C < m_Enemys.GetLength(1); C++)
            {
                for (int R = 0; R < m_Enemys.GetLength(0); R++)
                {
                    if (m_Enemys[R, C].IsAlive && m_Enemys[R, C] != m_RightEdgeEnemy)
                    {
                        m_Enemys[R, C].IntercertWithMonitorBoundries += onIntercertWithMonitorBoundries;
                        m_Enemys[R, C].DeltaIsShorterFromNextStep += onDeltaIsShorterFromNextStep;
                        m_LeftEdgeEnemy = m_Enemys[R, C];
                        refreshUpdateOrderForEdgeEnemys();
                        return;
                    }
                }
            }
        }

        private void onDeltaIsShorterFromNextStep(float i_DeltaX)
        {
            for (int R = 0; R < Rows; R++)
            {
                for (int C = 0; C < Cols; C++)
                {
                    m_Enemys[R, C].Velocity = new Vector2(i_DeltaX, m_Enemys[R, C].Velocity.Y);
                }
            }
        }

        private void onWhenEnemyDie(Enemy i_Enemy, Bullet i_Bullet)
        {
            if (NotifyEnemyDiedToGameManager != null)
            {
                NotifyEnemyDiedToGameManager(i_Enemy.Score, i_Bullet);
            }

            i_Enemy.WhenIDie -= onWhenEnemyDie;

            m_NumOfDeadEnemys++;

            if (m_NumOfEnemys <= m_NumOfDeadEnemys)
            {
                if (NotifyAllEnemysAreDead != null)
                {
                    NotifyAllEnemysAreDead();
                }
            }

            if (m_NumOfDeadEnemys % 5 == 0) 
            {
                increaseEnemysSpeedByThreePrecents();
            }

            if (m_LeftEdgeEnemy.Equals(i_Enemy))
            {
                i_Enemy.IntercertWithMonitorBoundries -= onIntercertWithMonitorBoundries;
                i_Enemy.DeltaIsShorterFromNextStep -= onDeltaIsShorterFromNextStep;
                chooseNewLeftEdgeEnemy();
            }
            else if (m_RightEdgeEnemy.Equals(i_Enemy))
            {
                i_Enemy.IntercertWithMonitorBoundries -= onIntercertWithMonitorBoundries;
                i_Enemy.DeltaIsShorterFromNextStep -= onDeltaIsShorterFromNextStep;
                chooseNewRightEdgeEnemy();
            }
        }

        private void onIntercertWithMonitorBoundries(Enemy i_Enemy)
        {
            Velocity = new Vector2(-(Velocity.X * 1.05f), Velocity.Y);

            for (int R = 0; R < m_Enemys.GetLength(0); R++)
            {
                for (int C = 0; C < m_Enemys.GetLength(1); C++)
                {
                    m_Enemys[R, C].Velocity = Velocity;
                    m_Enemys[R, C].DeltaToJump += k_EnemySize / 2f;
                }
            }

            ShoulrefreshUpdateOrder = true;
        }

        private void increaseEnemysSpeedByThreePrecents()
        {
            Velocity = new Vector2(Velocity.X * k_IncreaseSpeedBy3Percents, Velocity.Y);

            for (int R = 0; R < m_Enemys.GetLength(0); R++)
            {
                for (int C = 0; C < m_Enemys.GetLength(1); C++)
                {
                    m_Enemys[R, C].Velocity = Velocity;
                }
            }
        }
    }
}