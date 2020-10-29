using DigitalRuby.Tween;
using GamedevsToolbox.ScriptableArchitecture.Events;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Battle;
using Laresistance.Core;
using Laresistance.Data;
using Laresistance.Systems;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class PilgrimBehaviour : MonoBehaviour
    {
        [Header("Buy lists")]
        [SerializeField]
        private MinionList buyableMinionList = default;
        [SerializeField]
        private List<ConsumableData> consumableDatas = default;
        [SerializeField]
        private List<EquipmentData> equipmentDatas = default;
        [SerializeField]
        private List<MapAbilityData> mapAbilityDatas = default;
        [Header("References")]
        [SerializeField]
        private ScriptableIntReference bloodReference = default;
        [SerializeField]
        private ScriptableIntReference hardCurrencyReference = default;
        [SerializeField]
        private RuntimePlayerDataBehaviourSingle playerDataBehaviourReference = default;
        [SerializeField]
        private StringGameEvent gameContextSignal = default;
        [SerializeField]
        private RewardUILibrary rewardUILibrary = default;
        [Header("Prefabs")]
        [SerializeField]
        private GameObject[] offerPanelPrefabs = default;
        [Header("General")]
        [SerializeField]
        private Transform uiCanvas = default;
        [SerializeField]
        private Transform gridCanvas = default;
        [SerializeField]
        private Transform upgradeCanvas = default;
        [SerializeField]
        private Sprite[] offerKeys = default;
        [SerializeField]
        private Image exitShopPanel = default;
        [Header("Selection")]
        [SerializeField]
        private Color unselectedColor = default;
        [SerializeField]
        private Color selectedColor = default;
        [SerializeField]
        private Color cantBuyColor = default;
        [Header("Upgrade and shop")]
        [SerializeField]
        private GameObject[] shopBuyObjects = default;
        [SerializeField]
        private GameObject[] shopUpgradeObjects = default;

        private ShopSystem shopSystem;
        private RewardSystem rewardSystem;
        private Player player;
        private bool initialized = false;
        private bool panelTweenFinished = false;
        private int offerSelected = -1;
        private List<IShopOfferUI> shopOfferUIList;
        private List<IShopOfferUI> shopUpgradeUIList;

        #region Initialization
        private void Start()
        {
            shopOfferUIList = new List<IShopOfferUI>();
            shopUpgradeUIList = new List<IShopOfferUI>();
            Init();
        }

        private void Init()
        {
            if (initialized)
                return;
            player = playerDataBehaviourReference.Get().player;
            shopSystem = new ShopSystem(bloodReference, hardCurrencyReference);
            rewardSystem = new RewardSystem(player, bloodReference, hardCurrencyReference, rewardUILibrary);
            BattleStatusManager statusManager = playerDataBehaviourReference.Get().StatusManager;
            // TEST
            Minion m1 = MinionFactory.GetMinion(buyableMinionList.minionList[0], 1, player.GetEquipmentEvents(), statusManager);
            shopSystem.AddOffer(new ShopOffer(m1.Data.BaseBloodPrice, false, new RewardData(0, 0, m1, null, null, null)));
            Minion m2 = MinionFactory.GetMinion(buyableMinionList.minionList[1], 5, player.GetEquipmentEvents(), statusManager);
            shopSystem.AddOffer(new ShopOffer(m1.Data.BaseBloodPrice, false, new RewardData(0, 0, m2, null, null, null)));
            Consumable c = ConsumableFactory.GetConsumable(consumableDatas[0], player.GetEquipmentEvents(), statusManager);
            shopSystem.AddOffer(new ShopOffer(c.Data.BaseBloodPrice, false, new RewardData(0, 0, null, c, null, null)));
            Equipment e = EquipmentFactory.GetEquipment(equipmentDatas[0], player.GetEquipmentEvents());
            shopSystem.AddOffer(new ShopOffer(e.Data.HardCurrencyCost, true, new RewardData(0, 0, null, null, e, null)));
            shopSystem.AddOffer(new ShopOffer(2, true, new RewardData(0, 0, null, null, null, mapAbilityDatas[0])));

            // Initialize UI depending on offers, one panel per offer
            for (int i = 0; i < shopSystem.GetOffers().Count; ++i)
            {
                shopOfferUIList.Add(CreateOfferPanel(shopSystem.GetOffers()[i], offerKeys[i]));
            }
            initialized = true;
        }

        public void OpenShop()
        {
            Init();
            UpdateMinionUpgradePanel();
            StartCoroutine(OpenShopCoroutine());
        }

        private void UpdateMinionUpgradePanel()
        {
            foreach(Transform t in upgradeCanvas)
            {
                Destroy(t.gameObject);
            }
            shopUpgradeUIList.Clear();
            int index = 0;
            foreach (Minion m in player.GetMinions())
            {
                GameObject go = Instantiate(offerPanelPrefabs[4], upgradeCanvas);
                IShopOfferUI shopOfferUI = go.GetComponent<IShopOfferUI>();
                shopUpgradeUIList.Add(shopOfferUI);
                shopOfferUI.SetOfferKey(offerKeys[index]);
                shopOfferUI.SetupOffer(new ShopOffer(0, false, new RewardData(0, 0, m, null, null, null)));
                index++;
            }
        }

        private void UpdateSingleMinionUpgradePanel(int index, Minion m)
        {
            IShopOfferUI shopOfferUI = shopUpgradeUIList[index];
            shopOfferUI.SetupOffer(new ShopOffer(0, false, new RewardData(0, 0, m, null, null, null)));
        }
        

        private GameObject GetPrefabFromOffer(ShopOffer offer)
        {
            if (offer.Reward.minion != null)
            {
                return offerPanelPrefabs[0];
            } else if (offer.Reward.equip != null)
            {
                return offerPanelPrefabs[1];
            } else if (offer.Reward.consumable != null)
            {
                return offerPanelPrefabs[2];
            } else if (offer.Reward.mapAbilityData != null)
            {
                return offerPanelPrefabs[3];
            } else
            {
                return null;
            }
        }

        private IShopOfferUI CreateOfferPanel(ShopOffer offer, Sprite offerKey)
        {
            GameObject prefab = GetPrefabFromOffer(offer);
            GameObject panel = Instantiate(prefab, gridCanvas);
            IShopOfferUI offerUI = panel.GetComponent<IShopOfferUI>();
            offerUI.SetupOffer(offer);
            offerUI.SetOfferKey(offerKey);
            return offerUI;
        }
        #endregion

        #region Input
        public void Option1Selected(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                offerSelected = 0;
            }
        }

        public void Option2Selected(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                offerSelected = 1;
            }
        }

        public void Option3Selected(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                offerSelected = 2;
            }
        }

        public void Option4Selected(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                offerSelected = 3;
            }
        }

        public void Option5Selected(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                offerSelected = 4;
            }
        }

        public void Option6Selected(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                offerSelected = 5;
            }
        }

        public void OptionCancelSelected(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                offerSelected = -2;
            }
        }

        public void OptionChangeContextSelected(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                offerSelected = -3;
            }
        }
        #endregion

        #region ShopContextChange
        private void ActivateDeactivateContext(bool isShop)
        {
            foreach(var obj in shopBuyObjects)
            {
                obj.SetActive(isShop);
            }

            foreach(var obj in shopUpgradeObjects)
            {
                obj.SetActive(!isShop);
            }
        }
        #endregion

        #region Coroutines
        private IEnumerator OpenShopCoroutine()
        {
            gameContextSignal.Raise("UI");
            // Show all shop panels. Wait for input.
            yield return OpenShopUI();
            // If there are reserved minions, convert them into hard currency and do reward manager thing.
            int reserveSize = player.ClearMinionReserve();
            yield return rewardSystem.GetReward(new RewardData(0, reserveSize, null, null, null, null));
            gameContextSignal.Raise("Map");
        }

        private IEnumerator OpenShopUI()
        {
            ActivateDeactivateContext(true);
            exitShopPanel.color = unselectedColor;
            yield return StartingTween();
            offerSelected = -1;
            while (offerSelected > -2)
            {
                // If inputted an offer, try to buy.
                if (offerSelected >= 0 && offerSelected < shopSystem.GetOffers().Count)
                {
                    ShopOffer offer = shopSystem.GetOffers()[offerSelected];
                    if ((!offer.UseHardCurrency && bloodReference.GetValue() < offer.Cost) || (offer.UseHardCurrency && hardCurrencyReference.GetValue() < offer.Cost))
                    {
                        StartCoroutine(ShopCantBuy(shopOfferUIList, offerSelected));
                    }
                    else
                    {
                        RewardData rd = shopSystem.BuyOffer(offerSelected);
                        if (rd != null)
                        {
                            shopOfferUIList[offerSelected].SetPanelColor(selectedColor);
                            yield return FinishingTween();
                            RemoveOffer(offerSelected);
                            yield return rewardSystem.GetReward(rd);
                            yield return StartingTween();
                        }
                    }
                    offerSelected = -1;
                }
                yield return null;
            }
            if (offerSelected == -3)
            {
                yield return FinishingTween();
                yield return OpenShopUpgradeUI();
            }
            exitShopPanel.color = selectedColor;
            // If bought something, do reward manager thing.
            // Wait for exit or more buy.
            yield return FinishingTween();
        }

        private IEnumerator OpenShopUpgradeUI()
        {
            ActivateDeactivateContext(false);
            yield return StartingTween();
            offerSelected = -1;
            while (offerSelected > -2)
            {
                if (offerSelected >= 0 && offerSelected < player.EquippedMinionsQuantity)
                {
                    Minion minionToUpgrade = player.GetMinions()[offerSelected];
                    if (minionToUpgrade != null)
                    {
                        int cost = player.GetMinions()[offerSelected].GetUpgradeCost();
                        if (bloodReference.GetValue() < cost)
                        {
                            StartCoroutine(ShopCantBuy(shopUpgradeUIList, offerSelected));
                        }
                        else
                        {
                            bloodReference.SetValue(bloodReference.GetValue() - cost);
                            minionToUpgrade = player.GetMinions()[offerSelected];
                            minionToUpgrade.Upgrade();
                            UpdateSingleMinionUpgradePanel(offerSelected, minionToUpgrade);
                            StartCoroutine(ShopCanBuy(shopUpgradeUIList, offerSelected));
                        }
                    }
                    offerSelected = -1;
                }
                yield return null;
            }

            if (offerSelected == -3)
            {
                yield return FinishingTween();
                yield return OpenShopUI();
            }
        }

        private IEnumerator ShopCantBuy(List<IShopOfferUI> shopOfferList, int index)
        {
            shopOfferList[index].SetPanelColor(cantBuyColor);
            yield return new WaitForSeconds(1f);
            shopOfferList[index].SetPanelColor(unselectedColor);
        }

        private IEnumerator ShopCanBuy(List<IShopOfferUI> shopOfferList, int index)
        {
            shopOfferList[index].SetPanelColor(selectedColor);
            yield return new WaitForSeconds(1f);
            shopOfferList[index].SetPanelColor(unselectedColor);
        }

        protected virtual IEnumerator StartingTween()
        {
            uiCanvas.gameObject.SetActive(true);
            panelTweenFinished = false;
            TweenFactory.Tween("PanelSize", Vector3.zero, Vector3.one, 0.5f, TweenScaleFunctions.CubicEaseIn, UpdatePanelSize, ResizeCompleted);
            while (!panelTweenFinished)
            {
                yield return null;
            }
        }

        protected virtual IEnumerator FinishingTween()
        {
            uiCanvas.gameObject.Tween("PanelSize", Vector3.one, Vector2.zero, 0.5f, TweenScaleFunctions.CubicEaseIn, UpdatePanelSize, ResizeCompleted);
            panelTweenFinished = false;
            while (!panelTweenFinished)
            {
                yield return null;
            }
            uiCanvas.gameObject.SetActive(false);
        }

        private void UpdatePanelSize(ITween<Vector2> t)
        {
            uiCanvas.localScale = t.CurrentValue;
        }

        private void ResizeCompleted(ITween<Vector2> t)
        {
            panelTweenFinished = true;
        }
        #endregion

        private void RemoveOffer(int index)
        {
            Destroy(((Component)shopOfferUIList[index]).gameObject);
            shopOfferUIList.RemoveAt(index);
            for(int i = 0; i < shopOfferUIList.Count; ++i)
            {
                shopOfferUIList[i].SetOfferKey(offerKeys[i]);
            }
        }
    }
}