using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Data
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Consumable")]
    public class ConsumableData : ScriptableObject
    {
        [SerializeField]
        private string nameRef = default;
        public string NameRef { get { return nameRef; } }

        [SerializeField]
        private int baseBloodPrice = default;
        public int BaseBloodPrice { get { return baseBloodPrice; } }

        [SerializeField]
        private AbilityData abilityData = default;
        public AbilityData AbilityData { get { return abilityData; } }

        [SerializeField]
        private Sprite spriteReference = default;
        public Sprite SpriteReference { get { return spriteReference; } }
    }
}