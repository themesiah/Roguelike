using System.Collections;
using System.Collections.Generic;
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

        public Vector3 Center => miniroomCenter.position;
        public void Show() => roomRenderer.enabled = true;
    }
}