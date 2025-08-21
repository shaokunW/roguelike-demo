using System;
using System.Collections.Generic;
using CatAndHuman.UI.card;
using UnityEngine;
using UnityEngine.UI;

namespace CatAndHuman.UI.select
{
    public class SelectablePage: PageBase
    {
        [SerializeField] private GameObject cardPrefab;
        public List<CardView> _cards = new();
        public RectTransform contentRoot;

        public event Action<CardViewData> OnCardChoose;

        public void Bind(List<ItemDefinition> items)
        {
            
            ClearCards();
            if (items == null || items.Count == 0)
            {
                return;
            }
            foreach(var item in items)
            {
                var view = Instantiate(cardPrefab, contentRoot);
                var cardView = view.GetComponent<CardView>();
                var viewData = CardAdapter.Convert(item, false, CardFormat.PoolCard);
                cardView.Bind(viewData);
                cardView.OnChoose += HandleCardChoose;
                _cards.Add(cardView);
            }
        }

        private void HandleCardChoose(CardViewData data)
        {
            OnCardChoose?.Invoke(data);
        }

        private void ClearCards()
        {
            foreach (var cardView in _cards)
            {
                Destroy(cardView.gameObject);
            }
            _cards.Clear();
        }

    }
}