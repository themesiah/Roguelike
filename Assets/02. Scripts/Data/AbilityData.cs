using UnityEngine;

namespace Laresistance.Data
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Ability Data")]
    public class AbilityData : ScriptableObject
    {
        [SerializeField]
        private EffectData[] effectsData = default;
        public EffectData[] EffectsData { get { return effectsData; } }

        [SerializeField]
        private int cost = default;
        public int Cost { get { return cost; } }

        [SerializeField]
        private int weight = default;
        public int Weight { get { return weight; } }
    }
}