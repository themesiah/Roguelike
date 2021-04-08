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

        private UnityAction OnFinishSignal = delegate { };

        private void Awake()
        {
            UnityEngine.Assertions.Assert.IsTrue(destroyTime > effectDuration);
            Destroy(gameObject, 10f); // TIMEOUT, just in case
        }

        public void ConfigureBattleEffectCallback(UnityAction callback, float delay)
        {
            OnFinishSignal += callback;
            StartCoroutine(EffectCoroutine(delay));
        }

        private IEnumerator EffectCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject, destroyTime);
            onPlay?.Invoke();
            yield return new WaitForSeconds(effectDuration);
            OnFinishSignal?.Invoke();
        }
    }
}