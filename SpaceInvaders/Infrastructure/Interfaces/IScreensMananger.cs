using SpaceInvaders.Infrastructure.ObjectModel.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Infrastructure.Interfaces
{
    public interface IScreensMananger
    {
        GameScreen ActiveScreen { get; }
        void SetCurrentScreen(GameScreen i_NewScreen);
        bool Remove(GameScreen i_Screen);
        void Add(GameScreen i_Screen);
        void Push(GameScreen i_GameScreen);
    }
}

