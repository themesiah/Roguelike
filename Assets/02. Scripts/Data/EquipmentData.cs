using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Data
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Equipment")]
    public class EquipmentData : ScriptableObject
    {
        [SerializeField]
        private string equipNameReference = default;
        public string EquipmentNameReference { get { return equipNameReference; } }

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
        private List<EquipmentEffectData> equipmentEffects = default;
        public List<EquipmentEffectData> EquipmentEffects { get { return equipmentEffects; } }
    }
}