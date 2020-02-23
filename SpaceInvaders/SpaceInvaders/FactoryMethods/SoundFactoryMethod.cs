using Infrastructure;
using SpaceInvaders.Infrastructure.Managers;
using static Invaders.Utils;

namespace SpaceInvaders.SpaceInvaders.Services
{
    public class SoundFactoryMethod
    {
        public enum eSoundName
        {
            BarrierHit,
            BackgroundMusic,
            EnemyGunShot,
            EnemyKill,
            GameOver,
            LevelWin,
            LifeDie,
            MenuMove,
            MotherShipKill,
            SSGunShot,
        }

        private const string k_BarrierHitAsset = @"Sounds\BarrierHit";
        private const string k_BackgroundMusicAsset = @"Sounds\BGMusic";
        private const string k_EnemyGunShotAsset = @"Sounds\EnemyGunShot";
        private const string k_EnemyKillAsset = @"Sounds\EnemyKill";
        private const string k_GameOverAsset = @"Sounds\GameOver";
        private const string k_LevelWinAsset = @"Sounds\LevelWin";
        private const string k_LifeDieAsset = @"Sounds\LifeDie";
        private const string k_MenuMoveAsset = @"Sounds\MenuMove";
        private const string k_MotherShipKillAsset = @"Sounds\MotherShipKill";
        private const string k_SSGunShotAsset = @"Sounds/SSGunShot";

        public static Sound CreateSound(GameStructure i_Game, eSoundName i_eSoundType)
        {
            SoundsManager m_soundManager = i_Game.Services.GetService(typeof(SoundsManager)) as SoundsManager;
            Sound sound = null;

            switch (i_eSoundType)
            {
                case eSoundName.BackgroundMusic:
                    sound = new Sound(i_Game, k_BackgroundMusicAsset, true, eSoundType.BackgroundMusic);
                    break;
                case eSoundName.BarrierHit:
                    sound = new Sound(i_Game, k_BarrierHitAsset, false, eSoundType.SoundsEffects);
                    break;
                case eSoundName.EnemyGunShot:
                    sound = new Sound(i_Game, k_EnemyGunShotAsset, false, eSoundType.SoundsEffects);
                    break;
                case eSoundName.EnemyKill:
                    sound = new Sound(i_Game, k_EnemyKillAsset, false, eSoundType.SoundsEffects);
                    break;
                case eSoundName.GameOver:
                    sound = new Sound(i_Game, k_GameOverAsset, false, eSoundType.SoundsEffects);
                    break;
                case eSoundName.LevelWin:
                    sound = new Sound(i_Game, k_LevelWinAsset, false, eSoundType.SoundsEffects);
                    break;
                case eSoundName.LifeDie:
                    sound = new Sound(i_Game, k_LifeDieAsset, false, eSoundType.SoundsEffects);
                    break;
                case eSoundName.MenuMove:
                    sound = new Sound(i_Game, k_MenuMoveAsset, false, eSoundType.SoundsEffects);
                    break;
                case eSoundName.MotherShipKill:
                    sound = new Sound(i_Game, k_MotherShipKillAsset, false, eSoundType.SoundsEffects);
                    break;
                case eSoundName.SSGunShot:
                    sound = new Sound(i_Game, k_SSGunShotAsset, false, eSoundType.SoundsEffects);
                    break;
            }

            m_soundManager.SetSoundByInstanceType(sound);
            m_soundManager.AddSound(sound);
            return sound;
        }
    }
}