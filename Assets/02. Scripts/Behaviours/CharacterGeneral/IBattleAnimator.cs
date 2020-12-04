using GamedevsToolbox.Utils;
using System.Collections;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public interface IBattleAnimator : IPausable
    {
        IEnumerator PlayAnimation(string trigger);
        void Stop();
        void SetAttackPosition(Vector3 position);
    }
}