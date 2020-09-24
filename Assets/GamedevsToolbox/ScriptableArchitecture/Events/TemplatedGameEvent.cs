﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.ScriptableArchitecture.Events
{
    public abstract class TemplatedGameEvent<T> : ScriptableObject
    {
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private readonly List<TemplatedGameEventListener<T>> eventListeners =
            new List<TemplatedGameEventListener<T>>();

        public void Raise(T data)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised(data);
        }

        public void RegisterListener(TemplatedGameEventListener<T> listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void UnregisterListener(TemplatedGameEventListener<T> listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }
}