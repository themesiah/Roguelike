using UnityEngine;
using System.Collections;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Behaviours.Platforms
{
    [RequireComponent(typeof(PlatformEffector2D))]
    public class OneWayPlatform : MonoBehaviour
    {
        [SerializeField]
        private ScriptableFloatReference statusReturnTime = default;

        [SerializeField]
        private float targetAngle = 180f;

        [SerializeField]
        private LayerMask changedLayerMask = default;
        private LayerMask originalLayerMask;

        private PlatformEffector2D platformEffector;
        private float startingAngle;

        
        private void Awake()
        {
            platformEffector = GetComponent<PlatformEffector2D>();
            startingAngle = platformEffector.rotationalOffset;
            originalLayerMask = gameObject.layer;
            if (platformEffector == null)
            {
                Debug.LogErrorFormat(gameObject, "Not assigned platform effector in object {0}", name);
            }
        }

        public void ActivateFallingPlatform()
        {
            try
            {
                platformEffector.rotationalOffset = targetAngle;
                StartCoroutine(ReturnToStartingStatus());
                gameObject.layer = changedLayerMask;
            } catch(System.Exception e)
            {
                Debug.LogErrorFormat(gameObject, e.Message);
                Debug.LogErrorFormat(gameObject, "Not assigned platform effector in object {0} with parent {1}", name, transform.parent.name);
            }
        }

        private IEnumerator ReturnToStartingStatus()
        {
            yield return new WaitForSeconds(statusReturnTime.GetValue());
            platformEffector.rotationalOffset = startingAngle;
            gameObject.layer = originalLayerMask;
        }
    }
}