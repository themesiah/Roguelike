using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Laresistance.Battle;
using Laresistance.Core;

namespace Laresistance.Behaviours
{
    public class ShowableAbility : MonoBehaviour, IShowableGameElement
    {
        [SerializeField]
        private Image frame = default;
        [SerializeField]
        private Image icon = default;
        [SerializeField]
        private Text effectPowerText = default;
        [SerializeField]
        private Text shortText = default;
        [SerializeField]
        private Image[] interactableImages = default;
        [SerializeField]
        private Color nonInteractableColor = default;

        public void SetupShowableElement(ShowableElement showableElement)
        {
            if (showableElement == null)
            {
                if (icon != null)
                {
                    icon.gameObject.SetActive(false);
                }
                if (effectPowerText != null)
                {
                    effectPowerText.text = "";
                }
                if (shortText != null)
                {
                    shortText.text = "";
                }
            } else {
                BattleAbility ability = (BattleAbility)showableElement;

                if (frame != null)
                {
                    frame.sprite = ability.AbilityFrame;
                }

                if (icon != null)
                {
                    icon.gameObject.SetActive(true);
                    icon.sprite = ability.AbilityIcon;
                }

                if (effectPowerText != null)
                {
                    effectPowerText.text = ability.GetAbilityPowerText(ability.AbilityLevel);
                }

                if (shortText != null)
                {
                    shortText.text = ability.GetShortAbilityText(ability.AbilityLevel);
                }
            }
        }

        public void SetInteractable(bool interactable)
        {
            if (interactable)
            {
                foreach(Image im in interactableImages)
                {
                    im.color = Color.white;
                }
            } else
            {
                foreach(Image im in interactableImages)
                {
                    im.color = nonInteractableColor;
                }
            }
        }

        public void OverrideFrame(Sprite newFrame)
        {
            if (frame != null)
            {
                frame.sprite = newFrame;
            }
        }
    }
}