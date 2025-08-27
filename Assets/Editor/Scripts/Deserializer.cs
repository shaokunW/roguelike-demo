using System;
using System.Collections.Generic;
using System.Linq;
using CatAndHuman.Configs.Runtime;
using CatAndHuman.Stat;
using UnityEngine;

namespace CatAndHuman.Editor.Scripts
{
    public static class Deserializer
    {
        private static readonly Dictionary<Type, Func<Dictionary<string, string>, object>> _deserializers;
        static Deserializer()
        {
            _deserializers = new Dictionary<Type, Func<Dictionary<string, string>, object>>
            {
                [typeof(WeaponRow)] = DeserializeWeapon,
                [typeof(CharacterRow)] = DeserializeCharacter
            };
        }

        public static object Deserialize(Type type, Dictionary<string, string> data)
        {
            if (_deserializers.TryGetValue(type, out var deserializer))
            {
                try
                {
                    return deserializer(data);

                }
                catch (Exception e)
                {
                    Debug.LogError($"Deserialization failed for {type}: {e.Message}, data: {data}");
                    throw e;
                }

            }
            throw new ArgumentException($"Unknown type {type.Name}");
        }


        private static WeaponRow DeserializeWeapon(Dictionary<string, string> data)
        {
            return new WeaponRow
            {
                id = int.Parse(data["id"]),
                code = data["code"],
                displayName = data["displayName"],
                description = data["description"],
                icon = data["icon"],
                baseAttackRange = int.Parse(data["baseAttackRange"]),
                baseAttackCooldown = float.Parse(data["baseAttackCooldown"]),
                baseDamage = int.Parse(data["baseDamage"]),
                baseCritChance = float.Parse(data["baseCritChance"]),
                statModifiers = ParseList(data["statModifierCodes"]).Select(StatModifier.Parse).ToList(),
                tags = ParseList(data["tags"])
            };
        }

        private static CharacterRow DeserializeCharacter(Dictionary<string, string> data)
        {
            return new CharacterRow
            {
                // --- Basic Info ---
                id = int.Parse(data["id"]),
                code = data["code"],
                displayName = data["displayName"],
                description = data["description"],
                icon = data["icon"],
        
                // --- Base Stats ---
                baseMaxHp = int.Parse(data["baseMaxHp"]),
                baseSpeed = int.Parse(data["baseSpeed"]),
                baseDamage = int.Parse(data["baseDamage"]),
                baseCritChance = int.Parse(data["baseCritChance"]),
                baseArmor = int.Parse(data["baseArmor"]),
                baseDodge = int.Parse(data["baseDodge"]),
                baseHpRegen = int.Parse(data["baseHpRegen"]),
                baseHarvesting = int.Parse(data["baseHarvesting"]),
                baseAttackRange = int.Parse(data["baseAttackRange"]),
                basePickupRange = int.Parse(data["basePickupRange"]),
        
                // --- List ---
                statModifiers = ParseList(data["statModifierCodes"]).Select(StatModifier.Parse).ToList()
            };
        }

        private static List<string> ParseList(string s)
        {
            return string.IsNullOrEmpty(s)
                ? new List<string>()
                : s.Split(';').ToList();
        }
        
    }
    
    
  

}