using System.Collections.Generic;
using System.Linq;
using CatAndHuman.Configs.Runtime;
using UnityEngine;

namespace CatAndHuman.debug
{
    public class DebugController: MonoBehaviour
    {
        public WeaponTable weaponTable;
        public WeaponManager weaponManager;
        public List<int> weaponIds;

        public EnemyTable enemyTable;
        public List<int> enemyIds;
        public GameObject enemyPrefab;


        [ContextMenu("Equip Weapons")]
        public void EquipWeapons()
        {
            var weaponRows = weaponIds.Select(weaponTable.FindById).ToList();
            weaponManager.slotsCount = weaponRows.Count;
            weaponManager.GenerateSlots();
            weaponManager.EquipWeapons(weaponRows);
        }

        [ContextMenu("Generate Enemy")]
        public void GenerateEnemy()
        {
            foreach (var id in enemyIds)
            {
                var row = enemyTable.FindById(id);
                var go = Instantiate(enemyPrefab);
                var enemyController = go.GetComponent<EnemyController>();
                enemyController.Initialize(row);
            }

        }

    }
}