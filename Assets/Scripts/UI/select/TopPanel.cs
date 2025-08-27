using CatAndHuman.UI.card;
using UnityEngine;

namespace CatAndHuman.UI.select
{
    public class TopPanel: MonoBehaviour
    {
        public CardView talentCard;
        public CardView weaponCard;

        private void OnEnable()
        {
            UiSelectionState.OnStateChanged += OnSelectionStateChanged;
        }
        
        private void OnDisable()
        {
            UiSelectionState.OnStateChanged -= OnSelectionStateChanged;
        }


        private void OnSelectionStateChanged(UiSelectionState state)
        {
            var talent = state.SelectedTalent;
            if (talent.id <= 0)
            {
                talentCard.gameObject.SetActive(false);
            }
            else
            {
                talentCard.Bind(talent, CardFormat.Introduction);
                talentCard.gameObject.SetActive(true);
            }

            var weapon = state.SelectedWeapon;
            if (weapon.id <= 0)
            {
                weaponCard.gameObject.SetActive(false);
            }
            else
            {
                weaponCard.Bind(weapon, CardFormat.Introduction);
                weaponCard.gameObject.SetActive(true);
            }
        }
        
    }
}