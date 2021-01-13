using UnityEngine;
using UnityEngine.UI;
using GamedevsToolbox.ScriptableArchitecture.Sets;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Behaviours
{
    public class ExtendedBattleInfo : MonoBehaviour
    {
        private static Vector3 ARROW_EULER_LEFT = Vector3.zero;
        private static Vector3 ARROW_EULER_RIGHT = Vector3.forward * 180f;

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
        private Text[] infoTextsForColorChange = default;
        [SerializeField]
        private Color defaultTextColor = default;
        [SerializeField]
        private Color textSelectedColor = default;
        [SerializeField]
        private bool isPlayer = default;

        private Transform originalParent;

        private void Awake()
        {
            originalParent = transform.parent;
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

            foreach(Text t in infoTextsForColorChange)
            {
                t.color = defaultTextColor;
            }

            transform.SetParent(sideObject.transform, false);
        }

        public void Unsetup()
        {
            transform.SetParent(originalParent, false);
            gameObject.SetActive(false);
        }

        public void SetSelectionArrow(GameObject arrow)
        {
            arrow.transform.SetParent(arrowHolder, false);
            arrow.transform.localScale = Vector3.one;
            arrow.transform.localEulerAngles = Vector3.zero;
        }

        public void Selected()
        {
            foreach (Text t in infoTextsForColorChange)
            {
                t.color = textSelectedColor;
            }
        }

        public void Unselected()
        {
            foreach (Text t in infoTextsForColorChange)
            {
                t.color = defaultTextColor;
            }
        }
    }
}