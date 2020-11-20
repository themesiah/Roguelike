using UnityEngine;

namespace Laresistance.Behaviours
{
    public class BossBarrier : MonoBehaviour
    {
        [SerializeField]
        private int selfNumber = -1;
        [SerializeField]
        private bool allowReset = false;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public int GetBarrierNumber()
        {
            return selfNumber;
        }

        public void BarrierTrigger(int barrierNumber)
        {
            if (barrierNumber == selfNumber)
            {
                gameObject.SetActive(!gameObject.activeSelf);
            } else if (barrierNumber == -1 && allowReset)
            {
                gameObject.SetActive(false);
            }
        }
    }
}