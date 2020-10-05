using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Movement
{
    public interface IMovementManager
    {
        void Tick(float delta);
        void Stop();
        void Resume();
    }
}