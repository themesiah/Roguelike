using UnityEngine;

namespace Laresistance.Data
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Combo/Combo Data")]
    public class ComboData : ScriptableObject
    {
        [SerializeField]
        private ComboConditionData comboCondition = default;
        public ComboConditionData ComboCondition { get { return comboCondition; } }

        [SerializeField]
        private AbilityData comboAbility = default;
        public AbilityData ComboAbility { get { return comboAbility; } }
    }
}