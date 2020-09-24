using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.Dictionary
{
    [System.Serializable]
    public class SerializableDictionary<T, T2> : Dictionary<T, T2>
    {
        [System.Serializable]
        public class SerializableDictionaryValue
        {
            public T key;
            public T2 value;
        }

        [SerializeField]
        private List<SerializableDictionaryValue> valuesList = default;

        private bool initialized = false;

        public void Init()
        {
            if (initialized)
                return;

            initialized = true;
            foreach (var kvp in valuesList)
            {
                UnityEngine.Assertions.Assert.IsFalse(ContainsKey(kvp.key), string.Format("Key {0} is repeated in the dictionary", kvp.key));
                Add(kvp.key, kvp.value);
            }
        }
        
        public new void Add(T key, T2 value)
        {
            Init();
            base.Add(key, value);
        }

        public new void Clear()
        {
            Init();
            base.Clear();
        }

        public new void TryGetValue(T key, out T2 value)
        {
            Init();
            base.TryGetValue(key, out value);
        }

        public new void Remove(T key)
        {
            Init();
            base.Remove(key);
        }

        public new bool ContainsKey(T key)
        {
            Init();
            return base.ContainsKey(key);
        }

        public new bool ContainsValue(T2 value)
        {
            Init();
            return base.ContainsValue(value);
        }

        public new Dictionary<T,T2>.Enumerator GetEnumerator()
        {
            Init();
            return base.GetEnumerator();
        }

        public new int Count
        {
            get
            {
                Init();
                return base.Count;
            }
        }

        public new IEqualityComparer<T> Comparer
        {
            get
            {
                Init();
                return base.Comparer;
            }
        }

        public new Dictionary<T,T2>.KeyCollection Keys
        {
            get
            {
                Init();
                return base.Keys;
            }
        }

        public new Dictionary<T,T2>.ValueCollection Values
        {
            get
            {
                Init();
                return base.Values;
            }
        }
    }
}