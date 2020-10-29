using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Core;
using Laresistance.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class PauseUIBehaviour : MonoBehaviour
    {
        [SerializeField]
        private RuntimePlayerDataBehaviourSingle playerDataBehaviourRef = default;
        [SerializeField]
        private ScriptableIntReference bloodReference = default;
        [SerializeField]
        private ScriptableIntReference hardCurrencyReference = default;

        [SerializeField]
        private GameObject pausePanel = default;

        [SerializeField]
        private List<GameObject> minionPanels = default;
        [SerializeField]
        private List<GameObject> equipPanels = default;
        [SerializeField]
        private List<GameObject> consumablePanels = default;
        [SerializeField]
        private Text bloodTextRef = default;
        [SerializeField]
        private Text hardCurrencyTextRef = default;

        [SerializeField]
        private List<KeySetSelector> keyList = default;

        public void OpenPauseUI()
        {
            pausePanel.SetActive(true);
            Player player = playerDataBehaviourRef.Get().player;

            foreach(GameObject panel in minionPanels)
            {
                panel.SetActive(false);
            }

            foreach(GameObject panel in equipPanels)
            {
                panel.SetActive(false);
            }

            foreach(GameObject panel in consumablePanels)
            {
                panel.SetActive(false);
            }

            for (int i = 0; i < player.GetMinions().Length; ++i)
            {
                if (player.GetMinions()[i] != null)
                {
                    minionPanels[i].SetActive(true);
                    IShopOfferUI offerUI = minionPanels[i].GetComponent<IShopOfferUI>();
                    offerUI.SetupOffer(new ShopOffer(0, false, new RewardData(0, 0, player.GetMinions()[i], null, null, null)));
                    offerUI.SetOfferKey(keyList[i]);
                }
            }

            for (int i = 0; i < player.GetEquipments().Length; ++i)
            {
                if (player.GetEquipments()[i] != null)
                {
                    equipPanels[i].SetActive(true);
                    IShopOfferUI offerUI = equipPanels[i].GetComponent<IShopOfferUI>();
                    offerUI.SetupOffer(new ShopOffer(0, false, new RewardData(0, 0, null, null, player.GetEquipments()[i], null)));
                    offerUI.SetOfferKey(null);
                }
            }

            for (int i = 0; i < player.GetConsumables().Length; ++i)
            {
                if (player.GetConsumables()[i] != null)
                {
                    consumablePanels[i].SetActive(true);
                    IShopOfferUI offerUI = consumablePanels[i].GetComponent<IShopOfferUI>();
                    offerUI.SetupOffer(new ShopOffer(0, false, new RewardData(0, 0, null, player.GetConsumables()[i], null, null)));
                    offerUI.SetOfferKey(keyList[i + 4]);
                }
            }

            bloodTextRef.text = bloodReference.GetValue().ToString();
            hardCurrencyTextRef.text = hardCurrencyReference.GetValue().ToString();
        }

        public void ClosePauseUI()
        {
            pausePanel.SetActive(false);
        }
    }
}