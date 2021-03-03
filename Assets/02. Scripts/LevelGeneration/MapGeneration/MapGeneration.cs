/*
 * https://docs.google.com/document/d/1BU7K4vIgCer1zyFsEUf82N1KuQoETkaBSPCyC7hUHP8/edit
 */

using UnityEngine;
using System.Collections;

namespace Laresistance.LevelGeneration
{
    public class MapGeneration : MonoBehaviour
    {
        [SerializeField]
        private RoomBiome biome = default;

        [SerializeField]
        private int seed = -1;

        private void Start()
        {
            GenerateMap();
        }

        public void GenerateMap()
        {
            if (seed != -1)
            {
                Random.InitState(seed);
            }
            XYPair size = GenerateMapSize();
            MapData mapData = new MapData(size);
            mapData.GenerateMontecarloMinimalPath();
            mapData.GenerateMontecarloExtraPaths();
            mapData.GenerateInteractables();
            mapData.GenerateMovementTestRooms();
            mapData.GenerateRoomEnemies();
            mapData.GenerateRoomsGrids();
            StartCoroutine(GenerateRooms(mapData));
        }

        private IEnumerator GenerateRooms(MapData mapData)
        {
            foreach(var room in mapData.GetAllRooms())
            {
                RoomGeneration rg = new RoomGeneration(room, mapData, this, biome);
                yield return rg.GenerateRoom();
                yield return null;
            }
        }

        private XYPair GenerateMapSize()
        {
            return new XYPair() { x = 4, y = 4 };
        }
    }
}