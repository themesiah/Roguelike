using UnityEngine;
using System.Collections;

namespace Laresistance.Behaviours
{
    public class DummyBattleAnimator : IBattleAnimator
    {
        public bool IsAnimating()
        {
            return false;
        }

        public void Pause()
        {}

        public IEnumerator PlayAnimation(string trigger)
        {
            yield return null;
        }

        public void ResetTrigger(string trigger)
        {}

        public void Resume()
        {}

        public void SetTrigger(string trigger)
        {}

        public void Stop()
        {}
    }
}