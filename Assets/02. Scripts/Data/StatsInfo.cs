using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using UnityEngine;
using Laresistance.Behaviours;

namespace Laresistance.Data
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Stat Info")]
    public class StatsInfo : ScriptableObject
    {
        [SerializeField]
        private StatsType statType = default;
        public StatsType StatType { get { return statType; } }

        [SerializeField]
        private Sprite statSprite = default;
        public Sprite StatSprite { get { return statSprite; } }

        [SerializeField]
        private string statTitleRef = default;

        [SerializeField]
        private string statDescriptionRef = default;

        public string GetTitle() => Texts.GetText(statTitleRef);

        public string GetDescription()
        {
            string text = "";
            switch(statType)
            {
                case StatsType.Damage:
                    text = Texts.GetText(statDescriptionRef, GameConstantsBehaviour.Instance.damageStatCoeficient.GetValue() * 100f);
                    break;
                case StatsType.Shield:
                    text = Texts.GetText(statDescriptionRef, new object[] { GameConstantsBehaviour.Instance.shieldStatCoeficient.GetValue() * 100f, GameConstantsBehaviour.Instance.shieldTimeStatCoeficient.GetValue() * 100f });
                    break;
                case StatsType.Heal:
                    text = Texts.GetText(statDescriptionRef, GameConstantsBehaviour.Instance.healStatCoeficient.GetValue() * 100f);
                    break;
                case StatsType.StatusTime:
                    text = Texts.GetText(statDescriptionRef, GameConstantsBehaviour.Instance.statusTimeStatCoeficient.GetValue() * 100f);
                    break;
                case StatsType.MaxHealth:
                    text = Texts.GetText(statDescriptionRef, GameConstantsBehaviour.Instance.maxHealthStatAmount.GetValue());
                    break;
            }
            return text;
        }
    }
}