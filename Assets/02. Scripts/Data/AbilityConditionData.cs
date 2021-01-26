using UnityEngine;

namespace Laresistance.Data
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Combo/Ability Condition")]
    public class AbilityConditionData : ScriptableObject
    {
        [SerializeField]
        private AbilityData[] abilityDatas = default;
        public AbilityData[] AbilityDatas { get { return abilityDatas; } }
    }
}