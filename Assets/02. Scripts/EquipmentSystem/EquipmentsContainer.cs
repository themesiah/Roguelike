using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Equipments;
using UnityEngine;

namespace Laresistance.Core
{
    public class EquipmentsContainer
    {
        private static int MAX_EQUIPS = 4;
        public Equipment[] Equipments { get; private set; }

        public delegate void OnEquipmentEquipedHandler(EquipmentsContainer sender);
        public event OnEquipmentEquipedHandler OnEquip;
        public event OnEquipmentEquipedHandler OnUnequip;

        public EquipmentsContainer()
        {
            Equipments = new Equipment[MAX_EQUIPS];
        }

        public int ModifyValue(EquipmentSituation situation, int originalValue)
        {
            int value = originalValue;
            for (int i = 0; i <= 3; ++i)
            {
                foreach (var equip in Equipments)
                {
                    if (equip != null)
                        value = equip.ModifyValue(situation, value, i);
                }
            }
            return value;
        }

        public float ModifyValue(EquipmentSituation situation, float originalValue)
        {
            float value = originalValue;
            for (int i = 0; i <= 3; ++i)
            {
                foreach (var equip in Equipments)
                {
                    if (equip != null)
                        value = equip.ModifyValue(situation, value, i);
                }
            }
            return value;
        }

        public ScriptableIntReference ModifyValue(EquipmentSituation situation, ScriptableIntReference reference)
        {
            for (int i = 0; i <= 3; ++i)
            {
                foreach (var equip in Equipments)
                {
                    if (equip != null)
                        equip.ModifyValue(situation, reference, i);
                }
            }
            return reference;
        }

        public bool EquipEquipment(Equipment equipment, int slot)
        {
            if (equipment == null || slot == -1)
                throw new System.Exception("Can't equip. Equipment does not exist.");
            if (Equipments[slot] != null)
                throw new System.Exception("Can't equip. Slot already in use.");
            Equipments[slot] = equipment;
            equipment.EquipEquipment();
            OnEquip?.Invoke(this);
            return true;
        }

        public bool UnequipEquipment(int slot)
        {
            if (slot == -1 || Equipments[slot] == null)
                throw new System.Exception("Can't unequip. Equipment does not exist, or invalid slot");
            Equipments[slot].UnequipEquipment();
            Equipments[slot] = null;
            OnUnequip?.Invoke(this);
            return true;
        }
    }
}