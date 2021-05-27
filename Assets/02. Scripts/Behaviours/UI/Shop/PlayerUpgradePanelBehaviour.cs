using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using Laresistance.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class PlayerUpgradePanelBehaviour : ShopOfferUIBehaviour
    {
        [SerializeField]
        private Text bloodTextReference = default;
        [SerializeField]
        private Text playerNameReference = default;
        [SerializeField]
        private Text abilityTextReference = default;
        [SerializeField]
        private Text abilityAfterTextReference = default;
        [SerializeField]
        private Text levelTextReference = default;

        //[SerializeField]
        //private AssetReference playerCharacterReference = default;
        //[SerializeField]
        //private int sortingOrder = 101;
        //[SerializeField]
        //private float scaleMultiplier = 40f;
        //[SerializeField]
        //private Material unlitMaterial = default;

        public override void SetupOffer(ShopOffer offer)
        {
            SetCost(offer.Cost);
            playerNameReference.text = Texts.GetText("PLAYER");
            abilityTextReference.text = offer.Reward.player.GetAbilityText(offer.Reward.player.Level);
            abilityAfterTextReference.text = offer.Reward.player.GetAbilityText(offer.Reward.player.Level+1);
            levelTextReference.text = Texts.GetText("PLAYER_UPGRADE_PANEL_002", new object[] { offer.Reward.player.Level, offer.Reward.player.Level + 1 });
            //playerCharacterReference.InstantiateAsync(playerPrefabHolder).Completed += (handler) => {
            //    GameObject go = handler.Result;
            //    go.transform.localPosition = Vector3.zero;
            //    go.transform.localScale = go.transform.localScale * scaleMultiplier;
            //    SpriteRenderer renderer = go.GetComponent<SpriteRenderer>();
            //    renderer.sortingOrder = sortingOrder;
            //    renderer.sortingLayerName = "UI";
            //    renderer.material = unlitMaterial;
            //};            
        }

        public override void SetCost(int cost)
        {
            bloodTextReference.text = Texts.GetText("PLAYER_UPGRADE_PANEL_001", cost);
        }
    }
}