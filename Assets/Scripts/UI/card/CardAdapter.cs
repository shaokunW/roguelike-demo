using System.Collections.Generic;
using CatAndHuman.Configs.Runtime;
using CatAndHuman.Stat;
using UnityEngine;

namespace CatAndHuman.UI.card
{
    public class CardAdapter
    {
        
        public static CardViewData Convert(ItemDefinition item, bool isLocked) 
        {
            return new CardViewData
            {
                id = item.id,
                iconKey = item.graphicsId.RuntimeKey.ToString(),
                displayName = item.displayName,
                description = item.desc,
                tags = item.tags,
                statModifiers = new(),
                isLocked = isLocked
            };
        }
        
        
        public static CardViewData Convert(WeaponRow item, bool isLocked) 
        {
            return new CardViewData
            {
                id = item.id,
                iconKey = item.icon,
                displayName = item.displayName,
                description = item.description,
                tags = item.tags,
                isLocked = isLocked,
                statModifiers = item.statModifiers,
            };
        }
        
        public static CardViewData Convert(CharacterRow item, bool isLocked) 
        {
            return new CardViewData
            {
                id = item.id,
                iconKey = item.icon,
                displayName = item.displayName,
                description = item.description,
                tags = item.tags,
                isLocked = isLocked,
                statModifiers = item.statModifiers,
            };
        }
        
    }
}