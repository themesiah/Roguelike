using UnityEngine;
using UnityEngine.Events;

namespace GamedevsToolbox.Utils
{
    public class RepeatingEvent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent repeatingEvent = default;

        [SerializeField]
        private ScriptableArchitecture.Values.ScriptableFloatReference delay = default;

        [SerializeField]
        private ScriptableArchitecture.Values.ScriptableBoolReference executeOnStart = default;

        [SerializeField]
        private ScriptableArchitecture.Values.ScriptableBoolReference unscaledDeltaTime = default;

        private float timer = 0f;

        private void Start()
        {
            if (executeOnStart.GetValue())
            {
                repeatingEvent.Invoke();
            }
        }

        private void Update()
        {
            if (unscaledDeltaTime.GetValue())
            {
                timer += Time.unscaledDeltaTime;
            } else
            {
                timer += Time.deltaTime;
            }

            if (timer >= delay.GetValue())
            {
                timer = 0f;
                repeatingEvent.Invoke();
            }
        }

        public void StartTimer()
        {
            enabled = true;
        }

        public void StopTimer()
        {
            enabled = false;
        }
    }
}