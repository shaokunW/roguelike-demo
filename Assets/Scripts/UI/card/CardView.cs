using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CatAndHuman.UI.card
{
    [Serializable]
    public class CardView: MonoBehaviour
    {
        private CardViewData _data;
        [Header("col1")]
        public Image icon;
        public TMP_Text nameText;
        
        [Header("col2")]
        public TMP_Text tagsText;
        public TMP_Text descText;
        public Transform attrListRoot;
        AttributeView attrPrefab;
        List<AttributeView> _attrPool;

        [Header("col3")]
        public GameObject lockIcon;
        public GameObject col3;
        public Button primaryButton;
        public TMP_Text primaryBtnText;
        public Button secondButton;
        public TMP_Text secondBtnText;
        
        [Header("Optional Dynamic Elements")]
        public Image background; // 如果稀有度背景不同才用
        
        public void Bind(CardViewData data)
        {
            _data = data;
            icon.sprite = data.icon;
            nameText.text = data.name ?? "";
            descText.text = data.description ?? "";
            tagsText.text = string.Join(", ", data.tags) ?? "";
            RebuildAttributes(data.attributes);
            
            ApplyFormat(data.cardFormat, data.isLocked);
        }
        
        void RebuildAttributes(List<(string name, int baseV, String unitStr)> attrs)
        {
            ClearAttributes();
            if (attrs == null) return;

            for (int i = 0; i < attrs.Count; i++)
            {
                var (n, b, nv) = attrs[i];
                var item = Instantiate(attrPrefab, attrListRoot);
                item.SetAttr(n, b);
                _attrPool.Add(item);
            }
        }

        
        void ClearAttributes()
        {
            for (int i = 0; i < _attrPool.Count; i++)
                if (_attrPool[i]) Destroy(_attrPool[i].gameObject);
            _attrPool.Clear();
        }
        
        
        void ApplyFormat(CardFormat f, bool isLocked)
        {
            // 样式切换
            switch (f)
            {
                case CardFormat.Introduction:
                    col3.SetActive(false);
                    primaryButton.gameObject.SetActive(false);
                    secondButton.gameObject.SetActive(false);
                    break;

                case CardFormat.PoolCard:
                    col3.SetActive(true);
                    primaryButton.gameObject.SetActive(true);
                    primaryBtnText.text = "选择";
                    primaryButton.interactable = !isLocked;
                    SetLockVisual(isLocked);
                    secondButton.gameObject.SetActive(false);
                    break;

                case CardFormat.ShoppableCard:
                    primaryButton.gameObject.SetActive(true);
                    primaryBtnText.text = "购买";
                    secondButton.gameObject.SetActive(true);
                    secondBtnText.text = "锁定";
                    break;
            }
        }

        void SetLockVisual(bool isLocked)
        {
            lockIcon.SetActive(isLocked);
        }
        
    }
}