using UnityEngine;

namespace Laresistance.Behaviours
{
    public class Flipper : MonoBehaviour
    {
        [SerializeField]
        private bool originallyRight = true;

        private Vector3 originalScale;
        private bool initialized = false;
        private void Awake()
        {
            originalScale = transform.localScale;
            UnityEngine.Assertions.Assert.AreNotEqual(originalScale.x, 0f);
            initialized = true;
        }

        public void Flip(bool right)
        {
            if (!initialized)
                return;
            float direction = 1f;
            if (!right)
            {
                direction *= -1f;
            }
            if (originallyRight)
            {
                direction *= -1f;
            }
            var scale = transform.localScale;
            scale.x = originalScale.x * direction;
            transform.localScale = scale;
        }
    }
}