using UnityEngine;

namespace Laresistance.Behaviours
{
    public class AnimatorWrapperBridge : MonoBehaviour
    {
        [SerializeField]
        private AnimatorWrapperBehaviour animatorWrapper = default;

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

        public void ResetTrigger(string trigger)
        {
            animatorWrapper?.ResetTrigger(trigger);
        }
    }
}