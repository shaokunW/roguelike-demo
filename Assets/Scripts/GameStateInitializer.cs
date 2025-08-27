using System.Collections.Generic;
using System.Linq;
using CatAndHuman.Configs.Runtime;
using CatAndHuman.Stat;
using UnityEngine;

namespace CatAndHuman.Utilities
{
    public class GameStateInitializer: MonoBehaviour
    {
        public static GameStateInitializer Instance { get; private set; }

        public GameStateSO gameState;
        public InventorySO inventory;
        public ItemTable itemTable;
        public WeaponTable weaponTable;
        
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void InitializeGameData(CharacterRow characterRow, WeaponRow weapon)
        {
            gameState.Initialize(characterRow);
            List<WeaponRow> weapons = new(); 
            foreach (var row in characterRow.defaultWeaponCodes.Select(code => weaponTable.FindByCode(code)))
            {
                weapons.Add(row);
            }
            weapons.Add(weapon);
            
            List<ItemRow> items = new();
            foreach (var row in characterRow.defaultItemCodes.Select(code => itemTable.FindByCode(code)))
            {
                items.Add(row);
            }
            
            inventory.Initialize(characterRow.maxWeaponCount, weapons, items);
        }

    }
}