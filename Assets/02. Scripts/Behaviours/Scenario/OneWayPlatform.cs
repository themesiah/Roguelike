using UnityEngine;
using System.Collections;

namespace Laresistance.Behaviours.Platforms
{
    [RequireComponent(typeof(PlatformEffector2D))]
    public class OneWayPlatform : MonoBehaviour
    {
        private static float STATUS_RETURN_TIME = 0.5f;

        private PlatformEffector2D platformEffector;

        [SerializeField]
        private float targetAngle = 180f;
        private float startingAngle;

        
        private void Awake()
        {
            platformEffector = GetComponent<PlatformEffector2D>();
            startingAngle = platformEffector.rotationalOffset;
        }

        public void ActivateFallingPlatform()
        {
            platformEffector.rotationalOffset = targetAngle;
            StartCoroutine(ReturnToStartingStatus());
        }

        private IEnumerator ReturnToStartingStatus()
        {
            yield return new WaitForSeconds(STATUS_RETURN_TIME);
            platformEffector.rotationalOffset = startingAngle;
        }
    }
}