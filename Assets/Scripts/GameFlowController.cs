using System;
using System.Collections;
using CatAndHuman.Stat;
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
        End
    }

    public class GameFlowController: MonoBehaviour
    {
        public RunPhase currentPhase;
        public WaveRuntimeSO waveRuntime;
        public EnemyManager enemyManager;             // 敌人刷怪控制器

        private IEnumerator RunLoop(int waveIndex)
        {
            while (true)
            {
                PreWave(waveIndex);
                if (currentPhase == RunPhase.End) yield break;
                Time.timeScale = 1f;
                yield return DuringWave();
                Time.timeScale = 0f;
                if (currentPhase == RunPhase.End) yield break;
                yield return DuringLootChoose();
                if (currentPhase == RunPhase.End) yield break;
                yield return DuringTalentChoose();
                if (currentPhase == RunPhase.End) yield break;
                yield return DuringShopping();
                if (currentPhase == RunPhase.End) yield break;
                waveIndex++;
            }
        }

        private void PreWave(int waveIndex)
        {
            SetPhase(RunPhase.PreWave);
            // Init Something
            waveRuntime.InitializeBeforeWave(waveIndex, 30);
            Time.timeScale = 1f;
        }

        private IEnumerator DuringWave()
        {
            SetPhase(RunPhase.Wave);
            var task = enemyManager.StartWave();
            while (waveRuntime.timer > 0)
            {
                waveRuntime.timer -= Time.deltaTime;
                yield return null; 
            }
            StopCoroutine(task);
            yield return new WaitForSeconds(0.5f); // 给吸取动画半秒
        }

        private IEnumerator DuringLootChoose()
        {
            SetPhase(RunPhase.LootChoose);
            yield return null;
        }
        
        private IEnumerator DuringTalentChoose()
        {
            SetPhase(RunPhase.TalentChoose);
            yield return null;
        }
        
        private IEnumerator DuringShopping()
        {
            SetPhase(RunPhase.Shopping);

            yield return null;
        }
        
        private IEnumerator HandleGameOver()
        {
            SetPhase(RunPhase.End);
            Time.timeScale = 0f;
            yield break;
        }

        private void SetPhase(RunPhase phase)
        {
            currentPhase = phase;
        }
    }
}