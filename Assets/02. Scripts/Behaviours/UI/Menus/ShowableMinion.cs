using Laresistance.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class ShowableMinion : MonoBehaviour, IShowableGameElement
    {
        [SerializeField]
        private RectTransform minionPortraitParent = default;
        [SerializeField]
        private ShowableAbility[] minionAbilities = default;
        [SerializeField]
        private Text minionName = default;
        [SerializeField]
        private Text minionLevel = default;
        [SerializeField]
        private float scaleMultiplier = 40;
        [SerializeField]
        private int sortingOrder = 200;

        public void SetupShowableElement(ShowableElement showableElement)
        {
            int childs = minionPortraitParent.childCount;
            for (int i = childs - 1; i >= 0; --i)
            {
                Destroy(minionPortraitParent.GetChild(i).gameObject);
            }

            if (showableElement == null)
            {
                for (int i = 0; i < minionAbilities.Length; ++i)
                {
                    minionAbilities[i].SetupShowableElement(null);
                }
                if (minionLevel != null)
                {
                    minionLevel.text = "";
                }
                if (minionName != null)
                {
                    minionName.text = "";
                }
            }
            else
            {
                Minion minion = (Minion)showableElement;

                if (minionName != null)
                {
                    minionName.text = minion.Name;
                }

                if (minionLevel != null)
                {
                    minionLevel.text = minion.Level.ToString();
                }

                for (int i = 0; i < minionAbilities.Length; ++i)
                {
                    minionAbilities[i].SetupShowableElement(minion.Abilities[i]);
                }

                if (minionPortraitParent != null)
                {
                    minion.Data.PrefabReference.InstantiateAsync(minionPortraitParent).Completed += (handler) => {
                        GameObject go = handler.Result;
                        go.transform.localPosition = Vector3.zero;
                        go.transform.localScale = go.transform.localScale * scaleMultiplier;
                        go.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
                    };
                }
            }
        }
    }
}