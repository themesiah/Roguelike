using Laresistance.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BattlePosition
    {
        public struct BattlePositionData
        {
            public bool direction;
            public Vector3 center;
        }

        private static float CHARACTERS_HORIZONTAL_OFFSET = 3f;
        private static float PARTY_HORIZONTAL_OFFSET = -2.5f;
        private static float PARTY_VERTICAL_OFFSET = 0.0f;

        private static float DISTANCE_BETWEEN_VERTICAL_RAYCAST = 0.1f;
        private static float MIN_DISTANCE_FOR_SETUP = 14f;
        private static float MIN_HALF_DISTANCE_FOR_SETUP = MIN_DISTANCE_FOR_SETUP / 2f;

        public static bool CheckSpace(Vector3 pointCheck, int centerCheckLayerMask)
        {
            float distanceLeft = GetDistanceToLeft(pointCheck, centerCheckLayerMask);
            float distanceRight = GetDistanceToRight(pointCheck, centerCheckLayerMask);
            float total = distanceLeft + distanceRight;
            return total >= MIN_DISTANCE_FOR_SETUP;
        }

        private static float GetDistanceToLeft(Vector3 pointCheck, int centerCheckLayerMask)
        {
            return GetDistanceTo(pointCheck, -1f, centerCheckLayerMask);
        }

        private static float GetDistanceToRight(Vector3 pointCheck, int centerCheckLayerMask)
        {
            return GetDistanceTo(pointCheck, 1f, centerCheckLayerMask);
        }

        private static float GetDistanceTo(Vector3 pointCheck, float direction, int centerCheckLayerMask)
        {
            UnityEngine.Assertions.Assert.AreNotEqual(0f, direction, "Why is direction 0?");
            float distance = 0f;
            ContactFilter2D raycastFilters = new ContactFilter2D();
            raycastFilters.layerMask = centerCheckLayerMask;
            raycastFilters.useTriggers = false;
            raycastFilters.useLayerMask = true;
            List<RaycastHit2D> results = new List<RaycastHit2D>();

            // wall
            int hits = Physics2D.Raycast(pointCheck + Vector3.up * 0.1f, Vector2.right * direction, raycastFilters, results);
            UnityEngine.Assertions.Assert.AreNotEqual(0, hits, string.Format("Where the hell is the wall in the {0} direction?", direction > 0f ? "right" : "left"));
            distance = results[0].distance;


            // fall
            float upOffset = 0.1f;
            Debug.DrawLine(pointCheck + Vector3.down * 0.2f + Vector3.left * 0.2f + Vector3.up * upOffset, pointCheck + Vector3.up * 0.2f + Vector3.right * 0.2f + Vector3.up * upOffset);
            Debug.DrawLine(pointCheck + Vector3.down * 0.2f + Vector3.right * 0.2f + Vector3.up * upOffset, pointCheck + Vector3.up * 0.2f + Vector3.left * 0.2f + Vector3.up * upOffset);
            hits = Physics2D.Raycast(pointCheck + Vector3.up * upOffset, Vector2.down, raycastFilters, results);
            UnityEngine.Assertions.Assert.AreNotEqual(0, hits, "Where the hell is the floor?");
            float height = results[0].distance+0.1f;

            //hits = 1;
            int i = 1;
            while (hits > 0 && distance > DISTANCE_BETWEEN_VERTICAL_RAYCAST * i)
            {
                hits = Physics2D.Raycast(pointCheck + Vector3.up * 0.1f + Vector3.right * direction * DISTANCE_BETWEEN_VERTICAL_RAYCAST*i, Vector2.down, raycastFilters, results, height+0.2f);
                if (hits == 0)
                {
                    UnityEngine.Assertions.Assert.IsTrue(distance > DISTANCE_BETWEEN_VERTICAL_RAYCAST*i);
                    distance = DISTANCE_BETWEEN_VERTICAL_RAYCAST * i;
                    break;
                }
                i++;
                if (i > 500)
                {
                    Debug.LogWarning("Something did not work as intended on BattlePosition. Too many iterations.");
                    break;
                }
            }

            return distance;
        }

        private static Vector2 GetRaycastedCenter(Vector2 nonRaycastedCenter, int centerCheckLayerMask)
        {
            List<RaycastHit2D> results = new List<RaycastHit2D>();
            ContactFilter2D raycastFilters = new ContactFilter2D();
            raycastFilters.layerMask = centerCheckLayerMask;
            raycastFilters.useTriggers = false;
            raycastFilters.useLayerMask = true;
            int intRes = Physics2D.Raycast(nonRaycastedCenter, Vector2.down, raycastFilters, results);
            if (intRes > 0)
            {
                return results[0].point;
            }
            else
            {
                return nonRaycastedCenter;
            }
        }

        public static BattlePositionData MoveCharacters(GameObject playerObject, GameObject[] enemyObjects, int centerCheckLayerMask)
        {
            BattlePositionData bpd = new BattlePositionData();
            bool playerLookingRight = false;
            float direction = -1f;
            if (playerObject.transform.position.x < enemyObjects[0].transform.position.x)
            {
                playerLookingRight = true;
                direction = 1f;
            }
            Turn(playerObject, playerLookingRight);
            Turn(enemyObjects[0], !playerLookingRight);

            Vector3 center = enemyObjects[0].transform.position;
            center = GetRaycastedCenter(center, centerCheckLayerMask);
            float distanceToLeft = GetDistanceToLeft(center, centerCheckLayerMask);
            float distanceToRight = GetDistanceToRight(center, centerCheckLayerMask);
            UnityEngine.Assertions.Assert.IsTrue(distanceToLeft + distanceToRight >= MIN_DISTANCE_FOR_SETUP, string.Format("No space to battle. Available space is {0} and {1} is required", distanceToRight+distanceToLeft, MIN_DISTANCE_FOR_SETUP));
            float offset = 0f;
            if (distanceToLeft < MIN_HALF_DISTANCE_FOR_SETUP)
            {
                offset = MIN_HALF_DISTANCE_FOR_SETUP - distanceToLeft;
            } else if (distanceToRight < MIN_HALF_DISTANCE_FOR_SETUP)
            {
                offset = -(MIN_HALF_DISTANCE_FOR_SETUP - distanceToRight);
            }

            center = center + Vector3.right * offset;
            playerObject.transform.position = center + Vector3.up * 0.1f - Vector3.right * CHARACTERS_HORIZONTAL_OFFSET * direction;
            enemyObjects[0].transform.position = center + Vector3.right * CHARACTERS_HORIZONTAL_OFFSET * direction;
            for (int i = 1; i < enemyObjects.Length; ++i)
            {
                enemyObjects[i].transform.position = enemyObjects[0].transform.position + Vector3.left * PARTY_HORIZONTAL_OFFSET * direction * i + Vector3.up * PARTY_VERTICAL_OFFSET * i;
                Turn(enemyObjects[i], !playerLookingRight);
            }
            bpd.direction = playerLookingRight;
            bpd.center = center;
            return bpd;
        }

        private static void Turn(GameObject character, bool right)
        {
            MapBehaviour mb = character.GetComponent<MapBehaviour>();
            if (mb != null)
            {
                mb.GetCharacterController().Flip(right);
            }
            else
            {
                Vector3 scale = character.transform.localScale;
                scale.x = Mathf.Abs(scale.x);
                if (!right)
                {
                    scale.x *= -1;
                }
                character.transform.localScale = scale;
            }
        }
    }
}