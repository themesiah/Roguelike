using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.ScriptableArchitecture.Events
{
    [CreateAssetMenu(menuName = "Scriptable Architecture/Events/Audio game event")]
    public class AudioGameEvent : TemplatedGameEvent<Audio.ScriptableAudioClip>
    {
    }
}