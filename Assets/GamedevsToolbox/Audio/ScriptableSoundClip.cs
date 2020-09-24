using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.Audio
{
    [CreateAssetMenu(menuName = "Audio/Scriptable Sound Clip")]
    public class ScriptableSoundClip : ScriptableAudioClip
    {
        [SerializeField]
        private ScriptableArchitecture.Selectors.ScriptableAudioClipSelector clipSelector = default;
        [SerializeField]
        private ScriptableArchitecture.Values.ScriptableFloatReference pitch = default;
        [SerializeField]
        private ScriptableArchitecture.Values.ScriptableFloatReference volume = default;

        public override AudioClip GetClip()
        {
            return clipSelector.Get();
        }

        public override float GetPitch()
        {
            return pitch.GetValue();
        }

        public override float GetVolume()
        {
            return volume.GetValue();
        }
    }
}