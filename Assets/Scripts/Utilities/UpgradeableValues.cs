using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Vampire
{
    public interface IUpgradeableValue
    {
        int UpgradeCount { get; }

        void Register(AbilityManager abilityManager);
        void RegisterInUse();
        void Upgrade();
        string GetUpgradeDescription();
    }

    public abstract class UpgradeableValue<T> : IUpgradeableValue
    {
        [SerializeField] protected T value;
        [SerializeField] protected T[] upgrades;
        protected AbilityManager abilityManager;
        protected int level = 0;

        protected virtual string UpgradeName { get; set; }
        public virtual T Value { get => value; set => this.value = value; }
        public int UpgradeCount { get => upgrades.Length; }
        public UnityEvent OnChanged { get; } = new UnityEvent();

        public void Register(AbilityManager abilityManager) { this.abilityManager = abilityManager; }
        public abstract void RegisterInUse();
        public virtual void Upgrade()
        {
            if (level < upgrades.Length)
                Upgrade(upgrades[level++]); 
        }
        public abstract void Upgrade(T upgrade);
        public abstract string GetUpgradeDescription();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// Upgradeable Floats
    ////////////////////////////////////////////////////////////////////////////////
    public abstract class UpgradeableFloat : UpgradeableValue<float>
    {
        public override void Upgrade(float upgrade)
        {
            value *= (1+upgrade);
            OnChanged.Invoke();
        }

        public override string GetUpgradeDescription()
        {
            if (level >= upgrades.Length || upgrades[level] == 0) return "";
            return DescriptionUtils.GetUpgradeDescription(UpgradeName, upgrades[level]);
        }
    }

    [System.Serializable] public class UpgradeableDamage : UpgradeableFloat
    {
        protected override string UpgradeName => LocalizationSettings.StringDatabase.GetLocalizedString("Upgradeable Values", "Damage");
        public override void RegisterInUse() { abilityManager.DamageUpgradeablesCount++; }
    }
    [System.Serializable] public class UpgradeableDamageRate : UpgradeableFloat
    {
        protected override string UpgradeName => LocalizationSettings.StringDatabase.GetLocalizedString("Upgradeable Values", "Damage Rate");
        public override void RegisterInUse() { abilityManager.FireRateUpgradeablesCount++; }
    }
    [System.Serializable] public class UpgradeableWeaponCooldown : UpgradeableFloat
    {
        protected override string UpgradeName => LocalizationSettings.StringDatabase.GetLocalizedString("Upgradeable Values", "Weapon Cooldown");
        public override void RegisterInUse() { abilityManager.WeaponCooldownUpgradeablesCount++; }
    }
    [System.Serializable] public class UpgradeableRecoveryCooldown : UpgradeableFloat
    {
        protected override string UpgradeName => LocalizationSettings.StringDatabase.GetLocalizedString("Upgradeable Values", "Recovery Cooldown");
        public override void RegisterInUse() { abilityManager.RecoveryCooldownUpgradeablesCount++; }
    }
    [System.Serializable] public class UpgradeableDuration : UpgradeableFloat
    {
        protected override string UpgradeName => LocalizationSettings.StringDatabase.GetLocalizedString("Upgradeable Values", "Duration");
        public override void RegisterInUse() { abilityManager.DurationUpgradeablesCount++; }
    }
    [System.Serializable] public class UpgradeableAOE : UpgradeableFloat
    {
        protected override string UpgradeName => LocalizationSettings.StringDatabase.GetLocalizedString("Upgradeable Values", "AOE");
        public override void RegisterInUse() { abilityManager.AOEUpgradeablesCount++; }
    }
    [System.Serializable] public class UpgradeableKnockback : UpgradeableFloat
    {
        protected override string UpgradeName => LocalizationSettings.StringDatabase.GetLocalizedString("Upgradeable Values", "Knockback");
        public override void RegisterInUse() { abilityManager.KnockbackUpgradeablesCount++; }
    }
    [System.Serializable] public class UpgradeableProjectileSpeed : UpgradeableFloat
    {
        protected override string UpgradeName => LocalizationSettings.StringDatabase.GetLocalizedString("Upgradeable Values", "Projectile Speed");
        public override void RegisterInUse() { abilityManager.ProjectileSpeedUpgradeablesCount++; }
    }
    [System.Serializable] public class UpgradeableRecoveryChance : UpgradeableFloat
    {
        protected override string UpgradeName => LocalizationSettings.StringDatabase.GetLocalizedString("Upgradeable Values", "Recovery Chance");
        public override void RegisterInUse() { abilityManager.RecoveryChanceUpgradeablesCount++; }
    }
    [System.Serializable] public class UpgradeableBleedDamage : UpgradeableFloat
    {
        protected override string UpgradeName => LocalizationSettings.StringDatabase.GetLocalizedString("Upgradeable Values", "Bleed Damage");
        public override void RegisterInUse() { abilityManager.BleedDamageUpgradeablesCount++; }
    }
    [System.Serializable] public class UpgradeableBleedRate : UpgradeableFloat
    {
        protected override string UpgradeName => LocalizationSettings.StringDatabase.GetLocalizedString("Upgradeable Values", "Bleed Rate");
        public override void RegisterInUse() { abilityManager.BleedRateUpgradeablesCount++; }
    }
    [System.Serializable] public class UpgradeableBleedDuration : UpgradeableFloat
    {
        protected override string UpgradeName => LocalizationSettings.StringDatabase.GetLocalizedString("Upgradeable Values", "Bleed Duration");
        public override void RegisterInUse() { abilityManager.BleedDurationUpgradeablesCount++; }
    }
    [System.Serializable] public class UpgradeableMovementSpeed : UpgradeableFloat
    {
        protected override string UpgradeName => LocalizationSettings.StringDatabase.GetLocalizedString("Upgradeable Values", "Movement Speed");
        public override void RegisterInUse() { abilityManager.MovementSpeedUpgradeablesCount++; }
    }
    [System.Serializable] public class UpgradeableRotationSpeed : UpgradeableFloat
    {
        protected override string UpgradeName => LocalizationSettings.StringDatabase.GetLocalizedString("Upgradeable Values", "Rotation Speed");
        public override void RegisterInUse() { abilityManager.RotationSpeedUpgradeablesCount++; }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// Upgradeable Ints
    ////////////////////////////////////////////////////////////////////////////////
    public abstract class UpgradeableInt : UpgradeableValue<int>
    {
        public override void Upgrade(int upgrade)
        {
            value += upgrade;
            OnChanged.Invoke();
        }

        public override string GetUpgradeDescription()
        {
            if (level >= upgrades.Length || upgrades[level] == 0) return "";
            return DescriptionUtils.GetUpgradeDescription(UpgradeName, upgrades[level]);
        }
    }

    [System.Serializable] public class UpgradeableProjectileCount : UpgradeableInt
    {
        [SerializeField] protected int projectilesPer = 1;
        public override int Value { get => projectilesPer * value; }
        protected override string UpgradeName => LocalizationSettings.StringDatabase.GetLocalizedString("Upgradeable Values", "Projectile Count");
        public override void RegisterInUse() { abilityManager.ProjectileCountUpgradeablesCount++; }
        public override string GetUpgradeDescription()
        {
            if (level >= upgrades.Length || upgrades[level] == 0) return "";
            return DescriptionUtils.GetUpgradeDescription(UpgradeName, upgrades[level]*projectilesPer);
        }
    }
    [System.Serializable] public class UpgradeableRecovery : UpgradeableInt
    {
        protected override string UpgradeName => LocalizationSettings.StringDatabase.GetLocalizedString("Upgradeable Values", "Recovery");
        public override void RegisterInUse() { abilityManager.RecoveryUpgradeablesCount++; }
    }
    [System.Serializable] public class UpgradeableArmor : UpgradeableInt
    {
        protected override string UpgradeName => LocalizationSettings.StringDatabase.GetLocalizedString("Upgradeable Values", "Armor");
        public override void RegisterInUse() { abilityManager.ArmorUpgradeablesCount++; }
    }
}