using System.Collections.Generic;
using UnityEngine;

namespace CatAndHuman.AI
{
    [CreateAssetMenu(fileName = "NewAIProfile", menuName = "CatAndHuman/AI Profile")]
    public class AIProfile : ScriptableObject
    {
        [Tooltip("定义怪物的行为序列。执行器会按顺序循环执行列表中的行为。")]
        public List<AIBehavior> Behaviors;
    }
}