using GamedevsToolbox.ScriptableArchitecture.Events;
using GamedevsToolbox.ScriptableArchitecture.Values;
using GamedevsToolbox.ScriptableArchitecture.Sets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Laresistance.Data;

namespace Laresistance.Behaviours
{
    public class GoldenQueenFleeBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float percentDifferenceToFlee = 0.1f;
        [SerializeField]
        private MapBehaviour bossMapBehaviour = default;
        [SerializeField]
        private Collider2D[] bossColliders = default;
        [SerializeField]
        private StringGameEvent contextSignal = default;
        [SerializeField]
        private RuntimeBossBarrierSet bossBarriers = default;
        [SerializeField]
        private EnemyMapBehaviour mapBehaviuour = default;
        [SerializeField]
        private PartyManagerBehaviour partyManager = default;

        [SerializeField]
        private ScriptableFloatReference disappearSpeed = default;
        [SerializeField]
        private ScriptableFloatReference disappearDistance = default;
        [SerializeField]
        private RuntimeSingleGameObject[] floorLocations = default;

        private float lastPercentFled = 1f;

        private int currentFloor = 2;
        private List<int> availableFloors;

        private void Awake()
        {
            availableFloors = new List<int>();
        }

        public void HealthPercentChanged(float percent)
        {
            if (percent > 0f)
            {
                if (percent > lastPercentFled)
                {
                    lastPercentFled = percent;
                }
                else if (lastPercentFled - percent >= percentDifferenceToFlee)
                {
                    StartFlee();
                    lastPercentFled = percent;
                }
            }
        }

        // 0 is both closed. 1 is right open, 2 left open, 3 both open
        private int OpenWalls(int floor)
        {
            int barrier1 = -1;
            int barrier2 = -1;
            bool barrier1active = false;
            bool barrier2active = false;
            switch (floor)
            {
                case 1:
                    barrier1 = 1;
                    barrier2 = 2;
                    break;
                case 2:
                    barrier1 = 3;
                    barrier2 = 4;
                    break;
                case 3:
                    barrier1 = 5;
                    barrier2 = 6;
                    break;
            }
            foreach (var barrier in bossBarriers.Items)
            {
                if (barrier.GetBarrierNumber() == barrier1)
                {
                    barrier1active = barrier.gameObject.activeSelf;
                } else if (barrier.GetBarrierNumber() == barrier2)
                {
                    barrier2active = barrier.gameObject.activeSelf;
                }
            }

            if(barrier1active && barrier2active)
            {
                return 0;
            } else if (barrier1active && !barrier2active)
            {
                return 1;
            } else  if (!barrier1active && barrier2active)
            {
                return 2;
            } else
            {
                return 3;
            }
        }

        public void DebugFloor(int floor)
        {
            Debug.Log(OpenWalls(floor));
        }

        private void StartFlee()
        {
            // Check if both walls are blocking. If so, finish here.
            if (OpenWalls(currentFloor) == 0)
            {
                return;
            }
            // Select floor (1, 2 or 3). Check which floors (not the current) have at least a wall open and select one of them. If there are not, stay in current floor, don't flee.
            int floor = currentFloor;
            availableFloors.Clear();
            for (int i = 1; i <= 3; i++)
            {
                if (i != currentFloor && OpenWalls(i) != 0)
                {
                    availableFloors.Add(i);
                }
            }
            if (availableFloors.Count == 0)
            {
                return;
            } else
            {
                floor = availableFloors[Random.Range(0, availableFloors.Count)];
            }
            Debug.LogFormat("Floor {0} selected", floor);
            // Signal finish battle
            contextSignal.Raise("RemoveFirstEnemy");
            // Deactivate colliders
            foreach (Collider2D col in bossColliders)
            {
                col.enabled = false;
            }
            // Deactivate map behaviour
            bossMapBehaviour.PauseMapBehaviour();
            // Select direction (from the left or from the right, depending on which walls are blocked)
            bool toRight = true;
            int openWallsTo = OpenWalls(floor);
            UnityEngine.Assertions.Assert.AreNotEqual(0, openWallsTo);
            if (openWallsTo == 2)
            {
                toRight = false;
            } else if (openWallsTo == 3)
            {
                toRight = Random.Range(0, 2) == 0 ? true : false;
            }
            bool fromRight = true;
            int openWallsFrom = OpenWalls(currentFloor);
            UnityEngine.Assertions.Assert.AreNotEqual(0, openWallsFrom);
            if (openWallsFrom == 2)
            {
                fromRight = false;
            } else if (openWallsFrom == 3)
            {
                fromRight = Random.Range(0, 2) == 0 ? true : false;
            }
            StartCoroutine(Flee(floor, fromRight, toRight));
        }

        private IEnumerator Flee(int floor, bool fromRight, bool toRight)
        {
            Debug.Log("Starting flee");
            // Disappear from screem and appear again in the selected place
            yield return Disappear(fromRight);
            yield return Appear(toRight, floor);
            
            // End flee
            EndFlee();
        }

        private IEnumerator Disappear(bool right)
        {
            mapBehaviuour.Turn(right);
            float disappearDistanceWithSignFrom = disappearDistance.GetValue();
            if (!right)
            {
                disappearDistanceWithSignFrom = -disappearDistanceWithSignFrom;
            }

            Vector3 targetPosition = transform.position;
            targetPosition.x = floorLocations[currentFloor - 1].Get().transform.position.x + disappearDistanceWithSignFrom;
            while (!Mathf.Approximately(transform.position.x, targetPosition.x))
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, disappearSpeed.GetValue() * Time.deltaTime);
                yield return null;
            }
        }

        private IEnumerator Appear(bool right, int floor)
        {
            if (right)
            {
                Debug.Log("Appear from right");
            } else
            {
                Debug.Log("Appear from left");
            }
            float disappearDistanceWithSignFrom = disappearDistance.GetValue();
            if (!right)
            {
                disappearDistanceWithSignFrom = -disappearDistanceWithSignFrom;
            }
            mapBehaviuour.Turn(!right);
            Vector3 pos = transform.position;
            pos.y = floorLocations[floor - 1].Get().transform.position.y;
            pos.x = floorLocations[floor - 1].Get().transform.position.x + disappearDistanceWithSignFrom;
            transform.position = pos;
            Vector3 targetPosition = transform.position;
            targetPosition.x = floorLocations[floor - 1].Get().transform.position.x;
            while (!Mathf.Approximately(transform.position.x, targetPosition.x))
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, disappearSpeed.GetValue() * Time.deltaTime);
                yield return null;
            }
            currentFloor = floor;
        }

        private void EndFlee()
        {
            // Activate Colliders
            foreach (Collider2D col in bossColliders)
            {
                col.enabled = true;
            }
            // Activate map behaviour
            bossMapBehaviour.ResumeMapBehaviour();
            partyManager.SpawnParty();
        }
    }
}