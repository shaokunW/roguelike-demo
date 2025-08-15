using System.Collections.Generic;
using UnityEngine;

namespace CatAndHuman.UI.card
{
    public enum CardFormat
    {
        Introduction,
        PoolCard,
        ShoppableCard
    }

    public struct CardViewData
    {
        public readonly Sprite icon;
        public readonly string name;
        public readonly string description;
        public readonly List<string> tags;
        public readonly bool isLocked;
        public readonly List<(string, int, string)> attributes;
        public CardFormat cardFormat;
    }
}