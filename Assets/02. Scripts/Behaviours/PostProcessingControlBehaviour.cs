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
        [SerializeField]
        private ScriptableFloatReference gainValueRef = default;

        private ChromaticAberration chromaticAberration;
        private LiftGammaGain liftGammaGain;

        private void Start()
        {
            InitializeChromaticAberration();
            InitializeLiftGammaGain();
        }

        private void InitializeChromaticAberration()
        {
            volume.profile.TryGet(out chromaticAberration);
            ChromaticAberrationChangeValue(0f);
        }

        private void InitializeLiftGammaGain()
        {
            volume.profile.TryGet(out liftGammaGain);
            LiftGammaGainChangeValue(0f);
        }

        private void OnEnable()
        {
            chromaticAberrationValueRef.RegisterOnChangeAction(ChromaticAberrationChangeValue);
            gainValueRef.RegisterOnChangeAction(LiftGammaGainChangeValue);
        }

        private void OnDisable()
        {
            chromaticAberrationValueRef.UnregisterOnChangeAction(ChromaticAberrationChangeValue);
            gainValueRef.UnregisterOnChangeAction(LiftGammaGainChangeValue);
        }

        public void ChromaticAberrationChangeValue(float value)
        {
            if (chromaticAberration != null)
            {
                chromaticAberration.intensity.value = value * GameConstantsBehaviour.Instance.chromaticAberrationMaxIntensity.GetValue();
            }
        }

        public void LiftGammaGainChangeValue(float value)
        {
            if (liftGammaGain != null)
            {
                liftGammaGain.gain.value = Vector4.one * -value;
            }
        }
    }
}