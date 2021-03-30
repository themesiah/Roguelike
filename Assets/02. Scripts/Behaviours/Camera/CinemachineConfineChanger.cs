using UnityEngine;
using Cinemachine;

namespace Laresistance.Behaviours
{
    public class CinemachineConfineChanger : MonoBehaviour
    {
        [SerializeField]
        private CinemachineConfiner confiner = default;

        public void ChangeConfinerShape(Collider2D collider)
        {
            confiner.m_BoundingShape2D = collider;
        }
    }
}