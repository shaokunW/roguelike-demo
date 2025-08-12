
using UnityEngine;

namespace CatAndHuman
{
    public interface BulletAbility 
    {
      public void OnMove(IBulletOwner owner,  Transform pos);
      public void OnHit(IBulletOwner owner, Collider2D hitTarget, Transform hitPoint);
    }
    
}
