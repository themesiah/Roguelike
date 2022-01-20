using System.Collections;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class AnimatorWrapperBridge : MonoBehaviour
    {
        [SerializeField]
        private AnimatorWrapperBehaviour animatorWrapper = default;

        public IEnumerator PlayAnimation(string trigger)
        {
            yield return animatorWrapper.PlayAnimation(trigger);
        }

        public void SetHorizontalSpeed(float speed)
        {
            animatorWrapper?.SetHorizontalSpeed(speed);
        }

        public void SetVerticalSpeed(float speed)
        {
            animatorWrapper?.SetVerticalSpeed(speed);
        }

        public void PlayAnimationSync(string trigger)
        {
            animatorWrapper?.PlayAnimationSync(trigger);
        }

        public void SetTrigger(string trigger)
        {
            animatorWrapper?.SetTrigger(trigger);
        }

        public void SetBoolTrue(string trigger)
        {
            animatorWrapper?.SetBool(trigger, true);
        }

        public void SetBoolFalse(string trigger)
        {
            animatorWrapper?.SetBool(trigger, false);
        }

        public void ResetTrigger(string trigger)
        {
            animatorWrapper?.ResetTrigger(trigger);
        }

        public void SetFalling(bool falling)
        {
            animatorWrapper?.SetFalling(falling);
        }
    }
}