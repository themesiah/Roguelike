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
            int thresholdReduction = 0;
            int removedEnemies = 0;
            while (validPrefabs.Count == 0 && removedEnemies < 5) // Extreme case: there are no rooms with that many enemies.
            {
                thresholdReduction = 0;
                while (validPrefabs.Count == 0 && thresholdReduction < 5) // If there is no room candidate we make it easier for the generation.
                {
                    foreach (var prefab in roomPrefabs)
                    {
                        if (prefab.CheckRoomRequirements(roomData, thresholdReduction))
                        {
                            validPrefabs.Add(prefab);
                        }
                    }
                    thresholdReduction++;
                }
                if (validPrefabs.Count == 0)
                {
                    Debug.LogWarningFormat("Not enough candidates for room with index {0}. Removing enemies.", roomData.RoomIndex);
                    removedEnemies++;
                    roomData.RemoveLastEnemy();
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
            yield return roomObject.GetComponent<RoomConfiguration>().ConfigureRoom(mapData, roomData, biome);
            yield return null;
        }
    }
}
