using UnityEngine;
using UnityEngine.Events;

namespace Laresistance.Behaviours
{
    public class TimedEvaluator : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve evaluationCurve = default;

        [SerializeField]
        private UnityEvent<float> evaluationValue = default;

        [SerializeField]
        private float evaluationTime = 1f;

        [SerializeField]
        private bool pingPong = false;

        [SerializeField] [Range(0,1)]
        private float maxRandomTimeOffset = 0f;

        private float randomOffset = 0f;

        private void Start()
        {
            randomOffset = Random.Range(-maxRandomTimeOffset * evaluationTime, maxRandomTimeOffset * evaluationTime);
        }

        private void Update()
        {
            float timeValue = Time.time + randomOffset;
            if (pingPong)
            {
                timeValue = Mathf.PingPong(timeValue, evaluationTime);
            } else
            {
                timeValue = timeValue % evaluationTime;
            }
            float evaluation = evaluationCurve.Evaluate(timeValue / evaluationTime);
            evaluationValue?.Invoke(evaluation);
        }
    }
}