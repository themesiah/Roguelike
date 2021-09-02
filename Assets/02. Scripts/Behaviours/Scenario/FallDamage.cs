using GamedevsToolbox.ScriptableArchitecture.Values;
using GamedevsToolbox.ScriptableArchitecture.Events;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class FallDamage : MonoBehaviour
    {
        [SerializeField]
        private Transform teleportPoint = default;
        [SerializeField]
        private ScriptableIntReference fallDamageReference = default;
        [SerializeField]
        private IntGameEvent outsideBattleDamageEvent = default;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Check player
            if (collision.gameObject.CompareTag("Player"))
            {
                // Teleport first
                collision.transform.position = teleportPoint.position;
                // Deal damage
                outsideBattleDamageEvent?.Raise(fallDamageReference.GetValue());
            }
        }
    }
}