using Laresistance.Behaviours;
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
        private RoomConfiguration[] roomPrefabs;
        private RoomConfiguration roomPrefab;
        private GameObject roomObject;

        public RoomGeneration(RoomData roomData, MapData mapData, MapGeneration mapGeneration, RoomBiome biome, RoomConfiguration[] roomPrefabs)
        {
            this.roomData = roomData;
            this.mapData = mapData;
            this.mapGeneration = mapGeneration;
            this.biome = biome;
            this.roomPrefabs = roomPrefabs;
        }

        public void PregenerateRoom()
        {
            // Select a room prefab
            List<RoomConfiguration> validPrefabs = new List<RoomConfiguration>();
            foreach(var prefab in roomPrefabs)
            {
                if (prefab.CheckRoomRequirements(roomData))
                {
                    validPrefabs.Add(prefab);
                }
            }
            if (validPrefabs.Count > 0)
            {
                roomPrefab = validPrefabs[Random.Range(0, validPrefabs.Count)];
            } else
            {
                UnityEngine.Assertions.Assert.IsTrue(false, string.Format("Room with index {0} has no valid candidates.", roomData.RoomIndex));
            }
            // Instantiate
            roomObject = GameObject.Instantiate(roomPrefab.gameObject);
            roomObject.transform.position = roomData.RoomPosition;
            // Do preconfiguration
            roomObject.GetComponent<RoomConfiguration>().PreconfigureRoom(roomData);
        }

        public IEnumerator GenerateRoom()
        {
            // Configuration
            roomObject.GetComponent<RoomConfiguration>().ConfigureRoom(mapData, roomData, biome);
            yield return null;
        }
    }
}
