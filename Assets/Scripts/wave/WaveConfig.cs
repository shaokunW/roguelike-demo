using System;

namespace CatAndHuman
{
    public class WaveConfig
    {
        public static float Duration(int waveIndex)
        {
            if (waveIndex == 20)
            {
                return 90;
            }
            return Math.Min(20 + (waveIndex - 1) * 5, 60);
        }
    }
}