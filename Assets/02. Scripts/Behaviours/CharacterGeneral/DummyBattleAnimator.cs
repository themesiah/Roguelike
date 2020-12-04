﻿using UnityEngine;
using System.Collections;

namespace Laresistance.Behaviours
{
    public class DummyBattleAnimator : IBattleAnimator
    {
        public void Pause()
        {}

        public IEnumerator PlayAnimation(string trigger)
        {
            yield return null;
        }

        public void Resume()
        {}

        public void Stop()
        {}

        public void SetAttackPosition(Vector3 position)
        {}
    }
}