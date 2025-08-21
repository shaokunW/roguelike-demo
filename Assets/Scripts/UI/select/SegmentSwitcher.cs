using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CatAndHuman.UI.select
{
    public class SegmentSwitcher: MonoBehaviour
    {
        [SerializeField] private List<PageBase> pages = new();
        [Header("Tabs")] [SerializeField] private Button tabTalent;
        [SerializeField] private Button tabWeapon;
        [SerializeField] private Button tabOptions;
        [Header("Buttons")] [SerializeField] private Button startGameButton;
        public int CurrentIndex { get; private set; } = -1;

        private IPage _current;
        private bool _switching;
        private Coroutine _running;
        
        private void OnEnable()
        {
            UiSelectionState.OnStateChanged += RefreshUI;
        }

        private void OnDisable()
        {
            UiSelectionState.OnStateChanged -= RefreshUI;

        }
        
        public void SwitchTo(int index, object ctx = null)
        {
            Debug.Log($"Switch to {index}");
            if (_switching || index == CurrentIndex)
            {
                return;
            }

            if (index < 0 || index >= pages.Count)
            {
                return;
            }
            if (_running != null) StopCoroutine(_running);
            _running = StartCoroutine(CoSwitch(index, ctx));
        }

        private IEnumerator CoSwitch(int index, object ctx = null)
        {
            _switching = true;
            IPage next = pages[index];
            if (_current != null)
            {
                yield return _current.Exit();
            }
            _current = next;
            if (_current != null)
            {
                yield return _current.Enter();
            }
            CurrentIndex = index;
            _switching = false;
        }
        
        private void RefreshUI(UiSelectionState state)
        {
            
            tabTalent.interactable = true;
            tabWeapon.interactable = state.IsTalentSelected;
            startGameButton.interactable = state.IsGameReady;
        }
    }
}