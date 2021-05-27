using UnityEngine;

namespace Laresistance.Behaviours
{
    public class NonReferencedFlipper : MonoBehaviour
    {
        private Flipper[] flippers = null;

        public void Flip(bool right)
        {
            //if (flippers == null)
            {
                flippers = GetComponentsInChildren<Flipper>();
            }
            foreach(Flipper flipper in flippers)
            {
                flipper?.Flip(right);
            }
        }
    }
}