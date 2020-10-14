using Laresistance.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Laresistance.Behaviours
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorWrapperBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Animator animator = default;

        private bool animating = false;
        private string currentAnimating = "";
        private float lastAnimatorSpeed = 0f;
        private static string ASSERT_ANIMATION_FORMAT = "Trying to animate while currently animating. Last played: {0}. Tried to play: {1}";

        public void Start()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
                lastAnimatorSpeed = animator.speed;
            }
        }

        public IEnumerator PlayAnimation(string trigger)
        {
            Assert.IsFalse(animating, string.Format(ASSERT_ANIMATION_FORMAT, currentAnimating, trigger));
            animating = true;
#if UNITY_EDITOR
            currentAnimating = trigger;
#endif
            if (animator.HasParameter(trigger))
            {
                animator.SetTrigger(trigger);
                while (animating == true)
                {
                    yield return null;
                }
            } else
            {
                animating = false;
                Debug.LogWarning(string.Format("Animator doesn't have the trigger {0}", trigger));
                yield return null;
            }
        }

        public void ReceiveAnimationEndSignal()
        {
            animating = false;
        }

        public void Pause()
        {
            lastAnimatorSpeed = animator.speed;
            animator.speed = 0f;
        }

        public void Resume()
        {
            animator.speed = lastAnimatorSpeed;
        }
    }
}