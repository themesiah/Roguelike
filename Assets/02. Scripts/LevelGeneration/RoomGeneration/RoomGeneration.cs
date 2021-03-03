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

        private static float HALF_GROUND_UNIT = 5.66f;
        private static float SIXTH_GROUND_UNIT = HALF_GROUND_UNIT / 3f;
        private static float GROUND_UNIT = HALF_GROUND_UNIT * 2f;
        private static float CELL_HEIGHT_UNIT = 3.6f;

        public RoomGeneration(RoomData roomData, MapData mapData, MapGeneration mapGeneration, RoomBiome biome)
        {
            this.roomData = roomData;
            this.mapData = mapData;
            this.mapGeneration = mapGeneration;
            this.biome = biome;
        }

        public IEnumerator GenerateRoom()
        {
            // Instantiate shit here
            GameObject room = new GameObject("Room " + roomData.RoomIndex);
            room.transform.parent = null;
            room.transform.position = roomData.RoomPosition;
            List<AStarNode> unrepeatedNodes = new List<AStarNode>();
            foreach(var path in roomData.RoomPaths)
            {
                foreach(var node in path)
                {
                    if (!unrepeatedNodes.Contains(node))
                    {
                        unrepeatedNodes.Add(node);
                    }
                }
            }
            foreach(var node in unrepeatedNodes)
            {
                XYPair nodeGridPosition = MapGenerationUtils.IndexToCoordinates(node.Index, roomData.RoomSize.x);
                Vector2 nodePosition = new Vector2(nodeGridPosition.x * GROUND_UNIT, nodeGridPosition.y * CELL_HEIGHT_UNIT);
                GameObject go = GameObject.Instantiate(biome.ScenarioManager.middleGround.GetRandomScenarioAsset(true), room.transform);
                go.transform.localPosition = nodePosition;
            }
            yield return null;
        }
    }
}
