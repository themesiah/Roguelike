using UnityEngine;
using System.Collections;

namespace GamedevsToolbox.Utils
{
    public class TimedDestroy : MonoBehaviour
    {
        public void DoDestroy(float time)
        {
            Destroy(gameObject, time);
        }

        public void DoDeactivate(float time)
        {
            StartCoroutine(DeactivateTimer(time));
        }

        private IEnumerator DeactivateTimer(float time)
        {
            yield return new WaitForSeconds(time);
            gameObject.SetActive(false);
        }
    }
}