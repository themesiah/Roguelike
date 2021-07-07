using Laresistance.Extensions;
using System.Collections;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class AnimatorWrapperBehaviour : MonoBehaviour, IBattleAnimator
    {
        [SerializeField]
        private Animator animator = default;
        [SerializeField]
        private string horizontalSpeedParameterName = default;
        [SerializeField]
        private string verticalSpeedParameterName = default;
        [SerializeField]
        private string fallingParameterName = default;

        private bool animating = false;
        private float lastAnimatorSpeed = 0f;
        private float timer = 0f;
        private static float TIMEOUT = 5f;
        private bool paused = false;

        public void Start()
        {
            InitAnimator();
        }

        public void InitAnimator()
        {
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
                if (animator != null)
                {
                    lastAnimatorSpeed = animator.speed;
                }
            }
        }

        public void PlayAnimationSync(string trigger)
        {
            StartCoroutine(PlayAnimation(trigger));
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
                    timer += Time.deltaTime * animator.speed;
                    yield return null;
                }
                animating = false;
                if (timer >= TIMEOUT)
                {
                    GamedevsToolbox.Utils.Logger.Logger.LogWarning(string.Format("Animation trigger {0} caused a timeout. Stopping.", trigger));
                    if (animator != null)
                    {
                        animator.SetTrigger("Stop");
                    }
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
            Debug.Log("Received animation end signal");
            animating = false;
        }

        public void Pause()
        {
            if (!paused)
            {
                paused = true;
                lastAnimatorSpeed = animator.speed;
                animator.speed = 0f;
            }
        }

        public void Resume()
        {
            if (paused)
            {
                animator.speed = lastAnimatorSpeed;
                paused = false;
            }
        }

        public void Stop()
        {
            animator.SetTrigger("Stop");
        }

        public void SetHorizontalSpeed(float speed)
        {
            if (animator.HasParameter(horizontalSpeedParameterName))
                animator.SetFloat(horizontalSpeedParameterName, speed);
        }

        public void SetVerticalSpeed(float speed)
        {
            if (animator.HasParameter(verticalSpeedParameterName))
                animator.SetFloat(verticalSpeedParameterName, speed);
        }

        public void SetFalling(bool falling)
        {
            if (animator.HasParameter(fallingParameterName))
                animator.SetBool(fallingParameterName, falling);
        }

        public void SetTrigger(string trigger)
        {
            if (animator.HasParameter(trigger))
                animator.SetTrigger(trigger);
        }

        public void ResetTrigger(string trigger)
        {
            if (animator.HasParameter(trigger))
                animator.ResetTrigger(trigger);
        }

        public bool IsAnimating()
        {
            return animating;
        }
    }
}