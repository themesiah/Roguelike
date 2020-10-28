using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Data
{
    [System.Serializable]
    public class EquipmentEffectData
    {
        [SerializeField]
        private EquipmentEffectType effectType = default;
        public EquipmentEffectType EffectType { get { return effectType; } }

        [SerializeField]
        private float effectModifier1 = default;
        public float EffectModifier1 { get { return effectModifier1; } }

        [SerializeField]
        private float effectModifier2 = default;
        public float EffectModifier2 { get { return effectModifier2; } }

        [SerializeField]
        private float effectModifier3 = default;
        public float EffectModifier3 { get { return effectModifier3; } }


    }
}