using UnityEngine;
using Laresistance.LevelGeneration;
using Laresistance.Data;
using System.Collections.Generic;
using GamedevsToolbox.ScriptableArchitecture.Sets;

namespace Laresistance.Behaviours
{
    public class RoomConfiguration : MonoBehaviour
    {
        // How many more space than actual content is needed to consider the room too big for the requirements.
        // For example, assigning a 1 enemy room to a room with 6 enemy spots available may result in a boring, empty room.
        private static int TOO_MUCH_SPACE_THRESHOLD = 2;
        private static float ROOM_VALUE_PARTY_THRESHOLD = 0.7f;

        [Header("Configuration")]
        [SerializeField]
        private Transform[] possibleEnemySpawnPoints = default;
        [SerializeField]
        private Transform possibleMinibossSpawnPoint = default;
        [SerializeField]
        private Transform[] possibleInteractablePositions = default;
        [SerializeField]
        private RoomChangeBehaviour topRoomConnection = null;
        [SerializeField]
        private RoomChangeBehaviour bottomRoomConnection = null;
        [SerializeField]
        private RoomChangeBehaviour rightRoomConnection = null;
        [SerializeField]
        private RoomChangeBehaviour leftRoomConnection = null;
        [SerializeField]
        private bool hasMovementTest = default;

        [Header("Data")]
        [SerializeField]
        private RuntimeSetGameObject spawnableMinionList = default;
        [SerializeField]
        private GameObject[] interactableList = default;

        [Header("Temp and test")]
        public RoomBiome roomBiome;

        private void Start()
        {
            RoomData rd = new RoomData(0, null);
            rd.AddEnemy(new RoomEnemy() { roomEnemyType = RoomEnemyType.Enemy });
            rd.AddEnemy(new RoomEnemy() { roomEnemyType = RoomEnemyType.Minion });
            rd.AddInteractable(new RoomInteractable() { roomInteractableType = RoomInteractableType.BloodReward });
            rd.AddLink(new RoomLink() { linkedRoom = rd, linkedRoomIndex = 0, linkLocation = RoomLinkLocation.Right, linkType = RoomLinkType.Horizontal, minimalPath = true });
            rd.AddLink(new RoomLink() { linkedRoom = rd, linkedRoomIndex = 0, linkLocation = RoomLinkLocation.Left, linkType = RoomLinkType.Horizontal, minimalPath = true });
            rd.SetAsFirstRoom();
            rd.SetAsMinimalPathRoom();
            bool cool = CheckRoomRequirements(rd);
            if (cool)
            {
                ConfigureRoom(null, rd, roomBiome);
            } else
            {
                UnityEngine.Assertions.Assert.IsTrue(false);
            }
        }


        public bool CheckRoomRequirements(RoomData roomData, int thresholdReduction = 0)
        {
            // Check available links
            foreach(var link in roomData.GetLinks())
            {
                if (link.linkLocation == RoomLinkLocation.Top && topRoomConnection == null)
                {
                    return false;
                } else if (link.linkLocation == RoomLinkLocation.Bottom && bottomRoomConnection == null)
                {
                    return false;
                } else if (link.linkLocation == RoomLinkLocation.Right && rightRoomConnection == null)
                {
                    return false;
                } else if (link.linkLocation == RoomLinkLocation.Left && leftRoomConnection == null)
                {
                    return false;
                }
            }

            // Check enemy spots
            int nonMinibossEnemies = 0;
            foreach(var enemy in roomData.GetRoomEnemies())
            {
                if (enemy.roomEnemyType == RoomEnemyType.Miniboss)
                {
                    if (possibleMinibossSpawnPoint == null)
                    {
                        return false;
                    }
                } else
                {
                    nonMinibossEnemies++;
                }
            }
            if (nonMinibossEnemies > possibleEnemySpawnPoints.Length || possibleEnemySpawnPoints.Length - nonMinibossEnemies > TOO_MUCH_SPACE_THRESHOLD + thresholdReduction) // If there is not enough space or there are TOO MUCH space.
            {
                return false;
            }

            // Check interactable spots
            if (roomData.GetInteractables().Length > possibleInteractablePositions.Length || possibleInteractablePositions.Length - roomData.GetInteractables().Length > TOO_MUCH_SPACE_THRESHOLD + thresholdReduction)
            {
                return false;
            }

            // Check for movement test
            if (hasMovementTest != roomData.HaveMovementTest)
            {
                return false;
            }

            return true;
        }

        public void PreconfigureRoom(RoomData roomData)
        {
            PreconfigureLinks(roomData);
        }

        private void PreconfigureLinks(RoomData roomData)
        {
            for(int i = 0; i < roomData.GetLinks().Length; ++i)
            {
                var link = roomData.GetLinks()[i];
                switch(link.linkLocation)
                {
                    case RoomLinkLocation.Top:
                        link.roomChangeBehaviour = topRoomConnection;
                        break;
                    case RoomLinkLocation.Bottom:
                        link.roomChangeBehaviour = bottomRoomConnection;
                        break;
                    case RoomLinkLocation.Right:
                        link.roomChangeBehaviour = rightRoomConnection;
                        break;
                    case RoomLinkLocation.Left:
                        link.roomChangeBehaviour = leftRoomConnection;
                        break;
                }
                roomData.SetLink(link, i);
            }
        }

        public void ConfigureRoom(MapData mapData, RoomData roomData, RoomBiome biome)
        {
            // Configure links
            ConfigureLinks(mapData, roomData, biome);
            // Spawn enemies
            ConfigureEnemies(mapData, roomData, biome);
            // Spawn interactables
            ConfigureInteractables(roomData, biome);
        }

        private void ConfigureLinks(MapData mapData, RoomData roomData, RoomBiome biome)
        {
            if (mapData != null)
            {
                ConfigureLink(mapData, roomData, topRoomConnection, RoomLinkLocation.Top, RoomLinkLocation.Bottom);
                ConfigureLink(mapData, roomData, bottomRoomConnection, RoomLinkLocation.Bottom, RoomLinkLocation.Top);
                ConfigureLink(mapData, roomData, rightRoomConnection, RoomLinkLocation.Right, RoomLinkLocation.Left);
                ConfigureLink(mapData, roomData, leftRoomConnection, RoomLinkLocation.Left, RoomLinkLocation.Right);
            }
        }

        private void ConfigureLink(MapData mapData, RoomData roomData, RoomChangeBehaviour roomChangeBehaviour, RoomLinkLocation location, RoomLinkLocation connectionLocation)
        {
            if (roomChangeBehaviour != null)
            {
                int linkedRoomIndex = -1;
                foreach (var link in roomData.GetLinks())
                {
                    if (link.linkLocation == location)
                    {
                        linkedRoomIndex = link.linkedRoomIndex;
                    }
                }
                if (linkedRoomIndex != -1)
                {
                    foreach(var link in mapData.GetAllRooms()[linkedRoomIndex].GetLinks())
                    {
                        if (link.linkLocation == connectionLocation)
                        {
                            roomChangeBehaviour.SetNextRoom(link.roomChangeBehaviour);
                        }
                    }
                }
                else
                {
                    roomChangeBehaviour.ActivateAlternativeObjects();
                }
            }
        }

        private void ConfigureEnemies(MapData mapData, RoomData roomData, RoomBiome biome)
        {
            List<Transform> tempEnemySpawnPositions = new List<Transform>(possibleEnemySpawnPoints);
            float relativeValue = GetRoomRelativeValue(mapData, roomData);
            int levelOverride = GetRoomLevel(relativeValue, new Vector2Int(1, 10));
            foreach (var enemy in roomData.GetRoomEnemies())
            {
                GameObject go = null;
                switch (enemy.roomEnemyType)
                {
                    case RoomEnemyType.Miniboss:
                        go = Instantiate(biome.MinibossEnemies[Random.Range(0, biome.MinibossEnemies.Length)], possibleMinibossSpawnPoint);
                        break;
                    case RoomEnemyType.Enemy:
                        Transform randomPosition = tempEnemySpawnPositions[Random.Range(0, tempEnemySpawnPositions.Count)];
                        go = Instantiate(biome.NormalEnemies[Random.Range(0, biome.NormalEnemies.Length)], randomPosition);
                        tempEnemySpawnPositions.Remove(randomPosition);
                        break;
                    case RoomEnemyType.Minion:
                        Transform randomPositionMinion = tempEnemySpawnPositions[Random.Range(0, tempEnemySpawnPositions.Count)];
                        go = Instantiate(spawnableMinionList.Items[Random.Range(0, spawnableMinionList.Items.Count)], randomPositionMinion);
                        tempEnemySpawnPositions.Remove(randomPositionMinion);
                        break;
                }
                UnityEngine.Assertions.Assert.IsNotNull(go);
                go.transform.localPosition = Vector3.zero;
                go.GetComponent<EnemyBattleBehaviour>().InitEnemy(levelOverride);
                if (Random.Range(0, 1) == 0)
                {
                    var scale = go.transform.localScale;
                    scale.x = -1;
                    go.transform.localScale = scale;
                }
                // Depending on room value
                if (relativeValue <= ROOM_VALUE_PARTY_THRESHOLD)
                {
                    go.GetComponent<PartyManagerBehaviour>().enabled = false;
                }
            }
        }

        private void ConfigureInteractables(RoomData roomData, RoomBiome biome)
        {
            List<Transform> tempInteractablePositions = new List<Transform>(possibleInteractablePositions);
            foreach(var interactable in roomData.GetInteractables())
            {
                Transform randomPosition = tempInteractablePositions[Random.Range(0, tempInteractablePositions.Count)];
                GameObject go = Instantiate(interactableList[(int)interactable.roomInteractableType], randomPosition);
                tempInteractablePositions.Remove(randomPosition);
                go.transform.localPosition = Vector3.zero;
            }
        }

        private float GetRoomRelativeValue(MapData mapData, RoomData roomData)
        {
            if (mapData == null)
            {
                return 0f;
            }
            RoomData minimalPathRoom = GetNearestMinimalPathRoom(mapData, roomData);
            int index = 0;
            var minimalPath = mapData.GetMinimalPath();
            RoomData current = minimalPath[index];

            while (current != minimalPathRoom)
            {
                index++;
                current = minimalPath[index];
            }
            float relativeValue = (float)index / (float)minimalPath.Length;
            return relativeValue;
        }

        private int GetRoomLevel(float relativeValue, Vector2Int minMax)
        {
            float flevel;
            if (relativeValue <= ROOM_VALUE_PARTY_THRESHOLD)
            {
                float val = Mathf.InverseLerp(0f, ROOM_VALUE_PARTY_THRESHOLD, relativeValue);
                flevel = Mathf.Lerp(minMax.x, minMax.y, val);
            } else
            {
                float val = Mathf.InverseLerp(ROOM_VALUE_PARTY_THRESHOLD, 1f, relativeValue);
                flevel = Mathf.Lerp(minMax.x, minMax.y, val);
            }
            return Mathf.RoundToInt(flevel);
        }

        private RoomData GetNearestMinimalPathRoom(MapData mapData, RoomData roomData)
        {
            if (roomData.IsMinimalPathRoom)
            {
                return roomData;
            }
            RoomData nearest = roomData;
            int nearestDistance = -1;
            XYPair currentPosition = MapGenerationUtils.IndexToCoordinates(roomData.RoomIndex, mapData.GetMapSize().x);
            foreach(var room in mapData.GetMinimalPath())
            {
                XYPair gridPosition = MapGenerationUtils.IndexToCoordinates(room.RoomIndex, mapData.GetMapSize().x);
                int distance = System.Math.Abs(gridPosition.x - currentPosition.x) + System.Math.Abs(gridPosition.y - currentPosition.y);
                if (nearestDistance == -1 || distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = room;
                }
            }
            return nearest;
        }
    }
}