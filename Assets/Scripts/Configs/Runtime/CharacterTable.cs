using System;
using System.Collections.Generic;
using System.Linq;
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
        public float baseCritChance;
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
        
        public static CharacterRow Deserialize(Dictionary<string, string> data)
        {
            return new CharacterRow
            {
                // --- Basic Info ---
                id = DesrUtils.ParseInt(data,"id"),
                code = data["code"],
                displayName = data["displayName"], 
                description = data["description"],
                icon = data["icon"],
        
                // --- Base Stats ---
                baseMaxHp = DesrUtils.ParseInt(data,"baseMaxHp"),
                baseSpeed = DesrUtils.ParseInt(data,"baseSpeed"),
                baseDamage = DesrUtils.ParseInt(data,"baseDamage"),
                baseCritChance = DesrUtils.ParseFloat(data,"baseCritChance"),
                baseArmor = DesrUtils.ParseInt(data,"baseArmor"),
                baseDodge = DesrUtils.ParseInt(data,"baseDodge"),
                baseHpRegen = DesrUtils.ParseInt(data,"baseHpRegen"),
                baseHarvesting = DesrUtils.ParseInt(data,"baseHarvesting"),
                baseAttackRange = DesrUtils.ParseInt(data,"baseAttackRange"),
                basePickupRange = DesrUtils.ParseInt(data,"basePickupRange"),
                // --- List ---
                statModifiers = DesrUtils.ParseList(data,"statModifierCodes").Select(StatModifier.Parse).ToList(),
                defaultWeaponCodes = DesrUtils.ParseList(data,"defaultWeaponCodes"),
                defaultItemCodes = DesrUtils.ParseList(data,"defaultItemCodes"),
                tags = DesrUtils.ParseList(data,"tags"),

            };
        }
    }
}