using UnityEngine.Events;
using Laresistance.Data;
using Laresistance.Equipments;
using System.Collections.Generic;
using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using Laresistance.Battle;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Core
{
    public class Equipment : ShowableElement, ISlot
    {
        private bool equiped = false;
        private BattleStatusManager statusManager;
        private List<string> descriptionReferences;
        private List<string> descriptionVariables;
        public EquipmentData Data { get; private set; }
        public string Name => Texts.GetText(Data.EquipmentNameReference);

        public Equipment(EquipmentData equipmentData, BattleStatusManager statusManager)
        {
            this.Data = equipmentData;
            this.statusManager = statusManager;
            descriptionReferences = new List<string>();
            descriptionVariables = new List<string>();
        }

        public bool SetInSlot(Player player, int slot)
        {
            return player.EquipEquipment(this, slot);
        }

        public void EquipEquipment()
        {
            UnityEngine.Assertions.Assert.IsFalse(equiped);
            equiped = true;
        }

        public void UnequipEquipment()
        {
            UnityEngine.Assertions.Assert.IsTrue(equiped);
            equiped = false;
        }

        public string GetEquipmentEffectDescription()
        {
            return Texts.GetText(Data.EquipmentDescriptionReference);
        }

        public int ModifyValue(EquipmentSituation cond, int originalValue, int priority)
        {
            int value = originalValue;
            foreach (var effect in Data.EquipmentEffects)
            {
                value = effect.ModifyValue(cond, value, priority);
            }
            return value;
        }

        public float ModifyValue(EquipmentSituation cond, float originalValue, int priority)
        {
            float value = originalValue;
            foreach (var effect in Data.EquipmentEffects)
            {
                value = effect.ModifyValue(cond, value, priority);
            }
            return value;
        }

        public ScriptableIntReference ModifyValue(EquipmentSituation cond, ScriptableIntReference reference, int priority)
        {
            foreach (var effect in Data.EquipmentEffects)
            {
                effect.ModifyValue(cond, reference, priority);
            }
            return reference;
        }
    } 
}