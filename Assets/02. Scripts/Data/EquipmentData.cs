using System.Collections.Generic;
using UnityEngine;
using Laresistance.Equipments;

namespace Laresistance.Data
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Equipment")]
    public class EquipmentData : ScriptableObject
    {
        [SerializeField]
        private string equipNameReference = default;
        public string EquipmentNameReference { get { return equipNameReference; } }

        [SerializeField]
        private string equipDescriptionReference = default;
        public string EquipmentDescriptionReference { get { return equipDescriptionReference; } }

        [SerializeField]
        private EquipmentSlotType slot = default;
        public EquipmentSlotType Slot { get { return slot; } }

        [SerializeField]
        private int hardCurrencyCost = default;
        public int HardCurrencyCost { get { return hardCurrencyCost; } }

        [SerializeField]
        private Sprite spriteReference = default;
        public Sprite SpriteReference { get { return spriteReference; } }

        [SerializeField]
        private List<EquipmentEffect> equipmentEffects = default;
        public List<EquipmentEffect> EquipmentEffects { get { return equipmentEffects; } }

        [SerializeField]
        private bool corrupted = default;
        public bool Corrupted { get { return corrupted; } }
    }
}