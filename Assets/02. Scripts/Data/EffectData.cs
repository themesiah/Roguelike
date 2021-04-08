using UnityEngine;

namespace Laresistance.Data
{
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

        [SerializeField]
        [Tooltip("Prefabs that will show on the caster")]
        private GameObject[] selfEffectPrefabs = default;
        public GameObject[] SelfEffectPrefabs { get { return selfEffectPrefabs; } }

        [SerializeField]
        [Tooltip("Prefabs that will show on every entity that is a target of the ability")]
        private GameObject[] targetEffectPrefabs = default;
        public GameObject[] TargetEffectPrefabs { get { return targetEffectPrefabs; } }
    }
}