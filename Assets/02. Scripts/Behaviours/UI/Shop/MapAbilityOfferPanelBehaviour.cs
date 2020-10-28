using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using Laresistance.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class MapAbilityOfferPanelBehaviour : MonoBehaviour, IShopOfferUI
    {
        [SerializeField]
        private Image panelImage = default;
        [SerializeField]
        private Text hardCurrencyTextReference = default;
        [SerializeField]
        private Text abilityNameReference = default;
        [SerializeField]
        private Text abilityDescriptionReference = default;
        [SerializeField]
        private Image keyImageReference = default;
        [SerializeField]
        private Image abilityIcon = default;

        public void SetOfferKey(Sprite offerKey)
        {
            keyImageReference.sprite = offerKey;
        }

        public void SetupOffer(ShopOffer offer)
        {
            hardCurrencyTextReference.text = Texts.GetText("MAPABILITY_PANEL_001", offer.Reward.mapAbilityData.CurrencyCost);
            abilityNameReference.text = Texts.GetText(offer.Reward.mapAbilityData.AbilityName);
            abilityDescriptionReference.text = Texts.GetText(offer.Reward.mapAbilityData.AbilityDescriptionId);
            abilityIcon.sprite = offer.Reward.mapAbilityData.AbilitySpriteRef;
        }

        public void SetPanelColor(Color color)
        {
            panelImage.color = color;
        }
    }
}