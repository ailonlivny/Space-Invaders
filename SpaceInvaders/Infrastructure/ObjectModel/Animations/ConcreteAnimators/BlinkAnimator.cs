﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Infrastructure
{
    public class BlinkAnimator : SpriteAnimator
    {
        private TimeSpan m_BlinkLength;
        private TimeSpan m_TimeLeftForNextBlink;

        public TimeSpan BlinkLength
        {
            get { return m_BlinkLength; }
            set { m_BlinkLength = value; }
        }

        public BlinkAnimator(string i_Name, TimeSpan i_BlinkLength, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            this.m_BlinkLength = i_BlinkLength;
            this.m_TimeLeftForNextBlink = i_BlinkLength;
        }

        public BlinkAnimator(TimeSpan i_BlinkLength, TimeSpan i_AnimationLength)
            : this("BlinkAnimation", i_BlinkLength, i_AnimationLength)
        {
            m_BlinkLength = i_BlinkLength;
            m_TimeLeftForNextBlink = i_BlinkLength;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            m_TimeLeftForNextBlink -= i_GameTime.ElapsedGameTime;
            if (m_TimeLeftForNextBlink.TotalSeconds < 0)
            {
                // we have elapsed, so blink
                this.BoundSprite.Visible = !this.BoundSprite.Visible;
                m_TimeLeftForNextBlink = m_BlinkLength;
            }
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Visible = m_OriginalSpriteInfo.Visible;
        }

        public override void Restart()
        {
            base.Restart();
            m_TimeLeftForNextBlink = m_BlinkLength;
        }
    }
}
