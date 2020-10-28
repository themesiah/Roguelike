using UnityEngine.Events;
using UnityEngine;
using Laresistance.Data;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;

namespace Laresistance.Core
{
    public class Equipment : ISlot
    {
        #region Events
        private UnityAction onEquip;
        private UnityAction onUnequip;
        #endregion

        private int slot = -1;
        private bool equiped = false;
        private List<string> descriptionReferences;
        private List<string> descriptionVariables;
        public EquipmentData Data { get; private set; }
        public string Name => Texts.GetText(Data.EquipmentNameReference);
        public string SlotName => Texts.GetText("EQUIPMENT_SLOT_" + Slot.ToString());
        public int Slot
        {
            get
            {
                return slot;
            }
        }

        public Equipment(int slot, EquipmentData equipmentData)
        {
            this.slot = slot;
            this.Data = equipmentData;
            descriptionReferences = new List<string>();
            descriptionVariables = new List<string>();
        }

        public bool SetInSlot(Player player)
        {
            return player.EquipEquipment(this);
        }

        public void EquipEquipment()
        {
            UnityEngine.Assertions.Assert.IsFalse(equiped);
            equiped = true;
            onEquip?.Invoke();
        }

        public void UnequipEquipment()
        {
            UnityEngine.Assertions.Assert.IsTrue(equiped);
            equiped = false;
            onUnequip?.Invoke();
        }

        public string GetEquipmentEffectDescription()
        {
            StringBuilder builder = new StringBuilder();
            foreach(string effectDescriptionFormat in descriptionReferences)
            {
                builder.Append(Texts.GetText(effectDescriptionFormat));
                builder.Append("");
            }
            return string.Format(builder.ToString(), (string[])descriptionVariables.ToArray());
        }

        #region EventRegister
        public void SetPowerModifier(EquipmentEvents equipmentEvents, float modifier)
        {
            if (equipmentEvents == null)
                return;
            EquipmentEvents.OnGetPowerHandler handler = ((ref int currentPower) => { currentPower = Mathf.CeilToInt(currentPower * modifier); });
            onEquip += () =>
            {
                equipmentEvents.OnGetPower += handler;
            };

            onUnequip += () =>
            {
                equipmentEvents.OnGetPower -= handler;
            };

            if (modifier > 0f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_001-A");
                descriptionVariables.Add(((modifier - 1f) * 100f).ToString());
            } else  if (modifier < 0f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_001-B");
                descriptionVariables.Add(((1f - modifier) * 100f).ToString());
            }
        }

        public void SetCooldownModifier(EquipmentEvents equipmentEvents, float modifier)
        {
            if (equipmentEvents == null)
                return;
            EquipmentEvents.OnGetCooldownHandler handler = ((ref float currentCooldown) => { currentCooldown = currentCooldown * modifier; });
            onEquip += () =>
            {
                equipmentEvents.OnGetCooldown += handler;
            };

            onUnequip += () =>
            {
                equipmentEvents.OnGetCooldown -= handler;
            };

            if (modifier > 1f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_002-B");
                descriptionVariables.Add(((modifier - 1f) * 100f).ToString());
            }
            else if (modifier < 1f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_002-A");
                descriptionVariables.Add(((1f - modifier) * 100f).ToString());
            }
        }
        #endregion
    } 
}