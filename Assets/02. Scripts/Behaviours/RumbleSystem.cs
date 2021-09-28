using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Values;
using UnityEngine.InputSystem;
using System.Collections;

namespace Laresistance.Behaviours
{
    public class RumbleSystem : MonoBehaviour
    {
        public struct RumbleSignal
        {
            public float lowFreq;
            public float highFreq;
            public float duration;
        }

        [SerializeField]
        private ScriptableBoolReference rumbleActiveReference = default;

        private void Awake()
        {
            StopMotors();
        }

        private void OnEnable()
        {
            rumbleActiveReference.RegisterOnChangeAction(OnRumbleOptionChanged);
        }

        private void OnRumbleOptionChanged(bool option)
        {
            if (option == false)
            {
                StopAllCoroutines();
                StopMotors();
            }
        }

        public void Rumble(RumbleSignal signal)
        {
            if (rumbleActiveReference.GetValue() == true)
            {
                if (Gamepad.current != null)
                {
                    Gamepad.current.SetMotorSpeeds(signal.lowFreq, signal.highFreq);
                    StopAllCoroutines();
                    StartCoroutine(StopRumble(signal.duration));
                }
            }
        }

        private IEnumerator StopRumble(float duration)
        {
            yield return new WaitForSecondsRealtime(duration);
            StopMotors();
        }

        private void StopMotors()
        {
            if (Gamepad.current != null)
            {
                Gamepad.current.SetMotorSpeeds(0f, 0f);
            }
        }
    }
}