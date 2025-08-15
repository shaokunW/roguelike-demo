using System;
using System.Collections.Generic;
using UnityEngine;

namespace CatAndHuman
{
    [CreateAssetMenu(fileName = "RewardContainer_", menuName = "CatAndHuman/Reward/Container")]
    public class RewardContainer: ScriptableObject
    {
        public int id;
        public List<RewardSlot> slots;
    }
    
    [Serializable]
    public class RewardSlot
    {
        public float Weight;
        public int Cnt;
        public int ItemId;
        public bool isContainer;
    }
}
