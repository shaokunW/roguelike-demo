using System.Collections;
using CatAndHuman.Stat;
using UnityEngine;

namespace CatAndHuman
{
    public class WaveManager: MonoBehaviour
    {
        public WaveRuntimeSO waveRuntime;
        
        
        public IEnumerator InitializeWave(int waveIndex)
        {
            waveRuntime.InitializeBeforeWave(waveIndex, WaveConfig.Duration(waveIndex));
            yield break;
        }

        public IEnumerator PrepareNextWave()
        {
            int next = waveRuntime.currentWave + 1;
            yield return InitializeWave(next);
        }

        public IEnumerable StartWave()
        {
            yield return null;
        }

    }
}