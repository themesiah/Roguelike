using Laresistance.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Core
{
    public class EquipmentFactory
    {
        public static Equipment GetEquipment(EquipmentData equipmentData, EquipmentEvents events)
        {
            UnityEngine.Assertions.Assert.IsNotNull(equipmentData.EquipmentEffects);
            UnityEngine.Assertions.Assert.IsTrue(equipmentData.EquipmentEffects.Count > 0);
            Equipment equip = new Equipment((int)equipmentData.Slot, equipmentData);
            SetEquipmentModifiers(equipmentData, equip, events);
            return equip;
        }

        private static void SetEquipmentModifiers(EquipmentData data, Equipment equip, EquipmentEvents events)
        {
            foreach(EquipmentEffectData effectData in data.EquipmentEffects)
            {
                switch(effectData.EffectType)
                {
                    case EquipmentEffectType.Power:
                        equip.SetPowerModifier(events, effectData.EffectModifier1);
                        break;
                    case EquipmentEffectType.Cooldown:
                        equip.SetCooldownModifier(events, effectData.EffectModifier1);
                        break;
                }
            }
        }
    }
}