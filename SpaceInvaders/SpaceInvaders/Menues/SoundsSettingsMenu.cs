using SpaceInvaders.SpaceInvaders.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using SpaceInvaders.Infrastructure.ObjectModel.Screens;
using SpaceInvaders.Infrastructure.ObjectModel.Menues.MenuItems;
using SpaceInvaders.SpaceInvaders.Sprites;
using SpaceInvaders.Infrastructure.Managers;
using static SpaceInvaders.SpaceInvaders.Services.SoundFactoryMethod;
using static Invaders.Utils;

namespace SpaceInvaders.SpaceInvaders.Menues
{
    public class SoundsSettingsMenu : Menu
    {
        private MultiItem m_BackgroundMusicVol;
        private MultiItem m_SoundsEffectsVol;
        private MultiItem m_ToggleSound;
        private MenuItem m_Done;

        public SoundsSettingsMenu(GameStructure i_Game, GameScreen i_GameScreen) : base(i_Game, i_GameScreen)
        {
            string[] OptionsForVolumeControls = { "0", "10", "20", "30", "40", "50", "60", "70", "80", "90", "100" };
            m_BackgroundMusicVol = new MultiItem(i_Game, SetBackgroundMusicVol, OptionsForVolumeControls)
            {
                FunctionToExecute = SetBackgroundMusicVol,
                MainText = "Background Music Volume:",
                IsLooped = false,
                CurrenOptionIdx  = 5
            };

            m_SoundsEffectsVol = new MultiItem(i_Game, OptionsForVolumeControls)
            {
                FunctionToExecute = SetSoundsEffectsVol,
                MainText = "Sounds Effects Volume:",
                IsLooped = false,
                CurrenOptionIdx = 5
            };

            m_ToggleSound = new MultiItem(i_Game, "On", "Off")
            {
                FunctionToExecute = ToggleSound,
                MainText = "Toggle Sound:"
            };

            m_Done = new MenuItem(i_Game)
            {
                FunctionToExecute = GoToMainMenu,
                Text = "Done"
            };

            Add(m_BackgroundMusicVol);
            Add(m_SoundsEffectsVol);
            Add(m_ToggleSound);
            Add(m_Done);
        }

        private void ToggleSound()
        {
            SoundsManager.ToggleSound();
        }

        private void SetSoundsEffectsVol()
        {
            if (m_SoundsEffectsVol.CurrenOptionIdx > m_SoundsEffectsVol.PreviousOptionIdx)
            {
                SoundsManager.IncreaseSoundsMusicVolume(eSoundType.SoundsEffects);
            }
            else if(m_SoundsEffectsVol.CurrenOptionIdx < m_SoundsEffectsVol.PreviousOptionIdx)
            {
                SoundsManager.DecreaseSoundsMusicVolume(eSoundType.SoundsEffects);
            }
        }

        private void SetBackgroundMusicVol()
        {
            if (m_SoundsEffectsVol.CurrenOptionIdx > m_SoundsEffectsVol.PreviousOptionIdx)
            {
                SoundsManager.IncreaseSoundsMusicVolume(eSoundType.BackgroundMusic);
            }
            else if (m_SoundsEffectsVol.CurrenOptionIdx < m_SoundsEffectsVol.PreviousOptionIdx)
            {
                SoundsManager.DecreaseSoundsMusicVolume(eSoundType.BackgroundMusic);
            }
        }

        private void GoToMainMenu()
        {
            ResetAndNotifyToGoBack();
        }
    }
}