using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Data
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Status")]
    public class StatusData : ScriptableObject
    {
        [SerializeField]
        private StatusType status = default;
        public StatusType Status { get { return status; } }

        [SerializeField]
        private Sprite statusSprite = default;
        public Sprite StatusSprite { get { return statusSprite; } }

        [SerializeField]
        private Sprite statusFrame = default;
        public Sprite StatusFrame { get { return statusFrame; } }

        [SerializeField]
        private Color statusFrameColor = default;
        public Color StatusFrameColor { get { return statusFrameColor; } }
    }
}