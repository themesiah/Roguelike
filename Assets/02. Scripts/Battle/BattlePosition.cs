﻿using Laresistance.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BattlePosition
    {
        private static float CHARACTERS_HORIZONTAL_OFFSET = 3f;
        private static float PARTY_HORIZONTAL_OFFSET = 0.4f;
        private static float PARTY_VERTICAL_OFFSET = 0.4f;

        private static float DISTANCE_BETWEEN_VERTICAL_RAYCAST = 0.1f;
        private static float MIN_DISTANCE_FOR_SETUP = 12f;
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

        private static float GetDistanceTo(Vector3 pointCheck, float modifier, int centerCheckLayerMask)
        {
            float distance = 0f;
            ContactFilter2D raycastFilters = new ContactFilter2D();
            raycastFilters.layerMask = centerCheckLayerMask;
            raycastFilters.useTriggers = false;
            raycastFilters.useLayerMask = true;
            List<RaycastHit2D> results = new List<RaycastHit2D>();

            // wall
            int hits = Physics2D.Raycast(pointCheck + Vector3.up * 0.1f, Vector2.right * modifier, raycastFilters, results);
            if (hits > 0)
            {
                distance = results[0].distance;
            } else
            {
                UnityEngine.Assertions.Assert.IsTrue(false, "Where the hell is the wall?");
            }


            // fall
            hits = Physics2D.Raycast(pointCheck + Vector3.up * 0.1f, Vector2.down, raycastFilters, results);
            UnityEngine.Assertions.Assert.IsTrue(hits > 0);
            float height = results[0].distance;

            hits = 1;
            int i = 1;
            while (hits > 0 && distance > DISTANCE_BETWEEN_VERTICAL_RAYCAST * i)
            {
                hits = Physics2D.Raycast(pointCheck + Vector3.up * 0.1f + Vector3.right * modifier * DISTANCE_BETWEEN_VERTICAL_RAYCAST*i, Vector2.down, raycastFilters, results, height+0.2f);
                if (hits == 0)
                {
                    UnityEngine.Assertions.Assert.IsTrue(distance > DISTANCE_BETWEEN_VERTICAL_RAYCAST*i);
                    distance = DISTANCE_BETWEEN_VERTICAL_RAYCAST * i;
                    break;
                }
                i++;
                if (i > 500)
                {
                    break;
                }
            }

            return distance;
        }

        public static void MoveCharacters(GameObject playerObject, GameObject[] enemyObjects, int centerCheckLayerMask)
        {
            bool playerLookingRight = false;
            float modifier = -1f;
            if (playerObject.transform.position.x < enemyObjects[0].transform.position.x)
            {
                playerLookingRight = true;
                modifier = 1f;
            }
            Turn(playerObject, playerLookingRight);
            Turn(enemyObjects[0], !playerLookingRight);

            Vector3 center = enemyObjects[0].transform.position;
            float distanceToLeft = GetDistanceToLeft(center, centerCheckLayerMask);
            float distanceToRight = GetDistanceToRight(center, centerCheckLayerMask);
            UnityEngine.Assertions.Assert.IsTrue(distanceToLeft + distanceToRight >= MIN_DISTANCE_FOR_SETUP, string.Format("No space to battle. Available space is {0} and {1} is required", distanceToRight+distanceToLeft, MIN_DISTANCE_FOR_SETUP));
            float offset = 0f;
            if (distanceToLeft < MIN_HALF_DISTANCE_FOR_SETUP)
            {
                offset = MIN_HALF_DISTANCE_FOR_SETUP - distanceToLeft;
            } else if (distanceToRight < MIN_HALF_DISTANCE_FOR_SETUP)
            {
                offset = -MIN_HALF_DISTANCE_FOR_SETUP - distanceToRight;
            }

            center = center + Vector3.right * offset;
            playerObject.transform.position = center - Vector3.right * CHARACTERS_HORIZONTAL_OFFSET * modifier;
            enemyObjects[0].transform.position = center + Vector3.right * CHARACTERS_HORIZONTAL_OFFSET * modifier;
            for (int i = 1; i < enemyObjects.Length; ++i)
            {
                enemyObjects[i].transform.position = enemyObjects[0].transform.position + Vector3.left * PARTY_HORIZONTAL_OFFSET * modifier * i + Vector3.up * PARTY_VERTICAL_OFFSET * i;
                Turn(enemyObjects[i], !playerLookingRight);
            }
        }

        private static void Turn(GameObject character, bool right)
        {
            MapBehaviour mb = character.GetComponent<MapBehaviour>();
            if (mb != null)
            {
                mb.GetMovementManager().Turn(right);
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