using UnityEngine;

namespace CatAndHuman
{
    
    public abstract class GameplayEffect : ScriptableObject
    {
        public string description;
        
        public abstract void Apply(object target, object context);
    }
}
