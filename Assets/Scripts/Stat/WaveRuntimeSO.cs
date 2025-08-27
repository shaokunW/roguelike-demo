using System.Collections.Generic;
using CatAndHuman.Configs.Runtime;
using UnityEngine;

namespace CatAndHuman.Stat
{
    [CreateAssetMenu(menuName = "Game Stat/WaveRuntime", fileName = "WaveRuntime")]
    public class WaveRuntimeSO: ScriptableObject
    {
        public float timer;
        public int currentWave;
        public int shopRollCount;
        public int talentRollCount;
        public List<int> collectedLootIds = new();
        public int remainingGold;

        public void InitializeBeforeWave(int waveIndex, float timerThisWave)
        {
            timer = timerThisWave;
            currentWave = waveIndex;
            shopRollCount = 0;
            talentRollCount = 0;
            collectedLootIds.Clear();
            remainingGold = 0;
        }
    }
}