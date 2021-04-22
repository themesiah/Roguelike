using UnityEngine;
using UnityEngine.Events;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Behaviours
{
    public class VelocityToAnimatorSpeedConversor : MonoBehaviour
    {
        [SerializeField]
        private ScriptableFloatReference maxHorizontalSpeedReference = default;
        [SerializeField]
        private ScriptableFloatReference maxVerticalSpeedReference = default;
        [SerializeField]
        private UnityEvent<float> OnHorizontalSpeedConverted = default;
        [SerializeField]
        private UnityEvent<float> OnVerticalSpeedConverted = default;

        public void ConvertHorizontalSpeed(float speed)
        {
            if (maxHorizontalSpeedReference != null && maxHorizontalSpeedReference.GetValue() != 0f)
            {
                OnHorizontalSpeedConverted?.Invoke(speed / maxHorizontalSpeedReference.GetValue());
            } else
            {
                OnHorizontalSpeedConverted?.Invoke(speed);
            }
        }

        public void ConvertVerticalSpeed(float speed)
        {
            if (maxVerticalSpeedReference != null && maxVerticalSpeedReference.GetValue() != 0f)
            {
                OnVerticalSpeedConverted?.Invoke(speed / maxVerticalSpeedReference.GetValue());
            }
            else
            {
                OnVerticalSpeedConverted?.Invoke(speed);
            }
        }
    }
}