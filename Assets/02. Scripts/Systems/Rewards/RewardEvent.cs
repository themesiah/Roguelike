using Laresistance.Behaviours;
using Laresistance.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Systems
{
    [CreateAssetMenu(menuName = "Laresistance/Events/Reward Event")]
    public class RewardEvent : ScriptableObject
    {
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private readonly List<RewardSystemBehaviour> eventListeners = new List<RewardSystemBehaviour>();

        public IEnumerator Raise(RewardData data)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                yield return eventListeners[i].OnEventRaised(data);
        }

        public void RegisterListener(RewardSystemBehaviour listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void UnregisterListener(RewardSystemBehaviour listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }

        public void UnregisterAll()
        {
            eventListeners.Clear();
        }
    }
}