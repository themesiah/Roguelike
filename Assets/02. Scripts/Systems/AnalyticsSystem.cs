using System.Collections.Generic;
using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Values;
using UnityEngine.Analytics;

namespace Laresistance.Systems
{
    public class AnalyticsSystem : MonoBehaviour
    {
        [SerializeField]
        private ScriptableBoolReference analyticsActivated = default;
        [SerializeField]
        private bool logEvents = default;

        public static AnalyticsSystem Instance = null;

        private void Start()
        {
            Instance = this;
            if (analyticsActivated.GetValue())
            {
                Log("Analytics are activated");
            } else
            {
                Log("Analytics are NOT activated");
            }
        }

        public void CustomEvent(string eventName, Dictionary<string, object> parameters)
        {
            if (analyticsActivated.GetValue())
            {
                Analytics.CustomEvent(eventName, parameters);
                Log(string.Format("Event '{0}' sent with {1} parameters.", eventName, parameters.Count));
            }
        }

        private void Log(string text)
        {
            if (logEvents)
            {
                Debug.LogFormat("[Analytics] {0}", text);
            }
        }
    }
}