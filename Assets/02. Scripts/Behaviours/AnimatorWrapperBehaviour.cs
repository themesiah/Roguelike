using Laresistance.Battle;
using System.Collections;
using System.Collections.Generic;
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
        private static string ASSERT_ANIMATION_FORMAT = "Trying to animate while currently animating. Last played: {0}. Tried to play: {1}";
        private static string ATTACK_ANIMATION_NAME = "Attack";
        private static string DAMAGE_ANIMATION_NAME = "Damage";
        private static string EFFECT_ANIMATION_NAME = "Effect";

        public BattleAbilityManager.AnimationToExecuteHandler PlayAttackAnimationHandler;
        public BattleAbilityManager.AnimationToExecuteHandler PlayDamageAnimationHandler;
        public BattleAbilityManager.AnimationToExecuteHandler PlayEffectAnimationHandler;

        public void Start()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            PlayAttackAnimationHandler = PlayAttackAnimation;
            PlayDamageAnimationHandler = PlayReceiveDamageAnimation;
            PlayEffectAnimationHandler = PlayEffectAnimation;
        }

        public IEnumerator PlayAttackAnimation()
        {
            Assert.IsFalse(animating, string.Format(ASSERT_ANIMATION_FORMAT, currentAnimating, "PlayAttackAnimation"));
            animating = true;
#if UNITY_EDITOR
            currentAnimating = "PlayAttackAnimation";
#endif
            animator.Play(ATTACK_ANIMATION_NAME);
            while (animating == true)
            {
                yield return null;
            }
        }

        public IEnumerator PlayReceiveDamageAnimation()
        {
            Assert.IsFalse(animating, string.Format(ASSERT_ANIMATION_FORMAT, currentAnimating, "PlayReceiveDamageAnimation"));
            animating = true;
#if UNITY_EDITOR
            currentAnimating = "PlayReceiveDamageAnimation";
#endif
            animator.Play(DAMAGE_ANIMATION_NAME);
            while (animating == true)
            {
                yield return null;
            }
        }

        public IEnumerator PlayEffectAnimation()
        {
            Assert.IsFalse(animating, string.Format(ASSERT_ANIMATION_FORMAT, currentAnimating, "PlayEffectAnimation"));
            animating = true;
#if UNITY_EDITOR
            currentAnimating = "PlayEffectAnimation";
#endif
            animator.Play(EFFECT_ANIMATION_NAME);
            while (animating == true)
            {
                yield return null;
            }
        }

        public void ReceiveAnimationEndSignal()
        {
            animating = false;
        }
    }
}