using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatAndHuman.Stat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CatAndHuman.UI.card
{
    [Serializable]
    public class CardView : MonoBehaviour
    {
        public Color posColor = new(0.35f, 0.63f, 0.42f);
        public Color negColor = new(0.79f, 0.32f, 0.32f);
        [SerializeField] private CardViewData _data;
        [SerializeField] private CardFormat _cardFormat;
        private SpriteLease _spriteLease;
        [Header("col1")] public Image icon;
        public TMP_Text nameText;

        [Header("col2")] public TMP_Text mainTextBlock;

        [Header("col3")] public GameObject lockIcon;
        public GameObject col3;
        public Button primaryButton;
        public TMP_Text primaryBtnText;
        public Button secondButton;
        public TMP_Text secondBtnText;

        public event Action<CardViewData> OnBuy;
        public event Action<CardViewData> OnCardLock;
        public event Action<CardViewData> OnChoose;


        [Header("Optional Dynamic Elements")] public Image background; // 如果稀有度背景不同才用

        private void Awake()
        {
            primaryButton.onClick.AddListener(OnClickPrimaryBtn);
            secondButton.onClick.AddListener(OnClickSecondBtn);
        }

        public async Task Bind(CardViewData data, CardFormat cardFormat)
        {
            _spriteLease?.Dispose(); 
            _data = data;
            _cardFormat = cardFormat;
            var task = SpriteLease.GetAsync(data.iconKey);
            var (sprite, lease) = await task;
            _spriteLease = lease;
            icon.sprite = sprite;
            nameText.text = data.displayName ?? "";
            mainTextBlock.text = BuildMainText(data);
            ApplyFormat(data.isLocked);
        }
        
        
        private string BuildMainText(CardViewData data)
        {
            var sb = new StringBuilder();

            // 1. Build Tags section
            if (data.tags != null && data.tags.Count > 0)
            {
                // Example using a predefined style for tags.
                // You can define "TagStyle" in a TMP Style Sheet.
                sb.Append("<style=\"H3\">"); 
                sb.Append(string.Join(" ", data.tags));
                sb.Append("</style>");
            }

            // 2. Build Description section
            if (!string.IsNullOrEmpty(data.description))
            {
                // Add spacing if there were tags before the description.
                if (sb.Length > 0)
                {
                    sb.AppendLine(); // Two lines for a clear visual break
                }
                sb.Append(data.description);
            }

            // 3. Build Stats section
            if (data.statModifiers != null && data.statModifiers.Count > 0)
            {
                // Add spacing if there was text before the stats.
                if (sb.Length > 0)
                {
                    // Using a line-height tag gives more control over spacing than just newlines.
                    sb.AppendLine();
                }
                
                for (int i = 0; i < data.statModifiers.Count; i++)
                {
                    if (i > 0)
                    {
                        sb.AppendLine();
                    }
                    sb.Append(BuildStat(data.statModifiers[i]));
                }
            }
            
            return sb.ToString();
        }

        private string BuildStats(List<StatModifier> attrs)
        {
            if (attrs == null || attrs.Count == 0)
            {
                return "";
            }
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < attrs.Count; i++)
            {
                if (i > 0)
                {
                    stringBuilder.AppendLine(); // Use AppendLine for the correct newline character.
                }
                stringBuilder.Append(BuildStat(attrs[i]));
            }
            return stringBuilder.ToString();
        }

        private string BuildStat(StatModifier modifier)
        {
            float value = modifier.value;
            Color valueColor = value >= 0 ? posColor : negColor;
            string colorHex = ColorUtility.ToHtmlStringRGB(valueColor);

            string unit = modifier.modifier switch
            {
                ModifierType.Percentage => "%",
                ModifierType.AdditivePercentage => "%",
                _ => ""
            };

            string statName = modifier.stat.ToString();
            string valueString = value.ToString("+#;-#;0") + unit;
            return $"{statName}: <color=#{colorHex}>{valueString}</color>";
        }

        private void OnClickPrimaryBtn()
        {
            Debug.Log($"OnClickPrimaryBtn id={_data.id} name={_data.displayName}");
            switch (_cardFormat)
            {
                case CardFormat.PoolCard:
                    OnChoose?.Invoke(_data);
                    break;

                case CardFormat.ShoppableCard:
                    OnBuy?.Invoke(_data);
                    break;
            }
        }

        private void OnClickSecondBtn()
        {
            Debug.Log($"OnClickSecondBtn id={_data.id} name={_data.displayName}");
            switch (_cardFormat)
            {
                case CardFormat.ShoppableCard:
                    OnCardLock?.Invoke(_data);
                    break;
            }
        }


        void ApplyFormat(bool isLocked)
        {
            // 样式切换
            switch (_cardFormat)
            {
                case CardFormat.Introduction:
                    col3.SetActive(false);
                    primaryButton.gameObject.SetActive(false);
                    secondButton.gameObject.SetActive(false);
                    lockIcon.SetActive(false);
                    break;

                case CardFormat.PoolCard:
                    col3.SetActive(true);
                    primaryButton.gameObject.SetActive(true);
                    primaryBtnText.text = "选择";
                    primaryButton.interactable = !isLocked;
                    lockIcon.SetActive(isLocked);
                    secondButton.gameObject.SetActive(false);
                    break;

                case CardFormat.ShoppableCard:
                    col3.SetActive(true);
                    lockIcon.SetActive(false);
                    primaryButton.gameObject.SetActive(true);
                    primaryBtnText.text = "购买";
                    secondButton.gameObject.SetActive(true);
                    secondBtnText.text = "锁定";
                    break;
            }
        }

        void OnDestroy() => _spriteLease?.Dispose();

        public CardViewData GetData()
        {
            return _data;
        }
    }
}