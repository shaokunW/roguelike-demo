using System;
using UnityEngine;

namespace CatAndHuman
{
    [Serializable]
    public class EnemyDiedEventData
    {
        public string EnemyID;
        public Vector3 EnemyPosition;
    }
}