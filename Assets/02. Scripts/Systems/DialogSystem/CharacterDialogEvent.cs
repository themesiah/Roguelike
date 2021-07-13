using GamedevsToolbox.ScriptableArchitecture.Events;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Laresistance.Systems.Dialog
{
    [CreateAssetMenu(menuName = "Laresistance/Events/Character Dialog Event")]
    public class CharacterDialogEvent : ScriptableObject
    {
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private readonly List<DialogSystem> eventListeners = new List<DialogSystem>();

        public IEnumerator Raise(CharacterDialog data)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                yield return eventListeners[i].OnEventRaised(data);
        }

        public void RegisterListener(DialogSystem listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void UnregisterListener(DialogSystem listener)
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