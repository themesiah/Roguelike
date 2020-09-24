using UnityEngine;
using UnityEngine.Events;

namespace GamedevsToolbox.Audio
{
    public class LocatedAudioEventReceiver : ScriptableArchitecture.Events.LocatedAudioGameEventListener
    {
        [System.Serializable]
        public class DetectedAudioUnityEvent : UnityEvent<DetectedAudioEvent> { }
        [SerializeField]
        private ScriptableArchitecture.Values.ScriptableVector2Reference minMaxDistance = default;

        [SerializeField]
        private DetectedAudioUnityEvent detectionEvent = default;

        public override void OnEventRaised(Audio.LocatedAudioEvent data)
        {
            float distance = Vector3.Distance(data.location, transform.position);
            if (distance <= minMaxDistance.GetValue().y)
            {
                float volume = GetVolume(distance);
                detectionEvent.Invoke(new DetectedAudioEvent { audioClip = data.audioClip, volume = volume });
            }
        }
        
        protected virtual float GetVolume(float distance)
        {
            return 1f-Mathf.InverseLerp(minMaxDistance.GetValue().x, minMaxDistance.GetValue().y, distance);
        }
    }
}