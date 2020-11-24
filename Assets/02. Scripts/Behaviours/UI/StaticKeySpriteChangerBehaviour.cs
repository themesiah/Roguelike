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
                    spriteReference.enabled = true;
                    ((RectTransform)spriteReference.transform).sizeDelta = keySetSelector.Get().rect.size;
                } else
                {
                    spriteReference.enabled = false;
                }
            }
            if (spriteRenderer != null)
                spriteRenderer.sprite = keySetSelector.Get();
        }
    }
}