using UnityEngine;

namespace CatAndHuman
{
    [CreateAssetMenu(fileName = "EnemyKilledEvent", menuName = "Events/EnemyKilledEvent")]

    public class EnemyKilledEvent : GameEvent<EnemyDiedEventData> { }
    
}