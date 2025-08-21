using System;
using System.Collections.Generic;
using CatAndHuman.UI.card;
using CatAndHuman.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace CatAndHuman.UI.select
{
    public enum PageId
    {
        Talent = 0,
        Weapon = 1,
        Options = 2
    }

    public enum Flow
    {
        SelectTalent,
        SelectWeapon,
        StartGame
    }

    public class FlowController : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private SegmentSwitcher switcher;
        [SerializeField] private SelectablePage pageTalent;
        [SerializeField] private SelectablePage pageWeapon;
        [SerializeField] private TopPanel topPanel;

        [Header("Tabs")] [SerializeField] private Button tabTalent;
        [SerializeField] private Button tabWeapon;
        [SerializeField] private Button tabOptions;
        [Header("Buttons")] [SerializeField] private Button startGameButton;

        [Header("Debug Data")]
        [SerializeField] public List<ItemDefinition> talents;
        [SerializeField] public List<ItemDefinition> weapons;
        
        private Flow currentFlow;
        public UiSelectionState CurrentState { get; private set; }
        
        private FlowController()
        {
            CurrentState = new UiSelectionState();
        }
        
        private void Start()
        {
            currentFlow = Flow.SelectTalent;
            pageTalent.Bind(talents);
            pageWeapon.Bind(weapons);
            switcher.SwitchTo((int)PageId.Talent);
        }

        private void Awake()
        {
            // Tab 点击（Button 版）
            tabTalent?.onClick.AddListener(() => RequestGo(PageId.Talent));
            tabWeapon?.onClick.AddListener(() => RequestGo(PageId.Weapon));
            tabOptions?.onClick.AddListener(() => RequestGo(PageId.Options));
            pageWeapon.OnCardChoose += OnWeaponCardChosen;
            pageTalent.OnCardChoose += OnTalentCardChosen;
            startGameButton?.onClick.AddListener(OnGameStart);
        }
        
        private void OnGameStart()
        {
            if (CurrentState.IsGameReady)
            {
                GlobalGameState.Instance.Reset();
                GlobalGameState.Instance.InitializeGameDta();
                GameModeManager.Instance.ChangeGameMode(GameMode.InGame);
            }
        }

        private void RequestGo(PageId target)
        {
            Debug.Log($"RequestGo {target}");
            // 简单守卫：未选天赋时禁止去武器页
            if (target == PageId.Weapon && currentFlow == Flow.SelectTalent)
            {
                ShowToast("请先选择天赋");
                switcher.SwitchTo((int)PageId.Talent);
                return;
            }

            switcher.SwitchTo((int)target);
        }

        private void OnTalentChosen()
        {
            currentFlow = Flow.SelectWeapon;
            switcher.SwitchTo((int)PageId.Weapon);
        }

        private void OnWeaponChosen()
        {
            currentFlow = Flow.StartGame;
            switcher.SwitchTo((int)PageId.Options);
        }
        
        public void OnTalentCardChosen(CardViewData data)
        {
            var currentTalent = CurrentState.SelectedTalent;
            if (currentTalent.id != data.id)
            {
                CurrentState.SelectTalentAndWeapon(data, default);
            }
            OnTalentChosen();
        }
        
        public void OnWeaponCardChosen(CardViewData data)
        {
            var weapon = CurrentState.SelectedWeapon;
            if (weapon.id != data.id)
            {
                CurrentState.SelectWeapon(data);
            }
            OnWeaponChosen();
        }
        
        private void ShowToast(string msg)
        {
            // 这里先用日志代替；有 UI 提示的话在此调用
            Debug.Log($"[Toast] {msg}");
        }
    }
}