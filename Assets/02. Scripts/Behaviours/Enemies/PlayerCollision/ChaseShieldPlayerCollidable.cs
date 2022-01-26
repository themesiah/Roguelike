using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class ChaseShieldPlayerCollidable : MonoBehaviour, IPlayerCollidable
    {
        private string currentStatus = "Move";
        private bool direction = true;

        public bool PlayerAttacked(Transform playerTransform)
        {
            if (currentStatus == "Move")
            {
                return true;
            } else if (playerTransform.position.x < transform.position.x && direction == false)
            {
                return false;
            } else if (playerTransform.position.x > transform.position.x && direction == true)
            {
                return false;
            } else
            {
                return true;
            }
        }

        public void SetDirection(bool direction)
        {
            this.direction = direction;
        }

        public void SetStatus(string status)
        {
            currentStatus = status;
        }
    }
}