using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.Audio {
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceWrapper : MonoBehaviour
    {
        [SerializeField]
        private AudioSource source = default;

        [SerializeField]
        private float fadeTime = 1f;

        [SerializeField]
        private ScriptableArchitecture.Events.LocatedAudioGameEvent locatedAudioEvent = default;

        private float targetVolume = 1f;

        public void ChangeVolume(ScriptableArchitecture.Values.ScriptableFloatReference volumeReference)
        {
            source.volume = volumeReference.GetValue();
        }

        public void PlayClip(ScriptableAudioClip audioClip)
        {
            PlayClip(audioClip, false);
        }

        public void PlayClip(ScriptableAudioClip audioClip, bool isLocated)
        {
            AudioClip clip = audioClip.GetClip();
            float pitch = audioClip.GetPitch();
            source.pitch = pitch;
            source.volume = audioClip.GetVolume();
            source.PlayOneShot(clip);
            if (!isLocated)
                locatedAudioEvent?.Raise(new LocatedAudioEvent { audioClip = audioClip, location = transform.position });
        }

        public void PlayClip(DetectedAudioEvent audioEvent)
        {
            PlayClip(audioEvent.audioClip, true);
            source.volume *= audioEvent.volume;
        }

        public void PlayBGM(ScriptableAudioClip audioClip)
        {
            targetVolume = audioClip.GetVolume();

            source.volume = 0f;
            AudioClip clip = audioClip.GetClip();
            float pitch = audioClip.GetPitch();
            source.pitch = pitch;
            source.clip = clip;
            source.Play();

            StartCoroutine(FadeIn());
        }

        public void StopSource()
        {
            StartCoroutine(FadeOut());
        }

        private IEnumerator FadeIn()
        {
            float timer = 0f;
            while (timer < fadeTime)
            {
                timer += Time.deltaTime;
                source.volume = timer / fadeTime * targetVolume;
                yield return null;
            }
            source.volume = targetVolume;
        }

        private IEnumerator FadeOut()
        {
            float timer = 0f;
            while (timer < fadeTime)
            {
                timer += Time.deltaTime;
                source.volume = targetVolume - (timer / fadeTime * targetVolume);
                yield return null;
            }
            source.Stop();
            source.volume = targetVolume;
        }
    }
}