using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.ScriptableArchitecture.Events
{
    [CreateAssetMenu(menuName = "Scriptable Architecture/Events/Located Audio game event")]
    public class LocatedAudioGameEvent : TemplatedGameEvent<Audio.LocatedAudioEvent>
    {
    }
}