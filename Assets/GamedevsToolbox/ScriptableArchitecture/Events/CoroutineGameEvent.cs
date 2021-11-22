using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace GamedevsToolbox.ScriptableArchitecture.Events
{
    [CreateAssetMenu(menuName = "Scriptable Architecture/Events/Coroutine Game Event")]
    public class CoroutineGameEvent : ScriptableObject
    {

        public delegate IEnumerator CoroutineToCall();
        public event CoroutineToCall coroutineToCall;

        public IEnumerator Raise()
        {
            yield return coroutineToCall();
        }

        public void RegisterListener(CoroutineToCall listener)
        {
            coroutineToCall += listener;
        }

        public void UnregisterListener(CoroutineToCall listener)
        {
            coroutineToCall -= listener;
        }
    }
}