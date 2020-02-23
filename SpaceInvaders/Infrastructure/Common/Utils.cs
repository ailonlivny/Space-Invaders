using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Invaders
{
    public class Utils
    {
        private static Random s_Random;

        public static int CalculateRandom(int i_Range)
        {
            s_Random = new Random();

            return s_Random.Next(i_Range);
        }

        public enum ePlayerNumber
        {
            P1,
            P2
        }

        public enum eDirection
        {
            Up,
            Down
        }
        public enum eSoundType
        {
            BackgroundMusic,
            SoundsEffects,
        }
    }
}