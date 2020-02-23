using Infrastructure;
using Microsoft.Xna.Framework.Audio;
using static Invaders.Utils;

public class Sound
{
    private string m_AssetName;
    private SoundEffectInstance m_SoundEffectInstance;
    //private SoundFactory.eSoundType m_SoundType;
    private bool m_IsMuted;
    private bool IsMuted
    {
        get { return m_IsMuted; }
        set { m_IsMuted = value; }
    }

    private eSoundType m_SoundType;
    public eSoundType SoundType
    {
        get { return m_SoundType; }
        set { m_SoundType = value; }
    }

    public void Play()
    {
        if(!IsMuted)
        {
            m_SoundEffectInstance.Play();
        }
    }

    public bool isLooped
    {
        get { return m_SoundEffectInstance.IsLooped; }
        set { m_SoundEffectInstance.IsLooped = value; }
    }

    public void Resume()
    {
        IsMuted = false;
    }
    public void Pause()
    {
        IsMuted = true;
    }

    public void Stop()
    {
        m_SoundEffectInstance.Stop();
        m_IsMuted = true;
    }

    public float SoundVolume
    {
        get { return m_SoundEffectInstance.Volume; }
        set
        {
            if (value >= 0 && value <= 1)
            {
                m_SoundEffectInstance.Volume = value;
            }
        }
    }

    public Sound(GameStructure i_Game, string i_AssetName, bool i_IsLooped, eSoundType i_SoundType)
    {
        m_AssetName = i_AssetName;
        m_SoundEffectInstance = i_Game.Content.Load<SoundEffect>(i_AssetName).CreateInstance();
        m_SoundEffectInstance.IsLooped = i_IsLooped;
        m_SoundType = i_SoundType;
        SoundVolume = 0.5f;
        IsMuted = false;
    }
}