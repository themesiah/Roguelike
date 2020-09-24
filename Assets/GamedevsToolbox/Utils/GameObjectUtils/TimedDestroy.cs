using UnityEngine;

namespace GamedevsToolbox.Utils
{
    public class TimedDestroy : MonoBehaviour
    {
        public void DoDestroy(float time)
        {
            Destroy(gameObject, time);
        }
    }
}