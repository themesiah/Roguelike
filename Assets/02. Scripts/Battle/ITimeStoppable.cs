using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Battle
{
    public interface ITimeStoppable
    {
        void PerformTimeStop(bool activate);
    }
}