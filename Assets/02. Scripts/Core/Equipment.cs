using UnityEngine.Events;
using UnityEngine;

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
        public int Slot
        {
            get
            {
                return slot;
            }
        }

        public Equipment(int slot)
        {
            this.slot = slot;
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
        }
        #endregion
    } 
}