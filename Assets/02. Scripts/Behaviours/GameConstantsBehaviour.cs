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
        [Tooltip("Time between pressing the time stop button and the time being stopped completely")]
        public ScriptableFloatReference stopTimeDelay = default;
        [Tooltip("Time for an ability to go from the 'queued' state to 'executing'. It should be pretty small for the player not to notice it too much")]
        public ScriptableFloatReference abilityToUseDequeueTimer = default;

        [Header("Enemy in battle constants")]
        [Tooltip("Time between abilities cast by enemies in battle, unless overrided by the ability")]
        public ScriptableFloatReference nextAbilityCooldownDefault = default;
        [Tooltip("Variance in the time between abilities by enemies in battle, as a +/-")]
        public ScriptableFloatReference nextAbilityCooldownVariance = default;


        [Header("Status constants")]
        [Tooltip("Time in seconds of duration for speed buffs and debuffs")]
        public ScriptableFloatReference speedModifierDuration = default;
        [Tooltip("Time in seconds of duration for damage buffs and debuffs")]
        public ScriptableFloatReference damageModifierDuration = default;
        [Tooltip("Time in seconds of duration for damage over time effects")]
        public ScriptableFloatReference damageOverTimeDuration = default;
        [Tooltip("Time in seconds between damages in damage over time effects")]
        public ScriptableFloatReference damageOverTimeTickDelay = default;
        [Tooltip("Time in seconds for the effect of a shield to wear off")]
        public ScriptableFloatReference shieldDuration = default;
        [Tooltip("Time in seconds for the effect of blind to wear off")]
        public ScriptableFloatReference blindDuration = default;
        [Tooltip("Duration for parry preparation. During the duration, the enemy will parry attacks")]
        public ScriptableFloatReference parryPreparationTime = default;
        [Tooltip("Duration for shield preparation. During the duration, the enemy will block attacks")]
        public ScriptableFloatReference shieldPreparationTime = default;
        [Tooltip("Time in seconds of duration for retaliation buff")]
        public ScriptableFloatReference retaliationBuffDuration = default;
        [Tooltip("Time in seconds of duration for barrier buff")]
        public ScriptableFloatReference barrierBuffDuration = default;
        [Tooltip("Time in seconds of duration for heal over time effects")]
        public ScriptableFloatReference healOverTimeDuration = default;
        [Tooltip("Time in seconds between heals in heal over time effects")]
        public ScriptableFloatReference healOverTimeTickDelay = default;

        [Header("Battle effect constants")]
        [Tooltip("Amount (percent of 1) of damage that Health Siphon returns to the attacker")]
        public ScriptableFloatReference siphonPercent = default;

        [Header("Effect constants")]
        [Tooltip("Max intensity for chromatic aberration when stopping time")]
        public ScriptableFloatReference chromaticAberrationMaxIntensity = default;

        [Header("Enemy map constants")]
        [Tooltip("Vertical raycast length")]
        public ScriptableFloatReference enemyMapVerticalRaycastLength = default;
        [Tooltip("Distance offset to check vertical raycast for turning or not")]
        public ScriptableFloatReference enemyMapVerticalRaycastOffset = default;
        [Tooltip("Horizontal raycast length")]
        public ScriptableFloatReference enemyMapHorizontalRaycastLength = default;
        [Tooltip("Horizontal raycast up offset")]
        public ScriptableFloatReference enemyMapHorizontalRaycastOffsetUp = default;
        [Tooltip("Distance between enemy party members")]
        public ScriptableFloatReference enemyMapPartyOffset = default;

        [Header("Camera constants")]
        [Tooltip("Affects the zoom on battles")]
        public ScriptableFloatReference battleGroupRadius = default;

        [Header("Pilgrim constants")]
        [Tooltip("Discount for minions buying at the pilgrim. (0~1)")]
        public ScriptableFloatReference minionDiscount = default;

        private void Awake()
        {
            GameConstantsBehaviour.Instance = this;
        }

        private void OnDestroy()
        {
            GameConstantsBehaviour.Instance = null;
        }
    }
}