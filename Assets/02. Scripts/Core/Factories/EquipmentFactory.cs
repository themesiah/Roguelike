using Laresistance.Battle;
using Laresistance.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Core
{
    public class EquipmentFactory
    {
        public static Equipment GetEquipment(EquipmentData equipmentData, EquipmentEvents events, BattleStatusManager statusManager)
        {
            UnityEngine.Assertions.Assert.IsNotNull(equipmentData.EquipmentEffects);
            UnityEngine.Assertions.Assert.IsTrue(equipmentData.EquipmentEffects.Count > 0);
            Equipment equip = new Equipment((int)equipmentData.Slot, equipmentData, statusManager);
            SetEquipmentModifiers(equipmentData, equip, events);
            return equip;
        }

        private static void SetEquipmentModifiers(EquipmentData data, Equipment equip, EquipmentEvents events)
        {
            foreach(EquipmentEffectData effectData in data.EquipmentEffects)
            {
                switch(effectData.EffectType)
                {
                    case EquipmentEffectType.PowerFlat:
                        equip.SetPowerModifierFlat(events, effectData.EffectModifier1);
                        break;
                    case EquipmentEffectType.Power:
                        equip.SetPowerModifier(events, effectData.EffectModifier1);
                        break;
                    case EquipmentEffectType.AttackPowerFlat:
                        equip.SetAttackPowerModifierFlat(events, effectData.EffectModifier1);
                        break;
                    case EquipmentEffectType.AttackPower:
                        equip.SetAttackPowerModifier(events, effectData.EffectModifier1);
                        break;
                    case EquipmentEffectType.HealPowerFlat:
                        equip.SetHealPowerModifierFlat(events, effectData.EffectModifier1);
                        break;
                    case EquipmentEffectType.HealPower:
                        equip.SetHealPowerModifier(events, effectData.EffectModifier1);
                        break;
                    case EquipmentEffectType.ShieldPowerFlat:
                        equip.SetShieldPowerModifierFlat(events, effectData.EffectModifier1);
                        break;
                    case EquipmentEffectType.ShieldPower:
                        equip.SetShieldPowerModifier(events, effectData.EffectModifier1);
                        break;
                    case EquipmentEffectType.EffectPowerFlat:
                        equip.SetEffectPowerModifierFlat(events, effectData.EffectModifier1);
                        break;
                    case EquipmentEffectType.EffectPower:
                        equip.SetEffectPowerModifier(events, effectData.EffectModifier1);
                        break;
                    case EquipmentEffectType.EnergyProduction:
                        equip.SetEnergyProduction(events, effectData.EffectModifier1);
                        break;
                    case EquipmentEffectType.StartingEnergy:
                        equip.SetStartingEnergy(events, effectData.EffectModifier1);
                        break;
                    case EquipmentEffectType.AttackAbilityBloodCost:
                        equip.SetAttackAbilityBloodCost(events, effectData.EffectModifier1);
                        break;
                    case EquipmentEffectType.MaxHealthModifier:
                        equip.SetMaxHealth(events, effectData.EffectModifier1);
                        break;
                    case EquipmentEffectType.ExtraBlood:
                        equip.SetExtraBlood(events, effectData.EffectModifier1);
                        break;
                    case EquipmentEffectType.BloodLoss:
                        equip.SetBloodLoss(events, effectData.EffectModifier1);
                        break;
                    case EquipmentEffectType.UpgradePriceModifier:
                        equip.SetUpgradePriceModifier(events, effectData.EffectModifier1);
                        break;
                    case EquipmentEffectType.ShopBloodPriceModifier:
                        equip.SetShopPriceModifier(events, effectData.EffectModifier1);
                        break;
                    case EquipmentEffectType.RetaliationFlat:
                        equip.SetRetaliationFlatModifier(events, effectData.EffectModifier1);
                        break;
                    case EquipmentEffectType.DamageReceivedModifierFlat:
                        equip.SetDamageReceivedFlatModifier(events, effectData.EffectModifier1);
                        break;
                    case EquipmentEffectType.DamageReceivedModifier:
                        equip.SetDamageReceivedModifier(events, effectData.EffectModifier1);
                        break;
                    case EquipmentEffectType.AbilityBloodCostFlat:
                    case EquipmentEffectType.AbilityBloodCost:
                    case EquipmentEffectType.AttackAbilityBloodCostFlat:
                    case EquipmentEffectType.HealAbilityBloodCostFlat:
                    case EquipmentEffectType.HealAbilityBloodCost:
                    case EquipmentEffectType.ShieldAbilityBloodCostFlat:
                    case EquipmentEffectType.ShieldAbilityBloodCost:
                    case EquipmentEffectType.EffectAbilityBloodCost:
                    case EquipmentEffectType.Retaliation:
                        throw new System.NotImplementedException(string.Format("Equipment effect type {0} not implemented", effectData.EffectType.ToString()));
                }
            }
        }
    }
}