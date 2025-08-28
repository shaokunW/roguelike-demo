using System;
using UnityEngine;

namespace CatAndHuman
{
    [Serializable]
    public class EnemyDiedEventData
    {
        public int EnemyID;
        public Vector3 EnemyPosition;
    }
}