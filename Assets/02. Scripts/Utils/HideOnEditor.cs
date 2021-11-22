using UnityEngine;

namespace Laresistance
{
    public class HideOnEditor : MonoBehaviour
    {
        [SerializeField]
        private bool hideOnBuild = true;

        void Awake()
        {
#if UNITY_EDITOR
            if (!hideOnBuild)
            {
                gameObject.SetActive(false);
            }
#else
            if (hideOnBuild)
            {
                gameObject.SetActive(false);
            }
#endif
        }
    }
}