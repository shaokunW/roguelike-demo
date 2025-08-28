using System;
using System.Collections;
using CatAndHuman.shop;
using CatAndHuman.Stat;
using CatAndHuman.talent;
using UnityEngine;

namespace CatAndHuman
{
    public enum RunPhase
    {
        PreWave,
        Wave,
        LootChoose,
        TalentChoose,
        Shopping,
        WaveEnd,
        GameOver
    }

    public class WaveFlowController: MonoBehaviour
    {
        public RunPhase currentPhase;
        public WaveManager waveManager;             // 敌人刷怪控制器
        public LootManager lootManager;
        public TalentManager talentManager;
        public ShopManager shopManager;
        private IEnumerator RunLoop(int waveIndex)
        {
            yield return PreWave(waveIndex);
            while (true)
            {
                yield return DuringWave();
                if (currentPhase == RunPhase.GameOver) yield break;
                yield return DuringLootChoose();
                if (currentPhase == RunPhase.GameOver) yield break;
                yield return DuringTalentChoose();
                if (currentPhase == RunPhase.GameOver) yield break;
                yield return DuringShopping();
                if (currentPhase == RunPhase.GameOver) yield break;
                yield return waveManager.PrepareNextWave();
                if (currentPhase == RunPhase.GameOver) yield break;
            }
        }

        private IEnumerable PreWave(int waveIndex)
        {
            SetPhase(RunPhase.PreWave);
            // Init Something
            yield return waveManager.InitializeWave(waveIndex);
        }

        private IEnumerator DuringWave()
        {
            SetPhase(RunPhase.Wave);
            yield return waveManager.StartWave(); // 给吸取动画半秒
        }

        private IEnumerator DuringLootChoose()
        {
            SetPhase(RunPhase.LootChoose);
            yield return lootManager.StartLootChoose();
        }
        
        private IEnumerator DuringTalentChoose()
        {
            SetPhase(RunPhase.TalentChoose);
            yield return talentManager.StartTalentChoose();
        }
        
        private IEnumerator DuringShopping()
        {
            SetPhase(RunPhase.Shopping);

            yield return shopManager.StartShopping();
        }

        private IEnumerator WaveEnd()
        {
            SetPhase(RunPhase.WaveEnd);
            yield return waveManager.PrepareNextWave();
        }

        private IEnumerator HandleGameOver()
        {
            SetPhase(RunPhase.GameOver);
            Time.timeScale = 0f;
            yield break;
        }

        private void SetPhase(RunPhase phase)
        {
            currentPhase = phase;
        }
    }
}