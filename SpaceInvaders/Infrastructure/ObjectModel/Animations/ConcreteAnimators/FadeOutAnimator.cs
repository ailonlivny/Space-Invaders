using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Infrastructure
{
    public class FadeOutAnimator : SpriteAnimator
    {
        public FadeOutAnimator(TimeSpan i_AnimationLength)
            : base("FadeOutAnimation", i_AnimationLength)
        {
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Opacity = m_OriginalSpriteInfo.Opacity;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            this.BoundSprite.Opacity -= this.BoundSprite.Opacity * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}