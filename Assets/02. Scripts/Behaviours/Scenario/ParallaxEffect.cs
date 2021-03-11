using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class ParallaxEffect : MonoBehaviour
    {
        [SerializeField]
        private Transform[] parallaxCorners = default;

        [SerializeField]
        private Transform[] playerLimits = default;

        [SerializeField]
        private GamedevsToolbox.ScriptableArchitecture.Sets.RuntimeSingleCamera cameraRef = default;

        private Transform playerTransform;

        private void Start()
        {
            playerTransform = cameraRef.Get().transform;
            UnityEngine.Assertions.Assert.AreEqual(4, parallaxCorners.Length);
            UnityEngine.Assertions.Assert.AreEqual(4, playerLimits.Length);
        }

        private void Update()
        {
            float valx = Mathf.InverseLerp(playerLimits[0].position.x, playerLimits[1].position.x, playerTransform.position.x);
            float valy = Mathf.InverseLerp(playerLimits[2].position.y, playerLimits[3].position.y, playerTransform.position.y);
            float posx = Mathf.Lerp(parallaxCorners[0].position.x, parallaxCorners[1].position.x, valx);
            float posy = Mathf.Lerp(parallaxCorners[2].position.y, parallaxCorners[3].position.y, valy);
            Vector3 pos = transform.position;
            pos.x = posx;
            pos.y = posy;
            transform.position = pos;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            foreach(var corner in parallaxCorners)
            {
                Gizmos.DrawSphere(corner.position, 0.3f);
            }
            Gizmos.color = Color.blue;
            foreach(var corner in playerLimits)
            {
                Gizmos.DrawSphere(corner.position, 0.3f);
            }
        }
    }
}