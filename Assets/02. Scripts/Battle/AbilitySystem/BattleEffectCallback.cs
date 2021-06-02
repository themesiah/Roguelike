using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Laresistance.Battle
{
    public class BattleEffectCallback : MonoBehaviour
    {
        [SerializeField]
        private float effectDuration = default;
        [SerializeField]
        private float destroyTime = default;
        [SerializeField]
        private UnityEvent onPlay = default;

        protected UnityAction OnFinishSignal = delegate { };
        protected BattleStatusManager target;
        private bool finishSignalCalled = false;

        private void Awake()
        {
            UnityEngine.Assertions.Assert.IsTrue(destroyTime <= 0f || destroyTime > effectDuration);
            if (destroyTime > 0f)
            {
                Destroy(gameObject, 10f); // TIMEOUT, just in case
            }
        }

        public virtual void ConfigureBattleEffectCallback(BattleStatusManager target, UnityAction callback, float delay)
        {
            this.target = target;
            OnFinishSignal += callback;
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(EffectCoroutine(delay));
            } else
            {
                OnFinishSignal?.Invoke();
                finishSignalCalled = true;
                Destroy(gameObject);
            }
        }

        private IEnumerator EffectCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (destroyTime > 0f)
            {
                Destroy(gameObject, destroyTime);
            }
            onPlay?.Invoke();
            yield return new WaitForSeconds(effectDuration);
            finishSignalCalled = true;
            OnFinishSignal?.Invoke();
        }

        private void OnDestroy()
        {
            if (!finishSignalCalled)
            {
                OnFinishSignal?.Invoke();
            }
        }
    }
}