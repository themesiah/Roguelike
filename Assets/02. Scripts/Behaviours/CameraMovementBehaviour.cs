using GamedevsToolbox.ScriptableArchitecture.Sets;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class CameraMovementBehaviour : MonoBehaviour
    {
        [SerializeField]
        private RuntimeSingleGameObject playerReference = default;
        [SerializeField]
        private float depth = 50f;
        [SerializeField]
        private Vector2 offset = default;
        [SerializeField]
        private LayerMask layerMask = default;
        [SerializeField] [Tooltip("X is horizontal limit, Y is bottom limit and Z is top limit")]
        private Vector3 limitsDistance = default;
        [SerializeField]
        private Vector2 minMaxCameraSpeed = default;
        [SerializeField]
        private Vector2 minMaxDistanceToCamera = default;

        private List<Vector3> hits = new List<Vector3>();

        private void Update()
        {
            Vector3 targetPosition = GetTargetPosition(playerReference.Get().transform.position);
            float relativeSpeed = Mathf.InverseLerp(minMaxDistanceToCamera.x, minMaxDistanceToCamera.y, Vector2.Distance(transform.position, targetPosition));
            float currentSpeed = Mathf.Lerp(minMaxCameraSpeed.x, minMaxCameraSpeed.y, relativeSpeed);
            Vector3 framePosition = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

            transform.position = framePosition;
        }

        private Vector3 GetTargetPosition(Vector3 playerPosition)
        {
            hits.Clear();
            var pos = transform.position;
            Vector3 targetPosition = playerPosition;
            
            targetPosition.x += offset.x;
            targetPosition.y += offset.y;
            targetPosition.z = depth;

            float xdis = Mathf.Abs(targetPosition.x - pos.x);
            float ydis = Mathf.Abs(targetPosition.y - pos.y);
            if (xdis <= minMaxDistanceToCamera.x)
            {
                targetPosition.x = pos.x;
            }
            if (ydis <= minMaxDistanceToCamera.x)
            {
                targetPosition.y = pos.y;
            }

            var leftHit = Physics2D.Raycast(playerPosition, Vector2.left, limitsDistance.x, layerMask);
            var rightHit = Physics2D.Raycast(playerPosition, Vector2.right, limitsDistance.x, layerMask);
            var bottomHit = Physics2D.Raycast(playerPosition, Vector2.down, limitsDistance.y, layerMask);
            var topHit = Physics2D.Raycast(playerPosition, Vector2.up, limitsDistance.z, layerMask);

            if (leftHit.collider != null && rightHit.collider == null)
            {
                // Left hit case
                targetPosition.x = leftHit.point.x + limitsDistance.x;
                hits.Add(leftHit.point);
            }
            else if (leftHit.collider == null && rightHit.collider != null)
            {
                // Right hit case
                targetPosition.x = rightHit.point.x - limitsDistance.x;
                hits.Add(rightHit.point);
            }

            if (bottomHit.collider != null && topHit.collider == null)
            {
                // Bottom hit case
                targetPosition.y = bottomHit.point.y + limitsDistance.y;
                hits.Add(bottomHit.point);
            }
            else if (bottomHit.collider == null && topHit.collider != null)
            {
                // Top hit case
                targetPosition.y = topHit.point.y - limitsDistance.z;
                hits.Add(topHit.point);
            }
            return targetPosition;
        }

        public void InstantUpdate(Vector3 position)
        {
            Vector3 targetPosition = GetTargetPosition(position);
            transform.position = targetPosition;
        }

        private void OnDrawGizmos()
        {
            if (playerReference == null || playerReference.Get() == null)
                return;
            Transform playerTransform = playerReference.Get().transform;
            Vector3 targetPosition = playerTransform.position;
            targetPosition.x += offset.x;
            targetPosition.y += offset.y;
            targetPosition.z = 10f;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(targetPosition, targetPosition + Vector3.up * limitsDistance.z); // Top
            Gizmos.DrawLine(targetPosition, targetPosition + Vector3.down * limitsDistance.y); // Bottom
            Gizmos.DrawLine(targetPosition, targetPosition + Vector3.left * limitsDistance.x); // Left
            Gizmos.DrawLine(targetPosition, targetPosition + Vector3.right * limitsDistance.x); // Right
            foreach(var point in hits)
            {
                Gizmos.DrawSphere(point, 0.5f);
            }
        }
    }
}