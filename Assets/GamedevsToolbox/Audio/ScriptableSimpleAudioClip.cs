using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.Audio
{
    [CreateAssetMenu(menuName = "Audio/Scriptable Simple Audio Clip")]
    public class ScriptableSimpleAudioClip : ScriptableAudioClip
    {
        [SerializeField]
        private AudioClip clip = default;

        public override AudioClip GetClip()
        {
            return clip;
        }

        public override float GetPitch()
        {
            return 1f;
        }

        public override float GetVolume()
        {
            return 1f;
        }
    }
}