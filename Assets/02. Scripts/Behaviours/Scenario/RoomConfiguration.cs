using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Laresistance.LevelGeneration;
using Laresistance.Data;
using System.Collections.Generic;
using System.Collections;
using GamedevsToolbox.ScriptableArchitecture.Sets;
using GamedevsToolbox.ScriptableArchitecture.Events;
using UnityEngine.AddressableAssets;

namespace Laresistance.Behaviours
{
    [DefaultExecutionOrder(100)]
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
        [SerializeField]
        private Transform levelStartPosition = default;
        [SerializeField]
        private RoomChangeBehaviour levelEnd = default;
        [SerializeField]
        private CompositeCollider2D roomLimits = default;

        [Header("Data")]
        [SerializeField]
        private RuntimeSetAssetReference spawnableMinionList = default;
        [SerializeField]
        private AssetReference[] interactableList = default;

        [Header("References")]
        [SerializeField]
        private RuntimePlayerDataBehaviourSingle playerDataRef = default;
        [SerializeField]
        private Collider2DGameEvent boundsChangeEvent = default;

        [Header("Temp and test")]
        public RoomBiome roomBiome;
        public bool mockTest;
        public bool playerStartPoint;
        public bool spawnMiniboss;
        public int mockIndex = 0;
        public List<RoomEnemyType> enemyTypesSpawn;
        public List<RoomInteractableType> interactableTypesSpawn;

        public Transform[] EnemySpawns => possibleEnemySpawnPoints;
        public Transform MinibossSpawn => possibleMinibossSpawnPoint;

        public LayerMask enemySpaceLayerMask;

        private IEnumerator Start()
        {
            if (mockTest)
            {
                yield return InitMock();
            }
        }

        private IEnumerator InitMock()
        {
            RoomData rd = new RoomData(mockIndex, null);
            for (int i = 0; i < possibleEnemySpawnPoints.Length; ++i)
            {
                RoomEnemyType enemyType = RoomEnemyType.Enemy;
                if (enemyTypesSpawn.Count > i)
                {
                    enemyType = enemyTypesSpawn[i];
                }
                else
                {
                    enemyType = (RoomEnemyType)Random.Range(0, (int)RoomEnemyType.Miniboss);
                }
                rd.AddEnemy(new RoomEnemy() { roomEnemyType = enemyType });
            }
            if (possibleMinibossSpawnPoint != null && spawnMiniboss)
            {
                rd.AddEnemy(new RoomEnemy() { roomEnemyType = RoomEnemyType.Miniboss });
            }
            for (int i = 0; i < possibleInteractablePositions.Length; ++i)
            {
                RoomInteractableType interactableType = RoomInteractableType.BloodReward;
                if (interactableTypesSpawn.Count > i)
                {
                    interactableType = interactableTypesSpawn[i];
                }
                else
                {
                    interactableType = (RoomInteractableType)Random.Range(0, (int)RoomInteractableType.LevelStart);
                    while (interactableType == RoomInteractableType.Lore)
                    {
                        interactableType = (RoomInteractableType)Random.Range(0, (int)RoomInteractableType.LevelStart);
                    }
                }
                rd.AddInteractable(new RoomInteractable() { roomInteractableType = interactableType });
            }
            if (bottomRoomConnection != null)
            {
                if (bottomRoomConnection.NextRoom == null)
                rd.AddLink(new RoomLink() { linkedRoom = rd, linkedRoomIndex = -1, linkLocation = RoomLinkLocation.Bottom, linkType = RoomLinkType.Stairs, minimalPath = false });
                else
                rd.AddLink(new RoomLink() { linkedRoom = rd, linkedRoomIndex = 0, linkLocation = RoomLinkLocation.Bottom, linkType = RoomLinkType.Stairs, minimalPath = false });
            }
            if (topRoomConnection != null)
            {
                if (topRoomConnection.NextRoom == null)
                rd.AddLink(new RoomLink() { linkedRoom = rd, linkedRoomIndex = -1, linkLocation = RoomLinkLocation.Top, linkType = RoomLinkType.Stairs, minimalPath = false });
                else
                rd.AddLink(new RoomLink() { linkedRoom = rd, linkedRoomIndex = 1, linkLocation = RoomLinkLocation.Top, linkType = RoomLinkType.Stairs, minimalPath = false });
            }
            if (leftRoomConnection != null)
            {
                if (leftRoomConnection.NextRoom == null)
                rd.AddLink(new RoomLink() { linkedRoom = rd, linkedRoomIndex = -1, linkLocation = RoomLinkLocation.Left, linkType = RoomLinkType.Horizontal, minimalPath = false });
                else
                rd.AddLink(new RoomLink() { linkedRoom = rd, linkedRoomIndex = 2, linkLocation = RoomLinkLocation.Left, linkType = RoomLinkType.Horizontal, minimalPath = false });
            }
            if (rightRoomConnection != null)
            {
                if (rightRoomConnection.NextRoom == null)
                rd.AddLink(new RoomLink() { linkedRoom = rd, linkedRoomIndex = -1, linkLocation = RoomLinkLocation.Right, linkType = RoomLinkType.Horizontal, minimalPath = false });
                else
                rd.AddLink(new RoomLink() { linkedRoom = rd, linkedRoomIndex = 3, linkLocation = RoomLinkLocation.Right, linkType = RoomLinkType.Horizontal, minimalPath = false });
            }
            rd.SetAsMinimalPathRoom();
            if (levelStartPosition != null)
            {
                rd.SetAsFirstRoom();
            }
            if (playerStartPoint)
            {
                Transform spawnPosition = null;
                if (levelStartPosition != null)
                {
                    spawnPosition = levelStartPosition;
                }
                else
                {
                    if (bottomRoomConnection != null)
                    {
                        spawnPosition = bottomRoomConnection.RoomEnterPoint;
                    }
                    else
                    if (topRoomConnection != null)
                    {
                        spawnPosition = topRoomConnection.RoomEnterPoint;
                    }
                    else
                    if (leftRoomConnection != null)
                    {
                        spawnPosition = leftRoomConnection.RoomEnterPoint;
                    }
                    else
                    if (rightRoomConnection != null)
                    {
                        spawnPosition = rightRoomConnection.RoomEnterPoint;
                    }
                }
                Transform player = null;
                var pbb = FindObjectOfType<PlayerBattleBehaviour>();
                if (pbb != null)
                {
                    player = pbb.gameObject.transform;
                }
                if (player != null)
                {
                    player.position = spawnPosition.position;
                }
            }
            yield return ConfigureRoom(null, rd, roomBiome);
        }


        public bool CheckRoomRequirements(RoomData roomData, int thresholdReduction = 0)
        {
            // Check available links
            if (roomData.IsFirstRoom && levelStartPosition == null)
            {
                return false;
            }
            if (roomData.IsLastRoom && levelEnd == null)
            {
                return false;
            }

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
                roomData.SetBounds(roomLimits);
            }
        }

        public IEnumerator ConfigureRoom(MapData mapData, RoomData roomData, RoomBiome biome)
        {
            // Configure level start
            ConfigureLevelStart(roomData);
            // Configure level end
            ConfigureLevelEnd(roomData);
            // Configure links
            ConfigureLinks(mapData, roomData, biome);
            // Spawn enemies
            yield return ConfigureEnemies(mapData, roomData, biome);
            // Spawn interactables
            yield return ConfigureInteractables(roomData, biome);

            if (playerStartPoint)
            {
                boundsChangeEvent?.Raise(roomLimits);
            }
        }

        private void ConfigureLevelStart(RoomData roomData)
        {
            if (roomData.IsFirstRoom && !mockTest)
            {
                playerDataRef.Get().transform.position = levelStartPosition.position;
                boundsChangeEvent?.Raise(roomLimits);
            }
        }

        private void ConfigureLevelEnd(RoomData roomData)
        {
            if (roomData.IsLastRoom)
            {
                levelEnd.SetAsLevelEnd();
            } else if (levelEnd != null)
            {
                levelEnd.ActivateAlternativeObjects();
            }
        }

        private void ConfigureLinks(MapData mapData, RoomData roomData, RoomBiome biome)
        {
            ConfigureLink(mapData, roomData, topRoomConnection, RoomLinkLocation.Top, RoomLinkLocation.Bottom);
            ConfigureLink(mapData, roomData, bottomRoomConnection, RoomLinkLocation.Bottom, RoomLinkLocation.Top);
            ConfigureLink(mapData, roomData, rightRoomConnection, RoomLinkLocation.Right, RoomLinkLocation.Left);
            ConfigureLink(mapData, roomData, leftRoomConnection, RoomLinkLocation.Left, RoomLinkLocation.Right);
        }

        private void ConfigureLink(MapData mapData, RoomData roomData, RoomChangeBehaviour roomChangeBehaviour, RoomLinkLocation location, RoomLinkLocation connectionLocation)
        {
            if (roomChangeBehaviour != null)
            {
                int linkedRoomIndex = -1;
                var links = roomData.GetLinks();
                foreach (var link in links)
                {
                    if (link.linkLocation == location)
                    {
                        linkedRoomIndex = link.linkedRoomIndex;
                    }
                }
                if (linkedRoomIndex != -1)
                {
                    if (mapData != null)
                    {
                        foreach (var link in mapData.GetAllRooms()[linkedRoomIndex].GetLinks())
                        {
                            if (link.linkLocation == connectionLocation)
                            {
                                roomChangeBehaviour.SetNextRoom(link.roomChangeBehaviour);
                                roomChangeBehaviour.SetRoomBounds(roomLimits);
                            }
                        }
                    } else if (mockTest)
                    {
                        foreach (var link in roomData.GetLinks())
                        {
                            //if (link.linkLocation == connectionLocation)
                            {
                                //roomChangeBehaviour.SetNextRoom(roomChangeBehaviour);
                                roomChangeBehaviour.SetRoomBounds(roomLimits);
                            }
                        }
                    }
                }
                else
                {
                    roomChangeBehaviour.ActivateAlternativeObjects();
                }
            }
        }

        private IEnumerator ConfigureEnemies(MapData mapData, RoomData roomData, RoomBiome biome)
        {
            List<Transform> tempEnemySpawnPositions = new List<Transform>(possibleEnemySpawnPoints);
            float relativeValue = GetRoomRelativeValue(mapData, roomData);
            int levelOverride = GetRoomLevel(relativeValue, new Vector2Int(1, 10));
            int index = 0;
            int finishedIndex = 0;
            foreach (var enemy in roomData.GetRoomEnemies())
            {
                Tools.SimpleTimeProfiler.Instance?.AddTime(string.Format("Enemy {0}", index));
                AsyncOperationHandle<GameObject> op = default;
                switch (enemy.roomEnemyType)
                {
                    case RoomEnemyType.Miniboss:
                        op = biome.MinibossEnemies.Get().InstantiateAsync(possibleMinibossSpawnPoint);
                        break;
                    case RoomEnemyType.Enemy:
                        Transform randomPosition = tempEnemySpawnPositions[Random.Range(0, tempEnemySpawnPositions.Count)];
                        op = biome.NormalEnemies.Get().InstantiateAsync(randomPosition);
                        tempEnemySpawnPositions.Remove(randomPosition);
                        break;
                    case RoomEnemyType.Minion:
                        Transform randomPositionMinion = tempEnemySpawnPositions[Random.Range(0, tempEnemySpawnPositions.Count)];
                        op = spawnableMinionList.Items[Random.Range(0, spawnableMinionList.Items.Count)].InstantiateAsync(randomPositionMinion);
                        tempEnemySpawnPositions.Remove(randomPositionMinion);
                        break;
                }
                int i = index;
                op.Completed += (handle) =>
                {
                    Tools.SimpleTimeProfiler.Instance?.AddTime(string.Format("Enemy {0}", i));
                    GameObject go = handle.Result;
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
                    finishedIndex++;
                };
                index++;
            }
            while(finishedIndex < roomData.GetRoomEnemies().Length)
            {
                yield return null;
            }
        }

        private IEnumerator ConfigureInteractables(RoomData roomData, RoomBiome biome)
        {
            List<Transform> tempInteractablePositions = new List<Transform>(possibleInteractablePositions);
            foreach(var interactable in roomData.GetInteractables())
            {
                Transform randomPosition = tempInteractablePositions[Random.Range(0, tempInteractablePositions.Count)];
                var op = interactableList[(int)interactable.roomInteractableType].InstantiateAsync(randomPosition);
                op.Completed += (handler) => {
                    GameObject go = handler.Result;
                    go.transform.localPosition = Vector3.zero;
                };
                yield return op;
                tempInteractablePositions.Remove(randomPosition);
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