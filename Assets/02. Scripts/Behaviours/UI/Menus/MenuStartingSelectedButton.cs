using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Laresistance.Behaviours
{
    public class MenuStartingSelectedButton : MonoBehaviour
    {
        [SerializeField]
        private Selectable startingButton = default;

        [SerializeField]
        private Selectable otherButton = default;

        private void OnEnable()
        {
            StartCoroutine(SelectFirst());
        }

        private IEnumerator SelectFirst()
        {
            yield return null;
            otherButton.Select();
            startingButton.Select();
        }
    }
}