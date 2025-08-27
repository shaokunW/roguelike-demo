using System;
using System.Collections.Generic;
using CatAndHuman.Configs.Runtime;
using UnityEngine;

namespace CatAndHuman.Stat
{
    [CreateAssetMenu(menuName = "Game Stat/Inventory", fileName = "Inventory")]
    public class InventorySO : ScriptableObject
    {
        [Header("Weapon Slots")] [Min(1)] public int maxWeaponCnt = 6;

        [Header("Runtime Lists")] public List<WeaponRow> weapons = new();
        public List<ItemRow> items = new();

        private int WeaponCount => weapons?.Count ?? 0;
        public bool IsFull => WeaponCount >= maxWeaponCnt;
        public int FreeSlots => Mathf.Max(0, maxWeaponCnt - WeaponCount);

        public event Action OnChanged;

        public void Initialize(int maxWeaponCnt,
            List<WeaponRow> weapons,
            List<ItemRow> items)
        {
            this.maxWeaponCnt = maxWeaponCnt;
            if (weapons.Count > maxWeaponCnt)
            {
                throw new ArgumentException("weapon count is larger than max weapon count");
            }

            this.weapons = weapons;
            this.items = items;
        }

        public bool TryAddWeapon(WeaponRow candidate, out string reason)
        {
            reason = null;
            if (candidate == null)
            {
                reason = "Weapon is null.";
                return false;
            }

            if (IsFull)
            {
                if (HasCombinable(candidate, out var idx))
                {
                    weapons[idx] = Combine(weapons[idx], candidate);
                    OnChanged?.Invoke();
                    return true;
                }
                else
                {
                    reason = "No free weapon slot.";
                    return false;
                }
            }

            weapons.Add(candidate);
            OnChanged?.Invoke();
            return true;
        }

        public WeaponRow Combine(WeaponRow a, WeaponRow b)
        {
            //TODO
            return a;
        }

        public WeaponRow CombineWithAnotherSlot(int keepIndex, int otherIndex)
        {
            if (keepIndex != otherIndex && CanCombine(weapons[keepIndex], weapons[otherIndex]))
            {
                weapons[keepIndex] = Combine(weapons[keepIndex], weapons[otherIndex]);
                if (TryRemoveWeaponAt(keepIndex, out var weapon, out string reason))
                {
                    return weapon;
                }
                else
                {
                    throw new ArgumentException(reason);
                }
            }
            else
            {
                throw new ArgumentException("Can not combine weapons with same index");
            }
        }

        // —— 辅助：尝试让 keepIndex 槽与列表中的其它槽合成（仅合一对）——
        public bool TryCombineWithAnotherSlot(int keepIndex, out int otherIndex)
        {
            var a = weapons[keepIndex];
            for (int i = 0; i < weapons.Count; i++)
            {
                if (i == keepIndex) continue;
                if (CanCombine(a, weapons[i]))
                {
                    otherIndex = i;
                    return true;
                }
            }

            otherIndex = -1;
            return false;
        }


        public bool CanAddWeapon(WeaponRow candidate)
        {
            if (candidate == null)
            {
                return false;
            }

            if (!IsFull)
            {
                return true;
            }

            return HasCombinable(candidate, out _);
        }

        public bool TryRemoveWeaponAt(int index, out WeaponRow removed, out string reason)
        {
            removed = null;
            reason = null;

            weapons ??= new List<WeaponRow>();

            if (index < 0 || index >= weapons.Count)
            {
                reason = "Index out of range.";
                return false;
            }

            removed = weapons[index];
            weapons.RemoveAt(index); // 后面的元素左移，保持顺序
            OnChanged?.Invoke();
            return true;
        }


        private bool HasCombinable(WeaponRow row, out int index)
        {
            index = -1;
            for (int i = 0; i < WeaponCount; i++)
            {
                if (CanCombine(row, weapons[i]))
                {
                    index = i;
                    return true;
                }
            }

            return false;
        }

        // 同系列（combineKey）且同等级才能合成；到达上限则不可合成
        private bool CanCombine(WeaponRow a, WeaponRow b)
        {
            return a.id == b.id;
        }

        public void AppendItem(ItemRow item)
        {
            items.Add(item);
            OnChanged?.Invoke();
        }
    }
}