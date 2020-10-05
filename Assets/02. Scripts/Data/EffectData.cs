using UnityEngine;

namespace Laresistance.Data
{
    //[CreateAssetMenu(menuName = "Laresistance/Data/Effect Data")]
    [System.Serializable]
    public class EffectData
    {
        [SerializeField]
        private EffectType effectType = default;
        public EffectType EffectType { get { return effectType; } }

        [SerializeField]
        private int power = default;
        public int Power { get { return power; } }

        [SerializeField]
        private EffectTargetType targetType = default;
        public EffectTargetType TargetType { get { return targetType; } }
    }
}