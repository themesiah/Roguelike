using UnityEngine;
using UnityEngine.UI;

namespace Laresistance.Minimap
{
    public class Miniroom : MonoBehaviour
    {
        [SerializeField]
        private Image roomRenderer = default;
        [SerializeField]
        [Tooltip("Position of the room circle in the minimap prefab")]
        private Transform miniroomCenter = default;

        public Vector3 Center => miniroomCenter.position;
        public void Show()
        {
            roomRenderer.enabled = true;
        }
    }
}