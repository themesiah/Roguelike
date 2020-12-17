using Laresistance.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class StaticKeySpriteChangerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Image spriteReference = default;
        [SerializeField]
        private SpriteRenderer spriteRenderer = default;
        [SerializeField]
        private KeySetSelector keySetSelector = default;
        [SerializeField]
        private bool activateOnChange = true;
        [SerializeField]
        private Vector3[] scalePerScheme = default;

        private Vector3 originalScale;
        private Transform transformToUse;

        private void Awake()
        {
            if (spriteRenderer != null)
            {
                transformToUse = spriteRenderer.transform;
            } else if (spriteReference != null)
            {
                transformToUse = spriteReference.transform;
            }
            originalScale = transformToUse.localScale;
        }

        private void OnEnable()
        {
            keySetSelector.IndexForSelection.RegisterOnChangeAction(OnControlSchemeChanged);
            OnControlSchemeChanged(keySetSelector.IndexForSelection.GetValue());
        }

        private void OnDisable()
        {
            keySetSelector.IndexForSelection.UnregisterOnChangeAction(OnControlSchemeChanged);
        }

        public void OnControlSchemeChanged(int newControlScheme)
        {
            if (spriteReference != null)
            {
                spriteReference.sprite = keySetSelector.Get();
                if (spriteReference.sprite != null)
                {
                    if (activateOnChange)
                        spriteReference.enabled = true;
                    ((RectTransform)spriteReference.transform).sizeDelta = keySetSelector.Get().rect.size;
                } else
                {
                    if (activateOnChange)
                        spriteReference.enabled = false;
                }
            }
            if (spriteRenderer != null)
                spriteRenderer.sprite = keySetSelector.Get();

            if (scalePerScheme.Length > newControlScheme)
            {
                transformToUse.localScale = scalePerScheme[newControlScheme];
            } else
            {
                transformToUse.localScale = originalScale;
            }
        }
    }
}