using UnityEngine;

namespace Laresistance.Behaviours
{
    [RequireComponent(typeof(Collider2D))]
    public class BreakerObject : MonoBehaviour
    {
        [SerializeField]
        private string tagToCheck = default;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(tagToCheck))
            {
                IBreakable breakable = collision.gameObject.GetComponent<IBreakable>();
                if (breakable != null)
                {
                    breakable.Break();
                }
            }
        }
    }
}