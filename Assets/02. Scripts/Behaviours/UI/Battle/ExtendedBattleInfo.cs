using UnityEngine;
using UnityEngine.UI;
using GamedevsToolbox.ScriptableArchitecture.Sets;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Behaviours
{
    public class ExtendedBattleInfo : MonoBehaviour
    {
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
            } else
            {
                sideObject = rightSideRef.Get();
                foreach (HorizontalOrVerticalLayoutGroup layout in layouts)
                {
                    layout.childAlignment = TextAnchor.MiddleRight;
                    healthStatusLayout.reverseArrangement = true;
                }
            }

            transform.SetParent(sideObject.transform, false);
        }

        public void Unsetup()
        {
            transform.SetParent(originalParent, false);
            gameObject.SetActive(false);
        }
    }
}