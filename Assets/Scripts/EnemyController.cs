using System;
using CatAndHuman.Configs.Runtime;
using UnityEngine;

namespace CatAndHuman
{
    public class EnemyController : MonoBehaviour
    {
        public StatsController statsController;
        public event Action<EnemyController> OnDeactivated;
        public EnemyRow enemyData;
        public SpriteRenderer spriteRenderer;
        public bool killedEventSent;
        public GameEvent<EnemyDiedEventData> DiedEvent;
        
        public async void Initialize(EnemyRow data)
        {
            this.enemyData = data;
            this.killedEventSent = false;
            spriteRenderer.sprite = await SpriteProvider.Instance.AcquireAsync(data.icon);
            statsController._maxHp = data.baseHp;
            statsController._currentHp = data.baseHp;

        }

        // Update is called once per frame
        void Update()
        {
            if (statsController.Die())
            {
                if (!killedEventSent)
                {
                    var diedEvent = new EnemyDiedEventData();
                    diedEvent.EnemyID = enemyData.id;
                    diedEvent.EnemyPosition = transform.position;
                    DiedEvent.Raise(diedEvent);
                    killedEventSent = true;
                    OnDeactivated?.Invoke(this);
                }
            }
        }
    }
}