using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Invaders;

namespace Infrastructure
{
    public class ShrinkAnimator : SpriteAnimator
    { 
        public ShrinkAnimator( TimeSpan i_AnimationLength) : base("ShrinkAnimation", i_AnimationLength)
        {}

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Scales = m_OriginalSpriteInfo.Scales;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            float newX = this.BoundSprite.Scales.X - (this.BoundSprite.Scales.X * (float)i_GameTime.ElapsedGameTime.TotalSeconds);
            float newY = this.BoundSprite.Scales.Y - (this.BoundSprite.Scales.Y * (float)i_GameTime.ElapsedGameTime.TotalSeconds);
            this.BoundSprite.Scales = new Vector2(newX, newY);
        }
    }
}