using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.LevelGeneration
{
    public class RoomGeneration
    {
        private RoomData roomData;
        private MapData mapData;
        private MapGeneration mapGeneration;
        private RoomBiome biome;

        private static float HALF_GROUND_UNIT = 5f;
        private static float SIXTH_GROUND_UNIT = HALF_GROUND_UNIT / 3f;
        private static float GROUND_UNIT = HALF_GROUND_UNIT * 2f;
        private static float CELL_HEIGHT_UNIT = 6.26f;

        private int bottomOffset;
        private int topOffset;
        private int leftOffset;
        private int rightOFfset;

        private Dictionary<AStarNode, ScenarioAsset> nodeAssetDictionary; 

        public RoomGeneration(RoomData roomData, MapData mapData, MapGeneration mapGeneration, RoomBiome biome)
        {
            this.roomData = roomData;
            this.mapData = mapData;
            this.mapGeneration = mapGeneration;
            this.biome = biome;
            nodeAssetDictionary = new Dictionary<AStarNode, ScenarioAsset>();
        }

        #region Coroutines
        public IEnumerator GenerateRoom()
        {
            // Instantiate shit here
            nodeAssetDictionary = new Dictionary<AStarNode, ScenarioAsset>();
            GameObject room = new GameObject("Room " + roomData.RoomIndex);
            room.transform.parent = null;
            room.transform.position = roomData.RoomPosition;
            AStarNode[] unrepeatedNodes = roomData.UniquePathNodes;

            yield return SetupMovementTests(unrepeatedNodes);
            yield return RecogniseElevators(unrepeatedNodes);
            yield return RecogniseRoomLimits(unrepeatedNodes);
            yield return RecogniseGround(unrepeatedNodes);
            yield return RecognisePlatforms(unrepeatedNodes);
            yield return RecogniseHeightChanges(unrepeatedNodes);
            yield return InstantiateAssets(room);
        }

        private IEnumerator SetupMovementTests(AStarNode[] unrepeatedNodes)
        {
            foreach (var node in unrepeatedNodes)
            {
                if (!nodeAssetDictionary.ContainsKey(node))
                {
                    // TODO
                }
            }
            yield return null;
        }

        private IEnumerator RecogniseElevators(AStarNode[] unrepeatedNodes)
        {
            foreach (var node in unrepeatedNodes)
            {
                if (!nodeAssetDictionary.ContainsKey(node))
                {
                    // TODO
                }
            }
            yield return null;
        }

        private IEnumerator RecogniseRoomLimits(AStarNode[] unrepeatedNodes)
        {
            bottomOffset = 99;
            topOffset = 99;
            leftOffset = 99;
            rightOFfset = 99;

            // Check the scenario limits. Where do paths end? Make a rectangle.
            foreach (var node in unrepeatedNodes)
            {
                if (node.Coordinates.x < leftOffset)
                {
                    leftOffset = node.Coordinates.x;
                }
                if (roomData.RoomSize.x - node.Coordinates.x - 1 < rightOFfset)
                {
                    rightOFfset = roomData.RoomSize.x - node.Coordinates.x - 1;
                }
                if (node.Coordinates.y < bottomOffset)
                {
                    bottomOffset = node.Coordinates.y;
                }
                if (roomData.RoomSize.y - node.Coordinates.y - 1 < topOffset)
                {
                    topOffset = roomData.RoomSize.y - node.Coordinates.y - 1;
                }
            }
            // Right, left and top use non-walkable tiles, so they need one more unit of margin.
            if (leftOffset > 0)
            {
                leftOffset--;
            }
            if (rightOFfset > 0)
            {
                rightOFfset--;
            }
            if (topOffset > 0)
            {
                topOffset--;
            }
            //bottomOffset++;

            // Using the offsets, draw the rectangle.
            foreach (var node in roomData.Grid.Nodes)
            {
                if (!nodeAssetDictionary.ContainsKey(node))
                {
                    XYPair gridPosition = node.Coordinates;
                    if (gridPosition.x == leftOffset && gridPosition.y == bottomOffset)
                    {
                        // Left wall base
                        nodeAssetDictionary.Add(node, new ScenarioAsset(biome.ScenarioManager.wallBase, false));
                    } else if (gridPosition.x == leftOffset && gridPosition.y == roomData.RoomSize.y - 1 - topOffset)
                    {
                        // Left wall corner
                        nodeAssetDictionary.Add(node, new ScenarioAsset(biome.ScenarioManager.wallCorner, false));
                    } else if (gridPosition.x == roomData.RoomSize.x - 1 - rightOFfset && gridPosition.y == bottomOffset)
                    {
                        // Right wall base
                        nodeAssetDictionary.Add(node, new ScenarioAsset(biome.ScenarioManager.wallBase, true));
                    } else if (gridPosition.x == roomData.RoomSize.x - 1 - rightOFfset && gridPosition.y == roomData.RoomSize.y - 1 - topOffset)
                    {
                        // Right wall corner
                        nodeAssetDictionary.Add(node, new ScenarioAsset(biome.ScenarioManager.wallCorner, true));
                    } else if (gridPosition.x == leftOffset && (gridPosition.y > bottomOffset && gridPosition.y < roomData.RoomSize.y - 1 - topOffset))
                    {
                        // Left wall
                        nodeAssetDictionary.Add(node, new ScenarioAsset(biome.ScenarioManager.walls, false));
                    } else if (gridPosition.x == roomData.RoomSize.x - 1 - rightOFfset && (gridPosition.y > bottomOffset && gridPosition.y < roomData.RoomSize.y - 1 - topOffset))
                    {
                        // Right wall
                        nodeAssetDictionary.Add(node, new ScenarioAsset(biome.ScenarioManager.walls, true));
                    } else if (gridPosition.y == bottomOffset && (gridPosition.x > leftOffset && gridPosition.x < roomData.RoomSize.x - 1 - rightOFfset))
                    {
                        // Ground
                        nodeAssetDictionary.Add(node, new ScenarioAsset(biome.ScenarioManager.groundTile, true));
                    } else if (gridPosition.y == roomData.RoomSize.y - 1 - topOffset && (gridPosition.x > leftOffset && gridPosition.x < roomData.RoomSize.x - 1 - rightOFfset))
                    {
                        // Ceiling
                        nodeAssetDictionary.Add(node, new ScenarioAsset(biome.ScenarioManager.ceiling, false));
                    }
                }
            }
            yield return null;
        }

        private IEnumerator RecogniseGround(AStarNode[] unrepeatedNodes)
        {
            List<AStarNode> unrepeatedList = new List<AStarNode>(unrepeatedNodes);
            // First get left ground
            foreach (var node in unrepeatedNodes)
            {
                if (!nodeAssetDictionary.ContainsKey(node))
                {
                    bool isLeftGround = true;
                    XYPair nodePosition = node.Coordinates;
                    //  Y 3 at max
                    if (nodePosition.y >= bottomOffset+3 || nodePosition.y <= bottomOffset)
                    {
                        continue;
                    }
                    // If all neighbours are on the right
                    foreach(var neighbour in node.Neighbours)
                    {
                        if (unrepeatedList.Contains(neighbour))
                        {
                            XYPair neighbourPosition = neighbour.Coordinates;
                            if (nodePosition.x >= neighbourPosition.x)
                            {
                                isLeftGround = false;
                                break;
                            }
                        }
                    }
                    if (!isLeftGround)
                        continue;
                    LogRoom(0, nodePosition, "passed test 1");
                    // If there are no ground to the bottom
                    for(int i = nodePosition.y-1; i >= 0; --i)
                    {
                        int index = MapGenerationUtils.CoordinatesToIndex(new XYPair() { x = nodePosition.x, y = i }, roomData.RoomSize.x);
                        AStarNode bottomNode = roomData.Grid.Nodes[index];
                        if (unrepeatedList.Contains(bottomNode))
                        {
                            isLeftGround = false;
                            break;
                        }
                    }
                    if (!isLeftGround)
                        continue;
                    LogRoom(0, nodePosition, "passed test 2");
                    // It is a left ground tile!
                    nodeAssetDictionary.Add(node, new ScenarioAsset(biome.ScenarioManager.groundTile, false));
                    // Put middle ground on neighbours
                    foreach (var neighbour in node.Neighbours)
                    {
                        if (unrepeatedList.Contains(neighbour))
                        {
                            XYPair neighbourPosition = neighbour.Coordinates;
                            if (nodePosition.x < neighbourPosition.x && nodePosition.y == neighbourPosition.y)
                            {
                                //Debug.LogFormat("{0}x{1}y of room {2} arrives here", neighbourPosition.x, neighbourPosition.y, roomData.RoomIndex);
                                nodeAssetDictionary.Add(neighbour, new ScenarioAsset(biome.ScenarioManager.middleGround, false));
                            }
                        }
                    }
                }
            }
            foreach (var node in unrepeatedNodes)
            {
                if (!nodeAssetDictionary.ContainsKey(node))
                {
                    XYPair nodePosition = node.Coordinates;
                    bool isGround = true;
                    //  Y 3 at max
                    if (nodePosition.y >= 3)
                    {
                        continue;
                    }
                    // If there are no ground to the bottom
                    for (int i = nodePosition.y - 1; i >= 0; --i)
                    {
                        int index = MapGenerationUtils.CoordinatesToIndex(new XYPair() { x = nodePosition.x, y = i }, roomData.RoomSize.x);
                        AStarNode bottomNode = roomData.Grid.Nodes[index];
                        if (unrepeatedList.Contains(bottomNode))
                        {
                            isGround = false;
                            break;
                        }
                    }
                    if (isGround)
                    {
                        nodeAssetDictionary.Add(node, new ScenarioAsset(biome.ScenarioManager.groundTile, true));
                    }
                }
            }
            yield return null;
        }

        private IEnumerator RecognisePlatforms(AStarNode[] unrepeatedNodes)
        {
            foreach (var node in unrepeatedNodes)
            {
                if (!nodeAssetDictionary.ContainsKey(node))
                {
                    List<AStarNode> unrepeatedList = new List<AStarNode>(unrepeatedNodes);
                    XYPair nodePosition = node.Coordinates;
                    bool isSoftPlatform = false;
                    // If there are no ground tiles to the bottom
                    for (int i = nodePosition.y - 1; i >= nodePosition.y - 2; --i)
                    {
                        if (i >= 0)
                        {
                            int index = MapGenerationUtils.CoordinatesToIndex(new XYPair() { x = nodePosition.x, y = i }, roomData.RoomSize.x);
                            AStarNode bottomNode = roomData.Grid.Nodes[index];
                            if (unrepeatedList.Contains(bottomNode) || nodeAssetDictionary.ContainsKey(bottomNode))
                            {
                                isSoftPlatform = true;
                                break;
                            }
                        }
                    }
                    if (isSoftPlatform)
                    {
                        nodeAssetDictionary.Add(node, new ScenarioAsset(biome.ScenarioManager.platformTile, true));
                    } else
                    {
                        nodeAssetDictionary.Add(node, new ScenarioAsset(biome.ScenarioManager.hardPlatformTile, true));
                    }
                }
            }
            yield return null;
        }

        private IEnumerator RecogniseHeightChanges(AStarNode[] unrepeatedNodes)
        {
            foreach (var node in unrepeatedNodes)
            {
                if (!nodeAssetDictionary.ContainsKey(node))
                {

                }
            }
            yield return null;
        }

        private IEnumerator InstantiateAssets(GameObject room)
        {
            foreach (var node in roomData.Grid.Nodes)
            {
                if (nodeAssetDictionary.ContainsKey(node))
                {
                    XYPair nodeGridPosition = node.Coordinates;
                    Vector2 nodePosition = new Vector2(nodeGridPosition.x * GROUND_UNIT, nodeGridPosition.y * CELL_HEIGHT_UNIT);
                    GameObject go = GameObject.Instantiate(nodeAssetDictionary[node].scenarioAssetData.GetRandomScenarioAsset(nodeAssetDictionary[node].right), room.transform);
                    go.name = string.Format("Node {0}x{1}y", node.Coordinates.x, node.Coordinates.y);
                    go.transform.localPosition = nodePosition;
                    yield return null;
                }
                else
                {
                    // Filler
                }
            }
        }
        #endregion

        private void LogRoom(int roomIndex, XYPair coordinates, string message)
        {
            if (roomData.RoomIndex == roomIndex)
            {
                Debug.LogFormat("Node {0}x{1}y {2}", coordinates.x, coordinates.y, message);
            }
        }
    }
}
