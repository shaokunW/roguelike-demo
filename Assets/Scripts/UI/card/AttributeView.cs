using TMPro;
using UnityEngine;

namespace CatAndHuman.UI.card
{
    public class AttributeView: MonoBehaviour
    {
        public TMP_Text left;   
        public TMP_Text right; 
        public Color pos = new(0.35f,0.63f,0.42f); // #5AA16C
        public Color neg = new(0.79f,0.32f,0.32f); // #C95151

        public void SetAttr(string prefix, int baseV, string unit)
        {
            left.text = prefix;
            right.text = (baseV >= 0 ? $"+{baseV}" : $"-{baseV}") + unit;
            right.color = baseV >= 0 ? pos : neg;
        }
    }
}