using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CatAndHuman.Configs.Runtime;
using CatAndHuman.Stat;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace CatAndHuman
{
    [ExecuteAlways]
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager Instance { get; private set; }


        [Header("核心配置")] [SerializeField] public GameObject enemyPrefab;
        public WaveRuntimeSO waveRuntime;
        public EnemyTable enemyTable;

        [SerializeField] private LevelProgressionData levelProgression;
        [SerializeField] private TargetFinder targetFinder;

        // --- 游戏状态 ---
        private ObjectPool<EnemyController> _enemyPool;


        void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                _enemyPool = new ObjectPool<EnemyController>(
                    createFunc: () =>
                    {
                        var o = Instantiate(enemyPrefab);
                        var controller = o.GetComponent<EnemyController>();
                        controller.OnDeactivated += _enemyPool.Release;
                        return controller;
                    },
                    actionOnGet: (controller) => controller.gameObject.SetActive(true),
                    actionOnRelease: (controller) => controller.gameObject.SetActive(false),
                    actionOnDestroy: (controller) => Destroy(controller.gameObject),
                    collectionCheck: true,
                    defaultCapacity: 1000,
                    maxSize: 1000);
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }


        /// <summary>
        /// 启动一个指定的波次。
        /// </summary>
        /// <param name="waveNumber">要启动的波次数</param>
        [ContextMenu("Start Wave")]
        public Coroutine StartWave()
        {
            Debug.Log($"GameManager: 准备第 {waveRuntime.currentWave} 波的数据。");

            // 使用固定的种子来保证刷怪顺序的可复现性
            Random.InitState(waveRuntime.currentWave);

            // 1. 生成怪物列表
            List<EnemyGenerationData> spawnList = GenerateSpawnListForCurrentWave();

            // 2. 将所有数据打包成“任务单”
            WaveData waveData = new WaveData(
                spawnList,
                levelProgression.spawnRateOverTime,
                levelProgression.minSpawnRadius,
                levelProgression.maxSpawnRadius
            );

            Transform target = targetFinder.CurrentTargets.FirstOrDefault();
            return StartCoroutine(SpawnEnemiesCoroutine(target, waveData));
        }

        private List<EnemyGenerationData> GenerateSpawnListForCurrentWave()
        {
            var list = new List<EnemyGenerationData>();
            float budget = levelProgression.budgetPerWave.Evaluate(waveRuntime.currentWave);

            while (budget > 0)
            {
                float totalWeight = 0;
                foreach (var enemy in levelProgression.availableEnemies)
                {
                    totalWeight += enemy.spawnWeightCurve.Evaluate(waveRuntime.currentWave);
                }

                if (totalWeight <= 0) break;

                float randomValue = Random.Range(0, totalWeight);
                float weightSum = 0;
                EnemyGenerationData chosenEnemy = null;

                foreach (var enemy in levelProgression.availableEnemies)
                {
                    weightSum += enemy.spawnWeightCurve.Evaluate(waveRuntime.currentWave);
                    if (randomValue <= weightSum)
                    {
                        chosenEnemy = enemy;
                        break;
                    }
                }

                if (chosenEnemy != null && chosenEnemy.threatCost > 0)
                {
                    list.Add(chosenEnemy);
                    budget -= chosenEnemy.threatCost;
                }
                else
                {
                    break;
                }
            }

            return list;
        }

        private IEnumerator SpawnEnemiesCoroutine(Transform t, WaveData waveData)
        {
            int spawnedCount = 0;
            var spawnList = waveData.EnemiesToSpawn;
            float waveStartTime = Time.time;

            while (spawnedCount < spawnList.Count)
            {
                // 1. 计算刷怪速率和延迟
                // 注意：这里需要GameManager来传递波次的总时长，或者我们在这里假设一个值
                // 为了让Spawner更纯粹，我们先简化处理
                float timeProgress = Mathf.Clamp01((Time.time - waveStartTime) / 60f); // 假设波长60秒
                float currentSpawnRate = waveData.SpawnRateOverTime.Evaluate(timeProgress);
                float delay = 0.5f / (currentSpawnRate + 0.1f);
                yield return new WaitForSeconds(delay);


                // 3. 生成敌人
                EnemyGenerationData enemyToSpawn = spawnList[spawnedCount];
                var enemyRow = enemyTable.FindById(enemyToSpawn.id);
                Vector2 spawnPosition = GetSpawnPosition(t, waveData.MinSpawnRadius, waveData.MaxSpawnRadius);
                Spawn(spawnPosition, enemyRow);
                spawnedCount++;
            }
        }

        private void Spawn(Vector2 position, EnemyRow data)
        {
            var controller = GetFromPool(position, Quaternion.identity);
            controller.Initialize(data);
        }

        //TODO should be a strategy passed from upper class[Relies on Map]
        private Vector2 GetSpawnPosition(Transform t, float minRadius, float maxRadius)
        {
            if (t == null) return Vector2.zero;

            Vector2 playerPos = t.position;
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float radius = Random.Range(minRadius, maxRadius);

            return playerPos + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        }

        private EnemyController GetFromPool(Vector2 pos, Quaternion quaternion)
        {
            var o = _enemyPool.Get();
            o.transform.rotation = quaternion;
            o.transform.position = pos;
            return o;
        }
    }
}