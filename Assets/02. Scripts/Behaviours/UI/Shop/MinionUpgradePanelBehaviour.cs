using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class MinionUpgradePanelBehaviour : ShopOfferUIBehaviour
    {
        [SerializeField]
        private Text bloodTextReference = default;
        [SerializeField]
        private Text minionNameReference = default;
        [SerializeField]
        private Text abilityTextReference = default;
        [SerializeField]
        private Text abilityAfterTextReference = default;
        [SerializeField]
        private Transform minionPrefabHolder = default;
        [SerializeField]
        private Text levelTextReference = default;

        [SerializeField]
        private int sortingOrder = 101;
        [SerializeField]
        private float scaleMultiplier = 40f;
        [SerializeField]
        private Material unlitMaterial = default;

        public override void SetupOffer(ShopOffer offer)
        {
            SetCost(offer.Cost);
            minionNameReference.text = Texts.GetText(offer.Reward.minion.Name);
            abilityTextReference.text = offer.Reward.minion.GetAbilityText();
            abilityAfterTextReference.text = offer.Reward.minion.GetNextLevelAbilityText();
            levelTextReference.text = Texts.GetText("MINION_PANEL_003", new object[] { offer.Reward.minion.Level, offer.Reward.minion.Level + 1 });
            GameObject go = Instantiate(offer.Reward.minion.Data.Prefab, minionPrefabHolder);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = go.transform.localScale * scaleMultiplier;
            SpriteRenderer renderer = go.GetComponent<SpriteRenderer>();
            renderer.sortingOrder = sortingOrder;
            renderer.material = unlitMaterial;
        }

        public override void SetCost(int cost)
        {
            bloodTextReference.text = Texts.GetText("MINION_PANEL_001", cost);
        }
    }
}