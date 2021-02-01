using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Equipments;
using UnityEngine;

namespace Laresistance.Core
{
    public class EquipmentsContainer
    {
        private static int MAX_EQUIPS = 4;
        public Equipment[] Equipments { get; private set; }

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

        public bool EquipEquipment(Equipment equipment)
        {
            if (equipment == null || equipment.Slot == -1 || Equipments[equipment.Slot] != null)
                throw new System.Exception("Can't equip. Equipment does not exist, or invalid slot");
            Equipments[equipment.Slot] = equipment;
            equipment.EquipEquipment();
            return true;
        }

        public bool UnequipEquipment(Equipment equipment)
        {
            if (equipment == null || equipment.Slot == -1 || Equipments[equipment.Slot] != equipment)
                throw new System.Exception("Can't unequip. Equipment does not exist, or invalid slot");
            Equipments[equipment.Slot] = null;
            equipment.UnequipEquipment();
            return true;
        }
    }
}