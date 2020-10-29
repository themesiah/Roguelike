using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class MinionOfferPanelBehaviour : ShopOfferUIBehaviour
    {
        [SerializeField]
        private Text bloodTextReference = default;
        [SerializeField]
        private Text minionNameReference = default;
        [SerializeField]
        private Text abilityTextReference = default;
        [SerializeField]
        private Transform minionPrefabHolder = default;

        [SerializeField]
        private int sortingOrder = 101;
        [SerializeField]
        private float scaleMultiplier = 40f;
        [SerializeField]
        private Material unlitMaterial = default;

        public override void SetupOffer(ShopOffer offer)
        {
            if (offer.Cost > 0)
            {
                bloodTextReference.enabled = true;
                bloodTextReference.text = Texts.GetText("MINION_PANEL_001", offer.Cost);
            } else
            {
                bloodTextReference.enabled = false;
            }
            minionNameReference.text = Texts.GetText(offer.Reward.minion.Name);
            abilityTextReference.text = offer.Reward.minion.GetAbilityText();
            GameObject go = Instantiate(offer.Reward.minion.Data.Prefab, minionPrefabHolder);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = go.transform.localScale * scaleMultiplier;
            SpriteRenderer renderer = go.GetComponent<SpriteRenderer>();
            renderer.sortingOrder = sortingOrder;
            renderer.material = unlitMaterial;
        }
    }
}