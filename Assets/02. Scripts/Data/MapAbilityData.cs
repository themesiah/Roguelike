using GamedevsToolbox.ScriptableArchitecture.Values;
using UnityEngine;

namespace Laresistance.Data
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Map Ability Data")]
    public class MapAbilityData : ScriptableObject
    {
        [SerializeField]
        private string abilityName = default;
        public string AbilityName { get { return abilityName; } }

        [SerializeField]
        private string abilityDescriptionId = default;
        public string AbilityDescriptionId { get { return abilityDescriptionId; } }

        [SerializeField]
        private MapAbilityType abilityType = default;
        public MapAbilityType AbilityType { get { return abilityType; } }

        [SerializeField]
        private ScriptableBoolReference abilityObtainedReference = default;
        public ScriptableBoolReference AbilityObtainedReference { get { return abilityObtainedReference; } }

        [SerializeField]
        private Sprite abilitySpriteRef = default;
        public Sprite AbilitySpriteRef { get { return abilitySpriteRef; } }

        [SerializeField]
        private int currencyCost = default;
        public int CurrencyCost { get { return currencyCost; } }
    }
}
