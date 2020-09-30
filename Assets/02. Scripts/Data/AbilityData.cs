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
        private float cooldown = default;
        public float Cooldown { get { return cooldown; } }
    }
}