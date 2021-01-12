using GamedevsToolbox.ScriptableArchitecture.Values;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class GameConstantsBehaviour : MonoBehaviour
    {
        public static GameConstantsBehaviour Instance;

        [Header("Card system constants")]
        [Tooltip("How much energy do you obtain from shuffling per every card")]
        public ScriptableFloatReference energyPerCardReference = default;
        [Tooltip("How much time to wait to get a card after using one or obtaining the later")]
        public ScriptableFloatReference cardRenewCooldown = default;
        [Tooltip("How much time to wait to shuffle the cards since the beginning of the battle or the last shuffle")]
        public ScriptableFloatReference shuffleCooldown = default;
        [Tooltip("How much energy are you able to have at a time")]
        public ScriptableFloatReference maxEnergy = default;
        [Header("Status constants")]
        [Tooltip("Time in seconds of duration for speed buffs and debuffs")]
        public ScriptableFloatReference speedModifierDuration = default;
        [Tooltip("Time in seconds of duration for damage buffs and debuffs")]
        public ScriptableFloatReference damageModifierDuration = default;
        [Tooltip("Time in seconds of duration for damage over time effects")]
        public ScriptableFloatReference damageOverTimeDuration = default;
        [Tooltip("Time in seconds between damages in damage over time effects")]
        public ScriptableFloatReference damageOverTimeTickDelay = default;

        private void Awake()
        {
            GameConstantsBehaviour.Instance = this;
        }
    }
}