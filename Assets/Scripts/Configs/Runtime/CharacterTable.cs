using System;
using System.Collections.Generic;
using CatAndHuman.Stat;
using UnityEngine;

namespace CatAndHuman.Configs.Runtime
{
    public class CharacterTable : ScriptableObject
    {
        public List<CharacterRow> rows = new();
        
        
        public CharacterRow FindById(int id)
        {
            return rows.Find(x => x.id == id);
        }
    }

    [Serializable]
    public class CharacterRow
    {
        public int id;
        public string code;
        public string displayName;
        public string description;

        public string icon;

        // 基础属性
        public int baseMaxHp;
        public int baseSpeed;
        public int baseDamage;
        public int baseMeleeDamage;
        public int baseRangeDamage;
        public int baseElementalDamage;
        public int baseCritChance;
        public int baseLifeSteal;
        public int baseArmor;
        public int baseDodge;
        public int baseHpRegen;
        public int baseAttackSpeed;
        public int baseHarvesting;
        public int baseAttackRange;
        public int basePickupRange;
        public int baseLuck;
        public List<string> tags;
        public List<StatModifier> statModifiers;
        public int maxWeaponCount;
        public List<string> defaultWeaponCodes;
        public List<string> defaultItemCodes;

    }
}