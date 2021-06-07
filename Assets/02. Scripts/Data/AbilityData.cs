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
        private string shortDesc = default;
        public string ShortDesc { get { return shortDesc; } }

        [SerializeField]
        private int cost = default;
        public int Cost { get { return cost; } }

        [SerializeField] [Tooltip("Unused right now")]
        private float cooldown = default;
        public float Cooldown { get { return cooldown; } }        

        [SerializeField]
        private Sprite icon = default;
        public Sprite Icon { get { return icon; } }

        [SerializeField]
        private Sprite frameGraphic = default;
        public Sprite FrameGraphic { get { return frameGraphic; } }

        [SerializeField]
        private string animationTriggerOverride = default;
        public string AnimationTriggerOverride { get { return animationTriggerOverride; } }

        [SerializeField]
        private bool isComboSkill = false;
        public bool IsComboSkill { get { return isComboSkill; } }

        [Header("Enemy AI")]
        [SerializeField]
        private int weight = default;
        public int Weight { get { return weight; } }

        [SerializeField]
        [Tooltip("Only for enemies. With a cast time, some powerful abilities can be slower")]
        private float castTime = default;
        public float CastTime { get { return castTime; } }

        [SerializeField] [Tooltip("Only for enemies. This specifies the time between uses for skills not normally executed (shields and so on)")]
        private float internalCooldown = 0f;
        public float InternalCooldown { get { return internalCooldown; } }

        [SerializeField] [Tooltip("Situation in which the ability will be executed by the enemy. Always is the normal case where the enemy charge the skill before using.")]
        private AbilityDataAISpecification aiSpecification = AbilityDataAISpecification.Always;
        public AbilityDataAISpecification AiSpecification { get { return aiSpecification; } }
    }
}