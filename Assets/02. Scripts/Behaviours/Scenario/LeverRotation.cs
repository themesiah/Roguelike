using System.Collections;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class LeverRotation : MonoBehaviour
    {
        [SerializeField]
        private float targetAngle = 30f;
        [SerializeField]
        private float moveTime = 1f;
        [SerializeField]
        private Transform pivot = default;

        private bool positive = true;
        private Coroutine leverCoroutine = null;
        private float currentTime = 0f;

        private void Awake()
        {
            var euler = pivot.eulerAngles;
            euler.z = targetAngle;
            pivot.eulerAngles = euler;
            currentTime = moveTime;
        }

        // Called externally
        public void MoveLever()
        {
            positive = !positive;
            currentTime = moveTime - currentTime;
            if (leverCoroutine != null)
            {
                StopCoroutine(leverCoroutine);
            }
            leverCoroutine = StartCoroutine(MoveLeverCoroutine());
        }

        private IEnumerator MoveLeverCoroutine()
        {
            float target = positive ? targetAngle : -targetAngle;
            var euler = pivot.eulerAngles;
            while (currentTime < moveTime)
            {
                float current = Mathf.Lerp(-target, target, currentTime / moveTime);
                euler.z = current;
                pivot.eulerAngles = euler;
                currentTime += Time.deltaTime;
                yield return null;
            }
            currentTime = moveTime;
        }
    }
}