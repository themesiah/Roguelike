using UnityEngine;
using System.Collections;

namespace GamedevsToolbox.ScriptableArchitecture.Events
{
    public interface IGameEventListener<T>
    {
        void Register();
        void Unregister();
        void OnEventRaised(T data);
    }
}