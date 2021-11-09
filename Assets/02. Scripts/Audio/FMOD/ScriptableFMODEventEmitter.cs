using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Audio
{
    [CreateAssetMenu(menuName = "Laresistance/Audio/FMOD Event Emitter")]
    public class ScriptableFMODEventEmitter : ScriptableObject
    {
        [FMODUnity.EventRef]
        [SerializeField]
        private string eventName = default;
        [SerializeField]
        private string[] parameterNames = default;
        [SerializeField]
        private float[] parameterValues = default;
        [SerializeField]
        private float cooldown = 0f;

        private System.Guid eventId;
        private FMOD.Studio.EventDescription eventDescription;
        protected FMOD.Studio.PARAMETER_DESCRIPTION[] paramDescription = null;
        protected FMOD.Studio.EventInstance instance;
        [System.NonSerialized]
        protected float lastPlayed = 0f;

        private void Init()
        {
            if (!eventDescription.isValid())
            {
                eventDescription = FMODUnity.RuntimeManager.GetEventDescription(eventName);
                UnityEngine.Assertions.Assert.IsTrue(eventDescription.isValid());
                eventDescription.getID(out eventId);
            }
            if (eventDescription.isValid())
            {
                if (paramDescription == null)
                {
                    paramDescription = new FMOD.Studio.PARAMETER_DESCRIPTION[parameterNames.Length];
                    for(int i = 0; i < paramDescription.Length; ++i)
                    {
                        eventDescription.getParameterDescriptionByName(parameterNames[i], out paramDescription[i]);
                    }
                }
            }
        }

        private void GetInstance()
        {
            if (eventId != null && !instance.isValid())
            {
                instance = FMODUnity.RuntimeManager.CreateInstance(eventId);
                UnityEngine.Assertions.Assert.IsTrue(instance.isValid());
            }
        }

        protected virtual void SetParameters()
        {
            for (int i = 0; i < paramDescription.Length; ++i)
            {
                float value = 0f;
                if (parameterValues.Length > i)
                {
                    value = parameterValues[i];
                }
                instance.setParameterByID(paramDescription[i].id, value);
            }
        }

        public void Play()
        {
            Init();
            GetInstance();
            SetParameters();
            //Debug.LogFormat("Current time {0}, last played {1}, cooldown {2}", Time.time, lastPlayed, cooldown);
            if (Time.time > lastPlayed + cooldown)
            {
                instance.start();
                instance.release();
                lastPlayed = Time.time;
            }
        }
    }
}