using System;
using System.Collections.Generic;
using CatAndHuman.Stat;
using UnityEngine;
using UnityEngine.Serialization;

namespace CatAndHuman.UI.card
{
    public enum CardFormat
    {
        Introduction,
        PoolCard,
        ShoppableCard
    }

    [Serializable]
    public struct CardViewData
    {
        public int id;
        public string iconKey;
        public string displayName;
        public string description;
        public List<string> tags;
        public bool isLocked;
        public List<StatModifier> statModifiers;
    }

}