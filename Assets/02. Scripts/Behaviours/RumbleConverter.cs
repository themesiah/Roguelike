using UnityEngine;
using Laresistance.Extensions;

namespace Laresistance.Behaviours
{
    public class RumbleConverter : MonoBehaviour
    {
        [SerializeField]
        private RumbleSignalEvent rumbleEvent = default;
        [Header("Damage received")]
        [SerializeField]
        private float damageRumbleTime = 0.3f;
        [SerializeField]
        private int maxDamage = 40;
        [Header("Stun")]
        [SerializeField]
        private float stunRumbleTime = 0.2f;
        [SerializeField]
        private float lowRumbleStun = 0.8f;
        [SerializeField]
        private float highRumbleStun = 1f;
        [Header("Slow")]
        [SerializeField]
        private float slowRumbleTime = 0.5f;
        [SerializeField]
        private float lowRumbleSlow = 0.2f;
        [SerializeField]
        private float highRumbleSlow = 0f;
        [Header("Poison")]
        [SerializeField]
        private float poisonRumbleTime = 1f;
        [SerializeField]
        private float lowRumblePoison = 0.0f;
        [SerializeField]
        private float highRumblePoison = 0.1f;

        public void OnReceiveDamage(int damage)
        {
            RumbleSystem.RumbleSignal signal = new RumbleSystem.RumbleSignal();
            signal.duration = damageRumbleTime;
            float highValue = (float)damage / (float)maxDamage;
            float lowValue = highValue / 2f;
            signal.lowFreq = lowValue;
            signal.highFreq = highValue;
            rumbleEvent?.Raise(signal);
        }

        public void OnStun()
        {
            RumbleSystem.RumbleSignal signal = new RumbleSystem.RumbleSignal();
            signal.duration = stunRumbleTime;
            signal.lowFreq = lowRumbleStun;
            signal.highFreq = highRumbleStun;
            rumbleEvent?.Raise(signal);
        }

        public void OnSlow()
        {
            RumbleSystem.RumbleSignal signal = new RumbleSystem.RumbleSignal();
            signal.duration = slowRumbleTime; ;
            signal.lowFreq = lowRumbleSlow;
            signal.highFreq = highRumbleSlow;
            rumbleEvent?.Raise(signal);
        }

        public void OnPoison()
        {
            RumbleSystem.RumbleSignal signal = new RumbleSystem.RumbleSignal();
            signal.duration = poisonRumbleTime;
            signal.lowFreq = lowRumblePoison;
            signal.highFreq = lowRumblePoison;
            rumbleEvent?.Raise(signal);
        }
    }
}