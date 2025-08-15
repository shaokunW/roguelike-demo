using System;
using System.Collections.Generic;
using UnityEngine;

namespace CatAndHuman.UI.select
{
    public enum Pool
    {
        Character,
        Weapon
    }

    public struct UIState
    {
        public Pool pool;
        public int selectedCharacterId; // -1 未选
        public List<int> selectedWeaponIds;
        public static UIState Default => new()
            { pool = Pool.Character, selectedCharacterId = -1, selectedWeaponIds = null };
    }
    
    public sealed class UIStateStore : MonoBehaviour {
        public static UIStateStore I { get; private set; }
        public UIState State { get; private set; } = UIState.Default;
        public event Action<UIState> OnChanged;
        void Awake(){ I=this; }

        public void Mutate(Func<UIState, UIState> fn){
            var next = fn(State);
            if (!next.Equals(State)){ State = next; OnChanged?.Invoke(State); }
        }
    }
}