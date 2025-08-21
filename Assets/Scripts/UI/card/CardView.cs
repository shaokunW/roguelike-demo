using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CatAndHuman.UI.card
{
    [Serializable]
    public class CardView : MonoBehaviour
    {
        private CardViewData _data;
        private SpriteLease _spriteLease;
        [Header("col1")] public Image icon;
        public TMP_Text nameText;

        [Header("col2")] public TMP_Text tagsText;
        public TMP_Text descText;
        public Transform attrListRoot;
        AttributeView attrPrefab;
        List<AttributeView> _attrPool = new();

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

        public async Task Bind(CardViewData data)
        {
            _spriteLease?.Dispose(); 
            _data = data;

            var task = SpriteLease.GetAsync(data.iconKey);
            var (sprite, lease) = await task;
            _spriteLease = lease;
            icon.sprite = sprite;
            nameText.text = data.displayName ?? "";
            descText.text = data.description ?? "";
            tagsText.text = string.Join(", ", data.tags) ?? "";
            RebuildAttributes(data.attributes);
            ApplyFormat(data.cardFormat, data.isLocked);
        }

        void RebuildAttributes(List<Attribute> attrs)
        {
            ClearAttributes();
            if (attrs == null) return;

            for (int i = 0; i < attrs.Count; i++)
            {
                var attr = attrs[i];
                var item = Instantiate(attrPrefab, attrListRoot);
                item.SetAttr(attr.prefix, attr.value, attr.suffix);
                _attrPool.Add(item);
            }
        }


        void ClearAttributes()
        {
            for (int i = 0; i < _attrPool.Count; i++)
                if (_attrPool[i])
                    Destroy(_attrPool[i].gameObject);
            _attrPool.Clear();
        }

        private void OnClickPrimaryBtn()
        {
            Debug.Log($"OnClickPrimaryBtn id={_data.id} name={_data.displayName}");
            switch (_data.cardFormat)
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
            switch (_data.cardFormat)
            {
                case CardFormat.ShoppableCard:
                    OnCardLock?.Invoke(_data);
                    break;
            }
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

        void OnDestroy() => _spriteLease?.Dispose();
        
        void SetLockVisual(bool isLocked)
        {
            lockIcon.SetActive(isLocked);
        }

        public CardViewData GetData()
        {
            return _data;
        }
    }
}