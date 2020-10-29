using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public abstract class ShopOfferUIBehaviour : MonoBehaviour, IShopOfferUI
    {
        [SerializeField]
        private Image panelImage = default;
        [SerializeField]
        private ScriptableIntReference currentControlScheme = default;
        [SerializeField]
        private Image keyImageReference = default;

        private KeySetSelector keySetSelector;

        public void SetOfferKey(KeySetSelector offerKey)
        {
            if (offerKey == null)
            {
                keyImageReference.enabled = false;
            }
            else
            {
                keySetSelector = offerKey;
                keyImageReference.enabled = true;
                keyImageReference.sprite = keySetSelector.Get();
            }
        }

        private void OnKeySetChanged(int current)
        {
            if (keySetSelector != null)
            {
                keyImageReference.enabled = true;
                keyImageReference.sprite = keySetSelector.Get();
            }
        }

        private void OnDisable()
        {
            currentControlScheme.UnregisterOnChangeAction(OnKeySetChanged);
        }

        private void OnEnable()
        {
            currentControlScheme.RegisterOnChangeAction(OnKeySetChanged);
            OnKeySetChanged(currentControlScheme.GetValue());
        }

        public void SetPanelColor(Color color)
        {
            panelImage.color = color;
        }

        public abstract void SetupOffer(ShopOffer offer);
    }
}