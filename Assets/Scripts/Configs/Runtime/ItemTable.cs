using System;
using System.Collections.Generic;
using CatAndHuman.Stat;
using UnityEngine;

namespace CatAndHuman.Configs.Runtime
{
    [Serializable]
    public class ItemRow
    {
        public int id;
        public string code;
        public string displayName;
        public string description;
        public string icon;
        // 基础属性
        public int baseAttackRange;
        public float baseAttackCooldown;
        public int baseDamage;
        public float baseCritChance;
        public List<StatModifier> statModifiers;
        public List<string> tags;
    }
    
    public class ItemTable: ScriptableObject {
        public List<ItemRow> rows = new();
        
        
        public ItemRow FindById(int id)
        {
            return rows.Find(x => x.id == id);
        }
        
        public ItemRow FindByCode(string code)
        {
            return rows.Find(x => x.code == code);
        }
    }
}