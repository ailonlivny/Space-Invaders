using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Invaders;

namespace Infrastructure
{
    public class RotateAnimator : SpriteAnimator
    {
        private float m_RotateSpeed;

        public RotateAnimator(float i_RotateSpeed, TimeSpan i_AnimationLength)
            : base("RotateAnimation", i_AnimationLength)
        {
            m_RotateSpeed = i_RotateSpeed;
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.AngularVelocity = m_OriginalSpriteInfo.AngularVelocity;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            this.BoundSprite.AngularVelocity += m_RotateSpeed * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}