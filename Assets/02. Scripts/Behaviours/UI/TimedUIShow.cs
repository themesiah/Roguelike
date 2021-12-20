using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class TimedUIShow : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] objectsToShow = default;

        [SerializeField]
        private float showTime = 0.5f;

        public void Show()
        {
            foreach(var obj in objectsToShow)
            {
                obj.SetActive(true);
            }
            StartCoroutine(Hide());
        }

        private IEnumerator Hide()
        {
            yield return new WaitForSeconds(showTime);
            foreach (var obj in objectsToShow)
            {
                obj.SetActive(false);
            }
        }
    }
}