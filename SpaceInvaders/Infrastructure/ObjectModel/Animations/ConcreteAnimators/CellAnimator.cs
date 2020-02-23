using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Infrastructure
{
    public class CellAnimator : SpriteAnimator
    {
        private TimeSpan m_CellTime;
        private TimeSpan m_TimeLeftForCell;

        private int m_FirstCellIndex;

        public int FirstCellIndex
        {
            set {m_FirstCellIndex = value;}
        }
        private int m_CurrCellIdx;
        public int CurrCellIdx
        {
            get { return m_CurrCellIdx;}
            set {m_CurrCellIdx = value;}
        }
        private int m_NumberOfCells;
        public int NumberOfCells
        {
            get { return m_NumberOfCells; }
            set { m_NumberOfCells = value; }
        }

        public CellAnimator(TimeSpan i_CellTime, TimeSpan i_AnimationLength)
            : base("CellAnimation", i_AnimationLength)
        {
            this.m_CellTime = i_CellTime;
            this.m_TimeLeftForCell = i_CellTime;
            NumberOfCells = 2;
        }

        private void goToNextFrame()
        {
            CurrCellIdx++;
            if (CurrCellIdx % NumberOfCells == 0)
            {
               CurrCellIdx = m_FirstCellIndex;
            }
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.SourceRectangle = m_OriginalSpriteInfo.SourceRectangle;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            if (m_CellTime != TimeSpan.Zero)
            {
                m_TimeLeftForCell -= i_GameTime.ElapsedGameTime;
                if (m_TimeLeftForCell.TotalSeconds <= 0)
                {
                    /// we have elapsed, so blink
                    goToNextFrame();
                    m_TimeLeftForCell = m_CellTime;
                }
            }

            this.BoundSprite.SourceRectangle = new Rectangle(
                CurrCellIdx * this.BoundSprite.SourceRectangle.Width,
                this.BoundSprite.SourceRectangle.Top,
                this.BoundSprite.SourceRectangle.Width,
                this.BoundSprite.SourceRectangle.Height);
        }
    }
}
