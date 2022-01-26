using UnityEngine;

namespace Laresistance.Behaviours
{
    public class NonAdvantagePlayerCollidable : MonoBehaviour, IPlayerCollidable
    {
        public bool PlayerAttacked(Transform playerTransform)
        {
            return true;
        }

        public void SetDirection(bool direction)
        {
        }

        public void SetStatus(string status)
        {
        }
    }
}