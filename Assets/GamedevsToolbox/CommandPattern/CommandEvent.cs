using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.CommandPattern
{
    [CreateAssetMenu(menuName = "Scriptable Architecture/Events/Generic game event")]
    public class CommandEvent : ScriptableObject
    {
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private readonly List<CommandProcessor> eventListeners =
            new List<CommandProcessor>();

        public void Raise(Command data)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised(data);
        }

        public void RegisterListener(CommandProcessor listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void UnregisterListener(CommandProcessor listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }
}