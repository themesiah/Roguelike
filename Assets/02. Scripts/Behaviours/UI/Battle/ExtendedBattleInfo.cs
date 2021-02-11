using UnityEngine;
using UnityEngine.UI;
using GamedevsToolbox.ScriptableArchitecture.Sets;
using GamedevsToolbox.ScriptableArchitecture.Values;
using GamedevsToolbox.Utils;

namespace Laresistance.Behaviours
{
    public class ExtendedBattleInfo : MonoBehaviour
    {
        private static Vector3 ARROW_EULER_LEFT = Vector3.forward * 90f;
        private static Vector3 ARROW_EULER_RIGHT = Vector3.forward * -90f;

        [SerializeField]
        private HorizontalOrVerticalLayoutGroup[] layouts = default;
        [SerializeField]
        private HorizontalOrVerticalLayoutGroup healthStatusLayout = default;
        [SerializeField]
        private RuntimeSingleGameObject leftSideRef = default;
        [SerializeField]
        private RuntimeSingleGameObject rightSideRef = default;
        [SerializeField]
        private ScriptableIntReference battleSideRef = default;
        [SerializeField]
        private Transform arrowHolder = default;
        [SerializeField]
        private Text[] infoTextsForSizeChange = default;
        [SerializeField]
        private int textSelectedSize = default;
        [SerializeField]
        private Vector2 sliderSelectedSize = default;
        [SerializeField]
        private RectTransform healthSliderTransform = default;
        [SerializeField]
        private MaskSlider healthSlider = default;
        [SerializeField]
        private bool isPlayer = default;
        [SerializeField]
        private Transform[] extraObjectsToReparent = default;

        private Transform originalParent;
        private int originalTextSize;
        private Vector2 originalSliderSize;

        private void Awake()
        {
            originalParent = transform.parent;
            originalTextSize = infoTextsForSizeChange[0].fontSize;
            originalSliderSize = healthSliderTransform.sizeDelta;
        }

        public void SetupForBattle()
        {
            gameObject.SetActive(true);
            GameObject sideObject;
            if ((battleSideRef.GetValue() == 0 && isPlayer) || (battleSideRef.GetValue() == 1 && !isPlayer))
            {
                sideObject = leftSideRef.Get();
                foreach(HorizontalOrVerticalLayoutGroup layout in layouts)
                {
                    layout.childAlignment = TextAnchor.MiddleLeft;
                    healthStatusLayout.reverseArrangement = false;
                }
                arrowHolder.localEulerAngles = ARROW_EULER_LEFT;
            } else
            {
                sideObject = rightSideRef.Get();
                foreach (HorizontalOrVerticalLayoutGroup layout in layouts)
                {
                    layout.childAlignment = TextAnchor.MiddleRight;
                    healthStatusLayout.reverseArrangement = true;
                }
                arrowHolder.localEulerAngles = ARROW_EULER_RIGHT;
            }

            if (isPlayer)
            {
                Selected();
            }
            else
            {
                Unselected();
            }

            transform.SetParent(sideObject.transform, false);

            foreach(Transform extra in extraObjectsToReparent)
            {
                extra.SetParent(sideObject.transform, false);
                extra.gameObject.SetActive(true);
            }
        }

        public void Unsetup()
        {
            transform.SetParent(originalParent, false);
            gameObject.SetActive(false);

            foreach (Transform extra in extraObjectsToReparent)
            {
                extra.SetParent(originalParent, false);
                extra.gameObject.SetActive(false);
            }
        }

        public void SetSelectionArrow(GameObject arrow)
        {
            arrow.transform.SetParent(arrowHolder, false);
            arrow.transform.localScale = Vector3.one;
            arrow.transform.localEulerAngles = Vector3.zero;
        }

        public void Selected()
        {
            foreach (Text t in infoTextsForSizeChange)
            {
                t.fontSize = textSelectedSize;
            }
            healthSliderTransform.sizeDelta = sliderSelectedSize;
            healthSlider.UpdateSlider();
            RebuildLayouts();
        }

        public void Unselected()
        {
            foreach (Text t in infoTextsForSizeChange)
            {
                t.fontSize = originalTextSize;
            }
            healthSliderTransform.sizeDelta = originalSliderSize;
            healthSlider.UpdateSlider();
            RebuildLayouts();
        }

        private void RebuildLayouts()
        {
            Canvas.ForceUpdateCanvases();
            foreach (var layout in layouts)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)layout.transform);
                layout.CalculateLayoutInputHorizontal();
                layout.CalculateLayoutInputVertical();
            }
        }
    }
}