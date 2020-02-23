using SpaceInvaders.Infrastructure.ObjectModel.Screens;
using SpaceInvaders.SpaceInvaders.Sprites;
using SpaceInvaders.SpaceInvaders.Menues;
using Infrastructure;

namespace SpaceInvaders.SpaceInvaders.Screens
{
    public class SoundsSettingsScreen : GameScreen
    {
        private SoundsSettingsMenu m_ScreenSettingsMenu;
        private const string k_AssteNameForBackground = @"Sprites/BG_Space01_1024x768";

        public SoundsSettingsScreen(GameStructure i_Game) : base(i_Game)
        {
            this.IsOverlayed = false;
            m_ScreenSettingsMenu = new SoundsSettingsMenu(i_Game, this);
            m_Background = new Background(i_Game, k_AssteNameForBackground, 1);

            m_ScreenSettingsMenu.NotifyToGoBack += () => ExitScreen();

            Add(m_Background);
            Add(m_ScreenSettingsMenu);
        }
    }
}