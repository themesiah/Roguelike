using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class MapAbilityOfferPanelBehaviour : ShopOfferUIBehaviour
    {
        [SerializeField]
        private Text hardCurrencyTextReference = default;
        [SerializeField]
        private Text abilityNameReference = default;
        [SerializeField]
        private Text abilityDescriptionReference = default;
        [SerializeField]
        private Image abilityIcon = default;

        public override void SetupOffer(ShopOffer offer)
        {
            if (offer.Cost > 0)
            {
                hardCurrencyTextReference.enabled = true;
                hardCurrencyTextReference.text = Texts.GetText("MAPABILITY_PANEL_001", offer.Cost);
            } else
            {
                hardCurrencyTextReference.enabled = false;
            }
            abilityNameReference.text = Texts.GetText(offer.Reward.mapAbilityData.AbilityName);
            abilityDescriptionReference.text = Texts.GetText(offer.Reward.mapAbilityData.AbilityDescriptionId);
            abilityIcon.sprite = offer.Reward.mapAbilityData.AbilitySpriteRef;
        }
    }
}