using UnityEngine;

namespace Laresistance.Minimap
{
    public class Miniroom : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer roomRenderer = default;
        [SerializeField]
        [Tooltip("Position of the room circle in the minimap prefab")]
        private Transform miniroomCenter = default;

        private bool hasPilgrim = false;

        public Vector3 Center => miniroomCenter.position;
        public void SetPilgrim() => hasPilgrim = true;
        public void Show()
        {
            roomRenderer.enabled = true;
            if (hasPilgrim)
            {
                roomRenderer.color = Color.blue;
            }
        }
    }
}