using GamedevsToolbox.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Laresistance.LevelGeneration
{
    [System.Serializable]
    public class MapData
    {
        private static string LOG_FILE = "mapDataLog.log";
        // Movement tests
        private static float MOVEMENT_TESTS_PER_NON_MINIMAL_PATH_ROOMS = 0.2f;
        // Interactables
        private static float BLOOD_CLUSTERS_PER_ROOM = 0.4f;
        private static float EQUIPMENTS_PER_CANDIDATE_ROOM = 0.5f; // A candidate room is the end of a non minimal path
        private static float FOUNTAINS_PER_ROOM = 0.25f;
        private static float LORE_NPC_PER_ROOM = 0f; // not yet 0.1f;
        // Enemies
        private static int[] NORMAL_ENEMIES_DISTRIBUTION = { 1, 1, 1, 2 };
        private static int[] MINION_ENEMIES_DISTRIBUTION = { 0, 1, 1, 0 };
        private static float MINIBOSS_ENEMIES_PER_ROOM = 2f / 16f;

        [SerializeField]
        private XYPair mapSize;
        [SerializeField]
        private List<RoomData> nodesData;
        private List<RoomData> minimalPath;
        public RoomData FirstRoom { get; private set; }
        public RoomData LastRoom { get; private set; }

        public MapData(XYPair mapSize)
        {
            this.mapSize = mapSize;
            int totalSize = mapSize.x * mapSize.y;
            nodesData = new List<RoomData>();
            for (int i = 0; i < totalSize; ++i)
            {
                nodesData.Add(new RoomData(i, this));
                if (i == 0)
                {
                    FirstRoom = nodesData[i];
                    nodesData[i].SetAsFirstRoom();
                } else if (i == totalSize-1)
                {
                    LastRoom = nodesData[i];
                    nodesData[i].SetAsLastRoom();
                }
            }
        }

        #region Public methods
        public XYPair GetMapSize()
        {
            return mapSize;
        }

        public void GenerateMapMock()
        {
            minimalPath = new List<RoomData>();
            minimalPath.Add(nodesData[0]);
            minimalPath.Add(nodesData[2]);
            minimalPath.Add(nodesData[3]);
            foreach (var room in minimalPath)
            {
                room.SetAsMinimalPathRoom();
            }
            GenerateMinimalPathLinks();
            List<RoomData> otherPath = new List<RoomData>();
            otherPath.Add(nodesData[0]);
            otherPath.Add(nodesData[1]);
            GeneratePathLinks(otherPath, false);
            RoomInteractable blood = new RoomInteractable() { roomInteractableType = RoomInteractableType.BloodReward };
            RoomInteractable equipment = new RoomInteractable() { roomInteractableType = RoomInteractableType.EquipmentReward };
            RoomInteractable pilgrim = new RoomInteractable() { roomInteractableType = RoomInteractableType.Pilgrim };
            RoomInteractable fountain = new RoomInteractable() { roomInteractableType = RoomInteractableType.Fountain };
            nodesData[0].AddInteractable(blood);
            nodesData[1].AddInteractable(fountain);
            nodesData[2].AddInteractable(equipment);
            nodesData[3].AddInteractable(pilgrim);
            RoomEnemy enemy1_1 = new RoomEnemy() { roomEnemyType = RoomEnemyType.Enemy };
            RoomEnemy enemy1_2 = new RoomEnemy() { roomEnemyType = RoomEnemyType.Minion };
            RoomEnemy enemy2_1 = new RoomEnemy() { roomEnemyType = RoomEnemyType.Enemy };
            RoomEnemy enemy2_2 = new RoomEnemy() { roomEnemyType = RoomEnemyType.Minion };
            RoomEnemy enemy3_1 = new RoomEnemy() { roomEnemyType = RoomEnemyType.Enemy };
            RoomEnemy enemy3_2 = new RoomEnemy() { roomEnemyType = RoomEnemyType.Minion };
            RoomEnemy enemy4_1 = new RoomEnemy() { roomEnemyType = RoomEnemyType.Enemy };
            RoomEnemy enemy4_2 = new RoomEnemy() { roomEnemyType = RoomEnemyType.Minion };
            nodesData[0].AddEnemy(enemy1_1);
            nodesData[0].AddEnemy(enemy1_2);
            nodesData[1].AddEnemy(enemy2_1);
            nodesData[1].AddEnemy(enemy2_2);
            nodesData[2].AddEnemy(enemy3_1);
            nodesData[2].AddEnemy(enemy3_2);
            nodesData[3].AddEnemy(enemy4_1);
            nodesData[3].AddEnemy(enemy4_2);
        }

        public void GenerateMontecarloMinimalPath()
        {
            minimalPath = GenerateMontecarloPath(FirstRoom, LastRoom);
            foreach(var room in minimalPath)
            {
                room.SetAsMinimalPathRoom();
            }
            GenerateMinimalPathLinks();
        }

        public void GenerateMontecarloExtraPaths()
        {
            List<RoomData> nonLinkedRooms = nodesData.FindAll(FilterNonLinkedRoom);
            int iterations = 0;
            while (nonLinkedRooms.Count > 0) // While there are non linked rooms
            {
                List<RoomData> linkedRooms = nodesData.FindAll(FilterLinkedRoom);
                RoomData randomNonLinkedRoom = nonLinkedRooms[Random.Range(0, nonLinkedRooms.Count)];
                Assert.IsTrue(randomNonLinkedRoom.GetLinks().Length == 0);
                List<List<RoomData>> paths = new List<List<RoomData>>();
                foreach(var linkedRoom in linkedRooms)
                {
                    Assert.IsTrue(linkedRoom.GetLinks().Length > 0);
                    var path = GenerateMontecarloPath(randomNonLinkedRoom, linkedRoom);
                    if (path.Count > 0)
                    {
                        Assert.AreEqual(linkedRoom, path[path.Count - 1]);
                        bool atLeastOneConnection = false;
                        foreach (var room in path)
                        {
                            if (room.GetLinks().Length > 0)
                            {
                                atLeastOneConnection = true;
                            }
                        }
                        Assert.IsTrue(atLeastOneConnection);
                        paths.Add(path);
                    }
                }
                if (paths.Count > 0)
                {
                    List<RoomData> randomPath = paths[Random.Range(0, paths.Count)];
                    GeneratePathLinks(randomPath, false);
                }
                nonLinkedRooms = nodesData.FindAll(FilterNonLinkedRoom);
                iterations++;
                if (iterations == 50)
                {
                    break;
                }
            }
            // Remove non minimal paths between minimal path rooms
            foreach (var room in nodesData)
            {
                if (room.IsMinimalPathRoom)
                {
                    for (int i = room.GetLinks().Length-1; i >= 0; --i)
                    {
                        if (room.GetLinks()[i].linkedRoom.IsMinimalPathRoom && !room.GetLinks()[i].minimalPath)
                        {
                            room.RemoveLink(i);
                        }
                    }
                }
            }
        }

        public void GenerateInteractables()
        {
            // Set pilgrim in minimal path, but not in the first or last room. Just 1 pilgrim.
            GeneratePilgrimInteractable();
            // Set equipment rewards, only in candidate rooms (end of non minimal path).
            GenerateEquipmentInteractables();
            // Set fountains. At least one in the minimal path.
            GenerateFountainInteractables();
            // Set lore pieces, on places with no other reward.
            GenerateLoreInteractables();
            // Set blood clusters, on places with no other reward.
            GenerateBloodInteractables();
        }

        public void GenerateMovementTestRooms()
        {
            var rooms = nodesData.FindAll(FilterNonMinimalPathRoom);
            if (rooms.Count > 0)
            {
                int amountOfMovementTests = Mathf.RoundToInt(rooms.Count * MOVEMENT_TESTS_PER_NON_MINIMAL_PATH_ROOMS);
                amountOfMovementTests = System.Math.Max(amountOfMovementTests, 1);
                // Search for path ends
                var pathEndRooms = nodesData.FindAll(FilterPathEnd);
                while(amountOfMovementTests > 0 && pathEndRooms.Count > 0)
                {
                    int pathEndRoomIndex = Random.Range(0, pathEndRooms.Count);
                    var path = GetUniquePathToEnd(pathEndRooms[pathEndRoomIndex]);
                    var randomRoom = path[Random.Range(0, path.Count)];
                    MovementTestType movementTestType = (MovementTestType)Random.Range(0, (int)MovementTestType.MAX);
                    MovementTest movementTest = new MovementTest() { movementTestType = movementTestType, movementTestUse = MovementTestUse.BlockPath };
                    if (randomRoom.IsPathEnd)
                    {
                        movementTest.movementTestUse = MovementTestUse.BlockReward;
                    }
                    randomRoom.SetMovementTest(movementTest);
                    pathEndRooms.RemoveAt(pathEndRoomIndex);
                    amountOfMovementTests--;
                }
            }
        }

        public void GenerateRoomEnemies()
        {
            // Generate minibosses first
            GenerateMinibosses();
            // Generate minions
            GenerateMinions();
            // Generate normal enemies
            GenerateNormalEnemies();
        }

        public void GenerateMapLog()
        {
            Utils.DeleteFile(LOG_FILE);
            // Log minimal path indexes
            Utils.AppendText(LOG_FILE, "///// MINIMAL PATH /////");
            System.Text.StringBuilder minimalPathBuilder = new System.Text.StringBuilder();
            foreach(var room in minimalPath)
            {
                minimalPathBuilder.AppendFormat("{0}, ", room.RoomIndex);
            }
            Utils.AppendText(LOG_FILE, minimalPathBuilder.ToString());
            // Log connections per room
            Utils.AppendText(LOG_FILE, "///// CONNECTIONS /////");
            foreach (var room in nodesData)
            {
                System.Text.StringBuilder roomConnectionsBuilder = new System.Text.StringBuilder();
                roomConnectionsBuilder.AppendFormat("Room with index {0} connects with {1} rooms: ", room.RoomIndex, room.GetLinks().Length);
                foreach(var link in room.GetLinks())
                {
                    roomConnectionsBuilder.AppendFormat("{0} ", link.linkedRoomIndex);
                }
                Utils.AppendText(LOG_FILE, roomConnectionsBuilder.ToString());
            }
            // Log movement test rooms
            Utils.AppendText(LOG_FILE, "///// MOVEMENT TEST /////");
            System.Text.StringBuilder movementTestBuilder = new System.Text.StringBuilder();
            movementTestBuilder.Append("Rooms with movement test are: ");
            foreach (var room in nodesData)
            {
                if (room.HaveMovementTest)
                {
                    movementTestBuilder.AppendFormat("{0}, ", room.RoomIndex);
                }
            }
            Utils.AppendText(LOG_FILE, movementTestBuilder.ToString());
            // Log interactables
            Utils.AppendText(LOG_FILE, "///// INTERACTABLES /////");
            foreach (var room in nodesData)
            {
                System.Text.StringBuilder roomInteractablesBuilder = new System.Text.StringBuilder();
                roomInteractablesBuilder.AppendFormat("Room with index {0} have {1} interactables: ", room.RoomIndex, room.GetInteractables().Length);
                foreach (var interactable in room.GetInteractables())
                {
                    roomInteractablesBuilder.AppendFormat("{0} ", interactable.roomInteractableType.ToString());
                }
                Utils.AppendText(LOG_FILE, roomInteractablesBuilder.ToString());
            }
            // Log enemies
            Utils.AppendText(LOG_FILE, "///// ENEMIES /////");
            foreach (var room in nodesData)
            {
                System.Text.StringBuilder roomEnemiesBuilder = new System.Text.StringBuilder();
                roomEnemiesBuilder.AppendFormat("Room with index {0} have {1} enemies: ", room.RoomIndex, room.GetRoomEnemies().Length);
                foreach (var enemy in room.GetRoomEnemies())
                {
                    roomEnemiesBuilder.AppendFormat("{0} ", enemy.roomEnemyType.ToString());
                }
                Utils.AppendText(LOG_FILE, roomEnemiesBuilder.ToString());
            }
        }

        public RoomData[] GetMinimalPath()
        {
            return minimalPath.ToArray();
        }

        public RoomData[] GetAllRooms()
        {
            return nodesData.ToArray();
        }

        public void ToJson(string fileName)
        {
            string data = JsonUtility.ToJson(this, true);
            Utils.SaveFile(fileName, data);
        }
        #endregion

        #region Private methods
        private List<RoomData> GetAdjacentRooms(RoomData room)
        {
            List<RoomData> adjacentRooms = new List<RoomData>();
            List<XYPair> adjacentCoordinates = MapGenerationUtils.NeighboursFromIndex(room.RoomIndex, mapSize);
            foreach (XYPair xypair in adjacentCoordinates)
            {
                adjacentRooms.Add(nodesData[MapGenerationUtils.CoordinatesToIndex(xypair, mapSize.x)]);
            }
            return adjacentRooms;
        }

        private List<RoomData> GetNonVisitedAdjacentRooms(RoomData room, List<RoomData> visited)
        {
            List<RoomData> adjacentRooms = GetAdjacentRooms(room);
            for (int i = adjacentRooms.Count-1; i >= 0; --i)
            {
                if (visited.Contains(adjacentRooms[i]))
                {
                    adjacentRooms.RemoveAt(i);
                }
            }
            return adjacentRooms;
        }

        private RoomData GetRandomNonVisitedAdjacentRoom(RoomData room, List<RoomData> visited)
        {
            List<RoomData> adjacentRooms = GetNonVisitedAdjacentRooms(room, visited);
            if (adjacentRooms.Count == 0)
                return null;
            int index = Random.Range(0, adjacentRooms.Count);
            return adjacentRooms[index];
        }

        private List<RoomData> GenerateMontecarloPath(RoomData startingRoom, RoomData endingRoom)
        {
            List<RoomData> path = new List<RoomData>();
            RoomData currentNode = null;
            int iterations = 0;
            while (currentNode != endingRoom)
            {
                if (currentNode == null)
                {
                    path.Clear();
                    currentNode = startingRoom;
                    path.Add(currentNode);
                }
                else
                {
                    RoomData next = GetRandomNonVisitedAdjacentRoom(currentNode, path);
                    path.Add(next);
                    currentNode = next;
                }
                iterations++;
                if (iterations >= 5000)
                {
                    path.Clear();
                    Debug.LogError("Something went wrong with the Montecarlo Minimal Path generation. Too many iterations.");
                    break;
                }
            }
            return path;
        }

        private void GenerateMinimalPathLinks()
        {
            GeneratePathLinks(minimalPath, true);
        }

        private void GeneratePathLinks(List<RoomData> path, bool minimalPath)
        {
            for (int i = 0; i < path.Count; ++i)
            {
                XYPair current = MapGenerationUtils.IndexToCoordinates(path[i].RoomIndex, mapSize.x);
                if (i > 0)
                {
                    // Link to previous
                    GenerateLink(path[i], path[i - 1], minimalPath);
                }
                if (i < path.Count - 1)
                {
                    // Link to next
                    GenerateLink(path[i], path[i + 1], minimalPath);
                }
            }
        }

        private void GenerateLink(RoomData current, RoomData linked, bool minimalPath)
        {
            XYPair currentCoordinates = MapGenerationUtils.IndexToCoordinates(current.RoomIndex, mapSize.x);
            XYPair linkedCoordinates = MapGenerationUtils.IndexToCoordinates(linked.RoomIndex, mapSize.x);
            RoomLinkType linkType;
            RoomLinkLocation currentLocation;
            RoomLinkLocation linkedLocation;
            if (currentCoordinates.x == linkedCoordinates.x) // vertical
            {
                linkType = RoomLinkType.Vertical;
                if (currentCoordinates.y > linkedCoordinates.y)
                {
                    currentLocation = RoomLinkLocation.Bottom;
                    linkedLocation = RoomLinkLocation.Top;
                } else
                {
                    currentLocation = RoomLinkLocation.Top;
                    linkedLocation = RoomLinkLocation.Bottom;
                }
            }
            else // horizontal
            {
                linkType = RoomLinkType.Horizontal;
                if (currentCoordinates.x > linkedCoordinates.x)
                {
                    currentLocation = RoomLinkLocation.Left;
                    linkedLocation = RoomLinkLocation.Right;
                }
                else
                {
                    currentLocation = RoomLinkLocation.Right;
                    linkedLocation = RoomLinkLocation.Left;
                }
            }
            current.AddLink(new RoomLink() { linkedRoom = linked, linkedRoomIndex = linked.RoomIndex, linkLocation = currentLocation, linkType = linkType, minimalPath = minimalPath });
            linked.AddLink(new RoomLink() { linkedRoom = current, linkedRoomIndex = current.RoomIndex, linkLocation = linkedLocation, linkType = linkType, minimalPath = minimalPath });
        }

        private void GeneratePilgrimInteractable()
        {
            var pilgrimAbleRooms = nodesData.FindAll(FilterMinimalPathRoom).FindAll(FilterNonStartingRoom).FindAll(FilterNonFinalRoom);
            Assert.IsTrue(pilgrimAbleRooms.Count > 0);
            int indexOfPilgrim = Random.Range(0, pilgrimAbleRooms.Count);
            RoomInteractable pilgrim = new RoomInteractable() { roomInteractableType = RoomInteractableType.Pilgrim };
            pilgrimAbleRooms[indexOfPilgrim].AddInteractable(pilgrim);
        }

        private void GenerateBloodInteractables()
        {
            var bloodCandidates = nodesData.FindAll(FilterRoomWithNoRewards);
            int numberOfBloodClusters = Mathf.RoundToInt(mapSize.x * mapSize.y * BLOOD_CLUSTERS_PER_ROOM);
            while (numberOfBloodClusters > 0 && bloodCandidates.Count > 0)
            {
                int bloodIndex = Random.Range(0, bloodCandidates.Count);
                RoomInteractable blood = new RoomInteractable() { roomInteractableType = RoomInteractableType.BloodReward };
                bloodCandidates[bloodIndex].AddInteractable(blood);
                bloodCandidates.RemoveAt(bloodIndex);
                numberOfBloodClusters--;
            }
        }

        private void GenerateLoreInteractables()
        {
            var loreCandidates = nodesData.FindAll(FilterRoomWithNoRewards);
            int numberOfLoreInteractables = Mathf.RoundToInt(mapSize.x * mapSize.y * LORE_NPC_PER_ROOM);
            while (numberOfLoreInteractables > 0 && loreCandidates.Count > 0)
            {
                int loreIndex = Random.Range(0, loreCandidates.Count);
                RoomInteractable lore = new RoomInteractable() { roomInteractableType = RoomInteractableType.Lore };
                loreCandidates[loreIndex].AddInteractable(lore);
                loreCandidates.RemoveAt(loreIndex);
                numberOfLoreInteractables--;
            }
        }

        private void GenerateFountainInteractables()
        {
            var fountainCandidates = nodesData.FindAll(FilterRoomWithNoRewards).FindAll(FilterNonStartingRoom).FindAll(FilterNonFinalRoom);
            int numberOfFountains = Mathf.RoundToInt(mapSize.x * mapSize.y * FOUNTAINS_PER_ROOM);
            Assert.IsTrue(numberOfFountains > 0);
            var fountainMinimalPathCandidates = fountainCandidates.FindAll(FilterMinimalPathRoom);
            int fountainIndex = Random.Range(0, fountainMinimalPathCandidates.Count);
            RoomInteractable fountain = new RoomInteractable() { roomInteractableType = RoomInteractableType.Fountain };
            fountainMinimalPathCandidates[fountainIndex].AddInteractable(fountain);
            fountainCandidates.Remove(fountainMinimalPathCandidates[fountainIndex]);
            numberOfFountains--;
            while (numberOfFountains > 0 && fountainCandidates.Count > 0)
            {
                fountainIndex = Random.Range(0, fountainCandidates.Count);
                RoomInteractable fountain2 = new RoomInteractable() { roomInteractableType = RoomInteractableType.Fountain };
                fountainCandidates[fountainIndex].AddInteractable(fountain2);
                fountainCandidates.RemoveAt(fountainIndex);
                numberOfFountains--;
            }
        }

        private void GenerateEquipmentInteractables()
        {
            var equipmentCandidates = nodesData.FindAll(FilterPathEnd);
            int numberOfEquipments = Mathf.FloorToInt(equipmentCandidates.Count * EQUIPMENTS_PER_CANDIDATE_ROOM);
            if (numberOfEquipments == 0 && equipmentCandidates.Count > 0)
            {
                numberOfEquipments = 1; // At least one equipment if there is at least one candidate.
            }
            while (numberOfEquipments > 0 && equipmentCandidates.Count > 0)
            {
                RoomInteractable equipment = new RoomInteractable() { roomInteractableType = RoomInteractableType.EquipmentReward };
                int equipmentIndex = Random.Range(0, equipmentCandidates.Count);
                equipmentCandidates[equipmentIndex].AddInteractable(equipment);
                equipmentCandidates.RemoveAt(equipmentIndex);
                numberOfEquipments--;
            }
        }

        private void GenerateNormalEnemies()
        {
            var enemyCandidates = nodesData.FindAll(FilterAll);
            int index = 0;
            while (enemyCandidates.Count > 0)
            {
                int distributionIndex = index % NORMAL_ENEMIES_DISTRIBUTION.Length;
                int enemiesAmount = NORMAL_ENEMIES_DISTRIBUTION[distributionIndex];
                var randomCandidate = enemyCandidates[Random.Range(0, enemyCandidates.Count)];
                for (int i = 0; i < enemiesAmount; ++i)
                {
                    RoomEnemy enemy = new RoomEnemy() { roomEnemyType = RoomEnemyType.Enemy };
                    randomCandidate.AddEnemy(enemy);
                }
                enemyCandidates.Remove(randomCandidate);
                index++;
            }
        }

        private void GenerateMinions()
        {
            var minionCandidates = nodesData.FindAll(FilterAll);
            int index = 0;
            while (minionCandidates.Count > 0)
            {
                int distributionIndex = index % MINION_ENEMIES_DISTRIBUTION.Length;
                int minionAmount = MINION_ENEMIES_DISTRIBUTION[distributionIndex];
                var randomCandidate = minionCandidates[Random.Range(0, minionCandidates.Count)];
                for (int i = 0; i < minionAmount; ++i)
                {
                    RoomEnemy minion = new RoomEnemy() { roomEnemyType = RoomEnemyType.Minion };
                    randomCandidate.AddEnemy(minion);
                }
                minionCandidates.Remove(randomCandidate);
                index++;
            }
        }

        private void GenerateMinibosses()
        {
            int totalMapSize = mapSize.x * mapSize.y;
            int amountOfMinibosses = Mathf.FloorToInt(totalMapSize * MINIBOSS_ENEMIES_PER_ROOM);
            if (amountOfMinibosses > 0)
            {
                int minibossIndex = minimalPath.Count / 2; // Exactly in the middle of the minimal path
                RoomEnemy miniboss1 = new RoomEnemy() { roomEnemyType = RoomEnemyType.Miniboss };
                minimalPath[minibossIndex].AddEnemy(miniboss1);
                amountOfMinibosses--;
                var minibossCandidates = nodesData.FindAll(FilterNonLinkedRoom);
                while (amountOfMinibosses > 0 && minibossCandidates.Count > 0)
                {
                    minibossIndex = Random.Range(0, minibossCandidates.Count);
                    RoomEnemy miniboss = new RoomEnemy() { roomEnemyType = RoomEnemyType.Miniboss };
                    minibossCandidates[minibossIndex].AddEnemy(miniboss);
                    minibossCandidates.RemoveAt(minibossIndex);
                    amountOfMinibosses--;
                }
            }
        }

        private List<RoomData> GetUniquePathToEnd(RoomData pathEndRoom)
        {
            Assert.IsTrue(pathEndRoom.IsPathEnd);
            RoomData currentRoom = pathEndRoom;
            List<RoomData> uniquePath = new List<RoomData>();
            uniquePath.Add(currentRoom);
            while (!currentRoom.IsMinimalPathRoom && currentRoom.GetLinks().Length < 2)
            {
                Assert.IsTrue(currentRoom.GetLinks().Length > 0);
                currentRoom = pathEndRoom.GetLinks()[0].linkedRoom;
                uniquePath.Add(currentRoom);
            }
            uniquePath.Reverse();
            return uniquePath;
        }
        #endregion

        #region Room Filters
        private bool FilterLinkedRoom(RoomData roomData)
        {
            return roomData.GetLinks().Length > 0;
        }

        private bool FilterNonLinkedRoom(RoomData roomData)
        {
            return roomData.GetLinks().Length == 0;
        }

        private bool FilterMinimalPathRoom(RoomData roomData)
        {
            return roomData.IsMinimalPathRoom;
        }

        private bool FilterNonMinimalPathRoom(RoomData roomData)
        {
            return !roomData.IsMinimalPathRoom;
        }

        private bool FilterRoomWithNoRewards(RoomData roomData)
        {
            return roomData.GetInteractables().Length == 0;
        }

        private bool FilterRoomWithRewards(RoomData roomData)
        {
            return !FilterRoomWithNoRewards(roomData);
        }

        private bool FilterNonStartingRoom(RoomData roomData)
        {
            return !roomData.IsFirstRoom;
        }

        private bool FilterNonFinalRoom(RoomData roomData)
        {
            return !roomData.IsLastRoom;
        }

        private bool FilterPathEnd(RoomData roomData)
        {
            return roomData.IsPathEnd;
        }

        private bool FilterEquipmentReward(RoomData roomData)
        {
            foreach(var interactable in roomData.GetInteractables())
            {
                if (interactable.roomInteractableType == RoomInteractableType.EquipmentReward)
                {
                    return true;
                }
            }
            return false;
        }

        private bool FilterAll(RoomData roomData)
        {
            return true;
        }
        #endregion
    }
}