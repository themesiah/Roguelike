using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.Audio
{
    public class ScriptableAudioClip : ScriptableObject
    {
        public virtual AudioClip GetClip()
        {
            return default;
        }

        public virtual float GetPitch()
        {
            return 1f;
        }

        public virtual float GetVolume()
        {
            return 1f;
        }
    }
}