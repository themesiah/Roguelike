using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.ScriptableArchitecture.Sets
{
    public abstract class RuntimeSet<T> : ScriptableObject
    {
        public List<T> Items = new List<T>();

        public void Add(T thing)
        {
            if (!Items.Contains(thing))
                Items.Add(thing);
        }

        public void Remove(T thing)
        {
            if (Items.Contains(thing))
                Items.Remove(thing);
        }

        public void ForEach(System.Action<T> action)
        {
            Items.ForEach(action);
        }

        public void Clear()
        {
            Items.Clear();
        }
    }
}