using System;
using CatAndHuman.UI.card;

namespace CatAndHuman.UI.select
{
    [Serializable]
    public class UiSelectionState
    {
        public CardViewData SelectedTalent { get; private set; }
        public CardViewData SelectedWeapon { get; private set; }
        
        public bool IsTalentSelected => SelectedTalent.id > 0;
        public bool IsWeaponSelected => SelectedWeapon.id > 0;
        public bool IsGameReady => IsTalentSelected && IsWeaponSelected;
        
        public static event Action<UiSelectionState> OnStateChanged;
        
        
        public void SelectWeapon(CardViewData data)
        {
            SelectedWeapon = data;
            NotifyStateChange();
        }
        
        public void SelectTalent(CardViewData data)
        {
            SelectedTalent = data;
            NotifyStateChange();
        }
        public void SelectTalentAndWeapon(CardViewData talent, CardViewData weapon)
        {
            SelectedTalent = talent;
            SelectedWeapon = weapon;
            NotifyStateChange();
        }
        
        
        public void Reset()
        {
            SelectedTalent = default;
            SelectedWeapon = default;
            NotifyStateChange();
            
        }
        private void NotifyStateChange()
        {
            OnStateChanged?.Invoke(this);
        }

 
        
    }
}