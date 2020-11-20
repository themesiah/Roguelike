using Laresistance.Extensions;
using System.Collections;
using UnityEngine;

namespace Laresistance.Behaviours
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorWrapperBehaviour : MonoBehaviour, IBattleAnimator
    {
        [SerializeField]
        private Animator animator = default;

        private bool animating = false;
        private float lastAnimatorSpeed = 0f;
        private float timer = 0f;
        private static float TIMEOUT = 5f;

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
            animating = true;
            if (animator.HasParameter(trigger))
            {
                animator.SetTrigger(trigger);
                timer = 0f;
                while (animating == true && timer < TIMEOUT)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }
                animating = false;
                if (timer >= TIMEOUT)
                {
                    GamedevsToolbox.Utils.Logger.Logger.LogWarning(string.Format("Animation trigger {0} caused a timeout.", trigger));
                }
            } else
            {
                animating = false;
                GamedevsToolbox.Utils.Logger.Logger.LogWarning(string.Format("Animator doesn't have the trigger {0}", trigger));
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

        public void Stop()
        {
            animator.SetTrigger("Stop");
        }
    }
}