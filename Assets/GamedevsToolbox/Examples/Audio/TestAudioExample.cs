using UnityEngine;
using UnityEngine.Events;

namespace GamedevsToolbox.Examples
{
    public class TestAudioExample : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent delayedEvent = default;

        [SerializeField]
        private ScriptableArchitecture.Values.ScriptableFloatReference delayBetweenEvents = default;

        private float timer = 0f;
        
        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= delayBetweenEvents.GetValue())
            {
                timer = 0f;
                delayedEvent.Invoke();
            }
        }
    }
}