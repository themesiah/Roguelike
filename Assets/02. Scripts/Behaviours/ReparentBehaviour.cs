using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Sets;

namespace Laresistance.Behaviours
{
    public class ReparentBehaviour : MonoBehaviour
    {
        private Transform originalParent = null;
        private void Awake()
        {
            SetOriginalParent();
        }

        private void SetOriginalParent()
        {
            if (originalParent == null)
            {
                originalParent = transform.parent;
            }
        }

        public void Reparent(Transform parent)
        {
            SetOriginalParent();
            transform.SetParent(parent, false);
        }

        public void Reparent(RuntimeSingleGameObject gameObjectRef)
        {
            SetOriginalParent();
            Reparent(gameObjectRef.Get().transform);
        }

        public void Reparent(HealthBattleUIParentSelector selector)
        {
            SetOriginalParent();
            Reparent(selector.Get());
        }

        public void ReparentToOriginal()
        {
            transform.SetParent(originalParent, false);
        }
    }
}