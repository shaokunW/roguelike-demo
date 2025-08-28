using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CatAndHuman.Configs.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace CatAndHuman
{
    [ExecuteInEditMode]
    public class WeaponManager : MonoBehaviour
    {
        // --- From WeaponSlotsInitializer ---
        public int slotsCount = 1;
        public float distributionRadius = 0.5f;
        public Transform parentTransform;
        public GameObject weaponPrefab;
        [SerializeField] private TargetFinder targetFinder;

        public List<WeaponController> availableWeapons = new();

        [ContextMenu("Generate Slots")]   // 组件右键菜单里会出现这个项
        public void GenerateSlots()
        {
            if (availableWeapons.Count != slotsCount)
            {
                CleanupSlots();
                for (int i = 0; i < slotsCount; i++)
                {
                    availableWeapons.Add(Instantiate(weaponPrefab, parentTransform).GetComponent<WeaponController>());
                }
            }
            float angleStep = 360f / slotsCount;
            for (int i = 0; i < slotsCount; i++)
            {
                float degrees = i * angleStep;
                float radians = degrees * Mathf.Deg2Rad;
                float x = Mathf.Cos(radians) * distributionRadius;
                float y = Mathf.Sin(radians) * distributionRadius;
                var slot = availableWeapons[i];
                slot.transform.localPosition = new Vector3(x, y, 0);
                slot.transform.localRotation = Quaternion.identity;
            }
        }


        private void CleanupSlots()
        {
            for (int i = availableWeapons.Count - 1; i >= 0; i--)
            {
                var controller = availableWeapons[i];
                var go = controller.gameObject;
                if (!go) continue;                    // 兼容“假 null”
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    DestroyImmediate(go);
                }
                else
#endif
                {
                    Destroy(go);
                }
            }
            availableWeapons.Clear();
        }

        void Update()
        {
            // The rest of the Update method remains the same...
            List<Transform> currentTargets = targetFinder.CurrentTargets;
            Transform currentTarget = currentTargets.FirstOrDefault();

            foreach (var weapon in availableWeapons)
            {
                weapon.TickCooldown(Time.deltaTime);

                if (currentTarget != null)
                {
                    Vector2 directionToTarget = (currentTarget.position - weapon.transform.position).normalized;
                    weapon.Aim(directionToTarget);

                    if (weapon.CanFire())
                    {
                        float finalAttackRange = attackRange(weapon);

                        if (Vector2.Distance(weapon.transform.position, currentTarget.position) <= finalAttackRange)
                        {
                            float finalFireInterval = attackSpeed(weapon);
                            DamageAbility damage = new DamageAbility(1, 1, 1);
                            weapon.Fire(directionToTarget, finalFireInterval, targetFinder.GetLayerMask(),
                                finalAttackRange, damage);
                        }
                    }
                }
            }
        }

        public float attackRange(WeaponController weapon)
        {
            return weapon.data.baseAttackRange;
        }

        public float attackSpeed(WeaponController weapon)
        {
            return weapon.data.baseAttackCooldown;
        }

        public void EquipWeapons(List<WeaponRow> weaponRows)
        {
            if (availableWeapons.Count < weaponRows.Count)
            {
                throw new ArgumentException("Not Enough Weapon Slots");
            }

            for (int i = 0; i < availableWeapons.Count; i++)
            {
                var controller = availableWeapons[i];
                var data = i < weaponRows.Count ? weaponRows[i] : null;
                if (data == null)
                {
                    controller.gameObject.SetActive(false);
                }
                else
                {
                    controller.Initialize(data);
                    controller.gameObject.SetActive(true);
                }
            }
        }


        private void CleanWeaponSlotImmediate(Transform slot)
        {
            for (int i = slot.childCount - 1; i >= 0; i--)
            {
                var child = slot.GetChild(i);
                if (child.name.StartsWith("Weapon"))
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }
    }
}