using GamedevsToolbox.ScriptableArchitecture.Values;
using System.Runtime.CompilerServices;
using UnityEngine.Events;

namespace Laresistance.Core
{
    public class EquipmentEvents
    {
        public delegate void OnGetPowerHandler(ref int currentPower);
        // Power Flat
        public OnGetPowerHandler OnGetPowerFlat;
        // Power
        public OnGetPowerHandler OnGetPower;
        // Attack Power Flat
        public OnGetPowerHandler OnGetAttackPowerFlat;
        // Attack Power
        public OnGetPowerHandler OnGetAttackPower;
        // Heal Power Flat
        public OnGetPowerHandler OnGetHealPowerFlat;
        // Heal Power
        public OnGetPowerHandler OnGetHealPower;
        // Shield Power Flat
        public OnGetPowerHandler OnGetShieldPowerFlat;
        // Shield power
        public OnGetPowerHandler OnGetShieldPower;
        // Effect power flat
        public OnGetPowerHandler OnGetEffectPowerFlat;
        // Effect power
        public OnGetPowerHandler OnGetEffectPower;
        // Cooldown
        public delegate void OnGetCooldownHandler(ref float currentCooldown);
        public OnGetCooldownHandler OnGetCooldown;
        // Starting cooldowns
        public OnGetCooldownHandler OnGetStartingCooldowns;
        // Ability blood cost
        public delegate void OnGetAbilityBloodCostHandler(ScriptableIntReference bloodRef);
        public OnGetAbilityBloodCostHandler OnGetAbilityBloodCost;
        // Ability blood cost flat
        public OnGetAbilityBloodCostHandler OnGetAbilityBloodCostFlat;
        // Attack ability blood cost flat
        public OnGetAbilityBloodCostHandler OnGetAttackAbilityBloodCostFlat;
        // Attack ability blood cost
        public OnGetAbilityBloodCostHandler OnGetAttackAbilityBloodCost;
        // Heal ability blood cost flat
        public OnGetAbilityBloodCostHandler OnGetHealAbilityBloodCostFlat;
        // Heal ability blood cost
        public OnGetAbilityBloodCostHandler OnGetHealAbilityBloodCost;
        // Shield ability blood cost flat
        public OnGetAbilityBloodCostHandler OnGetShieldAbilityBloodCostFlat;
        // Shield ability blood cost
        public OnGetAbilityBloodCostHandler OnGetShieldAbilityBloodCost;
        // Effect ability blood cost
        public OnGetAbilityBloodCostHandler OnGetEffectAbilityBloodCost;
        // Max health modifier
        public delegate void OnGetMaxHealthHandler(ref int health);
        public OnGetMaxHealthHandler OnGetMaxHealth;
        // Extra blood
        public delegate void OnGetExtraBloodHandler(ref int blood);
        public OnGetExtraBloodHandler OnGetExtraBlood;
        // Blood loss per second
        public delegate void OnBloodLossPerSecondHandler(ref int bloodRef);
        public OnBloodLossPerSecondHandler OnBloodLossPerSecond;
        // Upgrade price modifier
        public delegate void OnShopPriceHandler(ref int price);
        public OnShopPriceHandler OnUpgradePrice;
        // Shop blood price modifier
        public OnShopPriceHandler OnShopPrice;
        // Retaliation flat
        public delegate void OnRetaliationDamageHandler(ref int received, ref int caused);
        public OnRetaliationDamageHandler OnRetaliationDamageFlat;
        // Retaliation
        public OnRetaliationDamageHandler OnRetaliationDamage;
        // Damage received modifier flat
        public delegate void OnDamageReceivedModifierHandler(ref int received);
        public OnDamageReceivedModifierHandler OnDamageReceivedModifierFlat;
        // Damage received modifier
        public OnDamageReceivedModifierHandler OnDamageReceivedModifier;
    }
}