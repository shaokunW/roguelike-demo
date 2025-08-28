using System;
using System.Collections.Generic;
using CatAndHuman.Stat;
using UnityEngine;

namespace CatAndHuman.Configs.Runtime
{
    public class EnemyTable : ScriptableObject
    {
        public List<EnemyRow> rows = new();

        public EnemyRow FindById(int id)
        {
            return rows.Find(x => x.id == id);
        }

        public EnemyRow FindByCode(string code)
        {
            return rows.Find(x => x.code == code);
        }
    }

    [Serializable]
    public class EnemyRow
    {
        public int id;
        public string code;
        public string displayName;
        public string description;

        public string icon;

        // 基础属性
        public int baseHp;
        public int hpPerWave;
        public int baseSpeed;
        public float baseDamage;
        public float damagePerWave;
        public float knockbackResistance;
        public int materialsDropped;
        public float consumableDropRate;
        public float lootCrateDropRate;
        public int firstWave;
        public List<string> tags;

        public static EnemyRow Deserialize(Dictionary<string, string> data)
        {
            return new EnemyRow
            {
                id = DesrUtils.ParseInt(data, "id"),
                code = data["code"],
                displayName = data["displayName"],
                description = data["description"],
                icon = data["icon"],

                baseHp = DesrUtils.ParseInt(data, "baseHp"),
                hpPerWave = DesrUtils.ParseInt(data, "hpPerWave"),
                baseSpeed = DesrUtils.ParseInt(data, "baseSpeed"),
                baseDamage = DesrUtils.ParseFloat(data, "baseDamage"),
                damagePerWave = DesrUtils.ParseFloat(data, "damagePerWave"),
                knockbackResistance = DesrUtils.ParseFloat(data, "knockbackResistance"),
                materialsDropped = DesrUtils.ParseInt(data, "materialsDropped"),
                consumableDropRate = DesrUtils.ParseFloat(data, "consumableDropRate"),
                lootCrateDropRate = DesrUtils.ParseFloat(data, "lootCrateDropRate"),
                firstWave = DesrUtils.ParseInt(data, "firstWave"),
                tags = DesrUtils.ParseList(data, "tags")
            };
        }
    }
}