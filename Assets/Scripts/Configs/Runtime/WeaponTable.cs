using System;
using System.Collections.Generic;
using System.Linq;
using CatAndHuman.Stat;
using UnityEngine;

namespace CatAndHuman.Configs.Runtime
{
    public class WeaponTable : ScriptableObject
    {
        public List<WeaponRow> rows = new();

        public WeaponRow FindById(int id)
        {
            return rows.Find(x => x.id == id);
        }

        public WeaponRow FindByCode(string code)
        {
            return rows.Find(x => x.code == code);
        }
    }

    [Serializable]
    public class WeaponRow
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
        // public List<BulletLauncher> bulletLaunchers;

        public static WeaponRow Deserialize(Dictionary<string, string> data)
        {
            return new WeaponRow
            {
                id = DesrUtils.ParseInt(data, "id"),
                code = data["code"],
                displayName = data["displayName"],
                description = data["description"],
                icon = data["icon"],
                baseAttackRange = DesrUtils.ParseInt(data, "baseAttackRange"),
                baseAttackCooldown = DesrUtils.ParseFloat(data, "baseAttackCooldown"),
                baseDamage = DesrUtils.ParseInt(data, "baseDamage"),
                baseCritChance = DesrUtils.ParseFloat(data, "baseCritChance"),
                statModifiers = DesrUtils.ParseList(data, "statModifierCodes").Select(StatModifier.Parse).ToList(),
                tags = DesrUtils.ParseList(data["tags"])
            };
        }
    }
}