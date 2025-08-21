using System.Collections.Generic;
using UnityEngine;

namespace CatAndHuman.UI.card
{
    public class CardAdapter
    {
        
        public static CardViewData Convert(ItemDefinition item, bool isLocked, CardFormat cardFormat) 
        {
            return new CardViewData
            {
                id = item.id,
                iconKey = item.graphicsId.RuntimeKey.ToString(),
                displayName = item.displayName,
                description = item.desc,
                tags = item.tags,
                attributes = new List<Attribute>(),
                cardFormat = cardFormat
            };
        }
        
        
    }
}