using System;
using UnityEngine;

namespace CatAndHuman
{
    [CreateAssetMenu(fileName = "Effect_Change_Exp", menuName = "CatAndHuman/Effects/Change/Exp")]
    public class ChangeExpEffect : GameplayEffect
    {
        public int amount;

        public override void Apply(object input, object context)
        {
            Debug.Log($"ChangeExpEffect: {amount}");
        }
    }
}