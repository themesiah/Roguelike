using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Behaviours
{
    public class PostProcessingControlBehaviour : MonoBehaviour
    {
        [SerializeField]
        Volume volume = default;
        [SerializeField]
        private ScriptableFloatReference chromaticAberrationValueRef = default;

        private ChromaticAberration chromaticAberration;

        private void Start()
        {
            InitializeChromaticAberration();

        }

        private void InitializeChromaticAberration()
        {
            volume.profile.TryGet(out chromaticAberration);
            ChromaticAberrationChangeValue(0f);
        }

        private void OnEnable()
        {
            chromaticAberrationValueRef.RegisterOnChangeAction(ChromaticAberrationChangeValue);
        }

        private void OnDisable()
        {
            chromaticAberrationValueRef.UnregisterOnChangeAction(ChromaticAberrationChangeValue);
        }

        public void ChromaticAberrationChangeValue(float value)
        {
            if (chromaticAberration != null)
            {
                chromaticAberration.intensity.value = value * GameConstantsBehaviour.Instance.chromaticAberrationMaxIntensity.GetValue();
            }
        }
    }
}