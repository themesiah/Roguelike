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

        [SerializeField]
        private int weight = default;
        public int Weight { get { return weight; } }

        [SerializeField]
        private float cooldown = default;
        public float Cooldown { get { return cooldown; } }

        [SerializeField]
        private float castTime = default;
        public float CastTime { get { return castTime; } }

        [SerializeField]
        private Sprite icon = default;
        public Sprite Icon { get { return icon; } }

        [SerializeField]
        private Sprite frameGraphic = default;
        public Sprite FrameGraphic { get { return frameGraphic; } }

        [SerializeField]
        private bool isBasicSkill = false;
        public bool IsBasicSkill { get { return isBasicSkill; } }

        [SerializeField]
        private bool isComboSkill = false;
        public bool IsComboSkill { get { return isComboSkill; } }
    }
}