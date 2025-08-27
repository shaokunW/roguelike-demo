using CatAndHuman.Configs.Runtime;
using UnityEngine;

namespace CatAndHuman.Stat
{
    [CreateAssetMenu(menuName = "Game Stat/Game Stat", fileName = "GameStat")]

    public class GameStateSO: ScriptableObject
    {
        // player
        public int characterId;
        public float hp;
        public float maxHp;
        public float hpRegen;
        public float lifeSteal;
        public float speed;
        public float damage;
        public float meleeDamage;
        public float rangeDamage;
        public float elementalDamage;
        public float crit;
        public float attackSpeed;
        public float armor;
        public float dodge;
        public float luck;
        public float harvesting;
        public float attackRange;
        public float pickUpRange;
        
        // wave data
        public int wave;
        public int level;
        public int exp;
        public int gold;


        public void Initialize(CharacterRow characterRow)
        {
            characterId = characterRow.id;
            level = 0;
            exp = 0;
            gold = 0;
            maxHp = characterRow.baseMaxHp;
            hp =  characterRow.baseMaxHp;
            hpRegen = characterRow.baseHpRegen;
            lifeSteal = characterRow.baseLifeSteal;
            speed = characterRow.baseSpeed;
            damage = characterRow.baseDamage;
            meleeDamage = characterRow.baseMeleeDamage;
            rangeDamage = characterRow.baseRangeDamage;
            elementalDamage = characterRow.baseElementalDamage;
            crit = characterRow.baseCritChance;
            attackRange = characterRow.baseAttackRange;
            attackSpeed = characterRow.baseAttackSpeed;
            pickUpRange = characterRow.basePickupRange;
            armor = characterRow.baseArmor;
            dodge = characterRow.baseDodge;
            harvesting = characterRow.baseHarvesting;
            pickUpRange = characterRow.basePickupRange;
            luck = characterRow.baseLuck;
            
            wave = 0;
        }

        public void InitializeFromSnapshot()
        {
            
        }
    }
}