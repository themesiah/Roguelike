using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Laresistance.Behaviours
{
    public class ChandelierAttackSpawner : MonoBehaviour, IRangedAttackSpawner
    {
        [SerializeField]
        private AnimatorWrapperBehaviour animatorWrapper = default;
        [SerializeField]
        private AssetReference firePreludeReference = default;
        [SerializeField]
        private AssetReference fireHazardReference = default;
        [SerializeField] [Tooltip("So the hazard appears on a surface like the floor or a platform")]
        private LayerMask hazardSurfaceMask = default;

        public IEnumerator SpawnRangedAttack(Transform targetPosition)
        {
            var hit = Physics2D.Raycast(targetPosition.position + Vector3.up * 0.5f, Vector2.down, 500f, hazardSurfaceMask);
            if (hit.collider != null)
            {
                var op1 = firePreludeReference.InstantiateAsync(null, true);
                op1.Completed += (handle) =>
                {
                    GameObject obj = handle.Result;
                    obj.transform.position = hit.point;
                };
                yield return op1;

                yield return new WaitForSeconds(0.5f);
                yield return animatorWrapper.PlayAnimation("MapAttack");

                var op2 = fireHazardReference.InstantiateAsync(null, true);
                op2.Completed += (handle) =>
                {
                    GameObject obj = handle.Result;
                    obj.transform.position = hit.point;
                };
                yield return op2;
            }
        }
    }
}