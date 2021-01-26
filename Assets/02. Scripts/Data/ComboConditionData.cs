using UnityEngine;

namespace Laresistance.Data
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Combo/Combo Condition")]
    public class ComboConditionData : ScriptableObject
    {
        [SerializeField]
        private AbilityConditionData[] abilityConditionDatas = default;
        public AbilityConditionData[] AbilityConditionDatas { get { return abilityConditionDatas; } }
    }
}