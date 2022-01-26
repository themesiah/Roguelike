using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public interface IPlayerCollidable
    {
        bool PlayerAttacked(Transform playerTransform);
        void SetStatus(string status);
        void SetDirection(bool direction);

    }
}