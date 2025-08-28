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
                [typeof(WeaponRow)] = WeaponRow.Deserialize,
                [typeof(CharacterRow)] = CharacterRow.Deserialize,
                [typeof(EnemyRow)] = EnemyRow.Deserialize,
                
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
                    Debug.LogError($"Deserialization failed for {type}: {e.Message}, data: {string.Join(", ", data.Select(kvp => $"{kvp.Key}: {kvp.Value}"))}");
                    throw e;
                }

            }
            throw new ArgumentException($"Unknown type {type.Name}");
        }


    }
    
    
  

}