using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.ScriptableArchitecture.Events
{
    [CreateAssetMenu(menuName = "Scriptable Architecture/Events/String game event")]
    public class StringGameEvent : TemplatedGameEvent<string>
    {
    }
}