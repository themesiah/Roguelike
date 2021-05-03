using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Laresistance.Data
{
    [System.Serializable]
    public class EffectData
    {
        [System.Serializable]
        public struct PrefabEffectData
        {
            public AssetReference prefabReference;
            public float delay;
            public Vector3 offset;
        }

        [SerializeField]
        private EffectType effectType = default;
        public EffectType EffectType { get { return effectType; } }

        [SerializeField]
        private int power = default;
        public int Power { get { return power; } }

        [SerializeField] [Tooltip("Delay for executing the next effect after this effect is executed")]
        private float delay = default;
        public float Delay { get { return delay; } }

        [SerializeField]
        private EffectTargetType targetType = default;
        public EffectTargetType TargetType { get { return targetType; } }

        [SerializeField]
        [Tooltip("Prefabs that will show on the caster")]
        private PrefabEffectData[] selfEffectPrefabs = default;
        public PrefabEffectData[] SelfEffectPrefabs { get { return selfEffectPrefabs; } }

        [SerializeField]
        [Tooltip("Prefabs that will show on every entity that is a target of the ability")]
        private PrefabEffectData[] targetEffectPrefabs = default;
        public PrefabEffectData[] TargetEffectPrefabs { get { return targetEffectPrefabs; } }
    }
}