using UnityEngine;

namespace CatAndHuman.UI.select
{
    public class SegmentSwitcher: MonoBehaviour
    {
        public void OnClickCharacter()
        {
            UIStateStore.I.Mutate(s => { s.pool = Pool.Character; return s; });
        }

        public void OnClickWeapon()
        {
            UIStateStore.I.Mutate(s => { s.pool = Pool.Weapon; return s; });
        }
        
    }
}