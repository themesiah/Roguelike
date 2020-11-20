using Laresistance.Battle;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class HealthRegeneration : MonoBehaviour
    {
        private static float TICK_TIME = 1f;

        [SerializeField]
        private int healPerTick = default;
        [SerializeField]
        private CharacterBattleBehaviour battleBehaviourRef = default;


        private CharacterHealth characterHealth;
        private float timer = 0f;

        private void Start()
        {
            characterHealth = battleBehaviourRef.StatusManager.health;
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= TICK_TIME)
            {
                timer = timer - TICK_TIME;
                characterHealth.Heal(healPerTick);
            }
        }
    }
}