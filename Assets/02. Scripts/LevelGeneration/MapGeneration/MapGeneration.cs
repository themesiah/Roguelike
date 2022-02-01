/*
 * https://docs.google.com/document/d/1BU7K4vIgCer1zyFsEUf82N1KuQoETkaBSPCyC7hUHP8/edit
 */

using UnityEngine;
using System.Collections;
using Laresistance.Behaviours;
using Laresistance.Data;
using System.Collections.Generic;
using UnityEngine.Events;
using Laresistance.Systems;

namespace Laresistance.LevelGeneration
{
    public class MapGeneration : MonoBehaviour
    {
        [SerializeField]
        private RoomBiome biome = default;

        [SerializeField]
        private RoomConfiguration[] roomPrefabs = default;

        [SerializeField]
        private int seed = -1;

        [SerializeField]
        private bool generateLogs = false;

        [SerializeField]
        private bool generateMock = false;

        [SerializeField]
        private RuntimePlayerDataBehaviourSingle playerDataRef = default;
        [SerializeField]
        private UnityEvent onFinishedGenerating = default;
        [SerializeField]
        private ScriptableGameSceneManager gameSceneManager = default;

        private int currentSeed = 0;

        private IEnumerator Start()
        {
            if (playerDataRef == null)
            {
                Debug.LogError("Missing player data reference. Can't generate a map");
                yield return null;
            } else if (generateMock)
            {
                yield return GenerateMapMock();
            }
            else
            {
                yield return GenerateMap();

            }
        }

        private IEnumerator GenerateMapMock()
        {
            XYPair size = new XYPair() { x = 3, y = 3 };
            MapData mapData = new MapData(size);
            mapData.GenerateMapMock();
            yield return GenerateRooms(mapData);
        }

        public IEnumerator GenerateMap()
        {
            if (seed == -1)
            {
                int newSeed = Random.Range(0, 999999);
                currentSeed = newSeed;
                Random.InitState(newSeed);
            }
            if (seed != -1)
            {
                currentSeed = seed;
                Random.InitState(seed);
            }
            XYPair size = GenerateMapSize();
            MapData mapData = new MapData(size);
            mapData.GenerateMontecarloMinimalPath();
            mapData.GenerateMontecarloExtraPaths();
            mapData.GenerateInteractables();
            mapData.GenerateMovementTestRooms();
            mapData.GenerateRoomEnemies();
            mapData.GenerateAltars();
            if (generateLogs)
                mapData.GenerateMapLog(currentSeed);
            yield return GenerateRooms(mapData);
        }

        private IEnumerator GenerateRooms(MapData mapData)
        {
            List<RoomGeneration> rooms = new List<RoomGeneration>();
            while (playerDataRef.Get() == null) {
                yield return null;
            }
            foreach(var room in mapData.GetAllRooms())
            {
                RoomGeneration rg = new RoomGeneration(room, mapData, this, biome, roomPrefabs);
                rg.PregenerateRoom();
                rooms.Add(rg);
            }
            foreach(var room in rooms)
            {
                yield return room.GenerateRoom();
            }
            onFinishedGenerating?.Invoke();
        }

        private XYPair GenerateMapSize()
        {
            return new XYPair() { x = 3, y = 3 };
        }

        public void LoadBossRoom()
        {
            gameSceneManager.LoadScene(biome.BossSceneRef);
        }
    }
}