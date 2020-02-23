using Infrastructure;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Invaders.Utils;

namespace SpaceInvaders.Infrastructure.Managers
{
    public class SoundsManager : GameService
    {
        protected List<Sound> m_Sounds;
        protected Dictionary<eSoundType, float> m_VolumeDicionary;
        private Dictionary<eSoundType, float> VolumeDicionary
        {
            get { return m_VolumeDicionary; }
            set { m_VolumeDicionary = value; }
        }
        private bool m_IsMuted;
        private bool IsMuted
        {
            get { return m_IsMuted; }
            set { m_IsMuted = value; }
        }
        public SoundsManager(GameStructure i_Game) : base(i_Game)
        {
            IsMuted = false;
            m_Sounds = new List<Sound>();
            VolumeDicionary = new Dictionary<eSoundType, float>();
            VolumeDicionary.Add(eSoundType.BackgroundMusic, 0.5f);
            VolumeDicionary.Add(eSoundType.SoundsEffects, 0.5f);
        }

        public void Mute()
        {
            foreach (Sound sound in m_Sounds)
            {
                sound.Pause();
            }
        }

        public void UnMute()
        {
            foreach (Sound sound in m_Sounds)
            {
                sound.Resume();
            }
        }

        public void AddSound(Sound i_SoundToAdd)
        {
            m_Sounds.Add(i_SoundToAdd);
        }

        public void ToggleSound()
        {
            if(IsMuted)
            {
                UnMute();
            }
            else if(!IsMuted)
            {
                Mute();
            }
            IsMuted = !IsMuted;
        }

        public void IncreaseSoundsMusicVolume(eSoundType i_SoundType)
        {
            foreach (Sound sound in m_Sounds)
            {
                if (sound.SoundType == i_SoundType)
                {
                    sound.SoundVolume = MathHelper.Clamp(sound.SoundVolume + 0.1f, 0, 1);
                }
            }
            VolumeDicionary[i_SoundType] = MathHelper.Clamp(VolumeDicionary[i_SoundType] + 0.1f, 0, 1);
        }
        public void DecreaseSoundsMusicVolume(eSoundType i_SoundType)
        {
            foreach (Sound sound in m_Sounds)
            {
                if (sound.SoundType == i_SoundType)
                {
                    sound.SoundVolume = MathHelper.Clamp(sound.SoundVolume - 0.1f, 0, 1);
                }
            }
            VolumeDicionary[i_SoundType] = MathHelper.Clamp(VolumeDicionary[i_SoundType] + 0.1f, 0, 1);
        }

        public void SetSoundByInstanceType(Sound sound)
        {
            sound.SoundVolume = VolumeDicionary[sound.SoundType];
        }
    }
}
