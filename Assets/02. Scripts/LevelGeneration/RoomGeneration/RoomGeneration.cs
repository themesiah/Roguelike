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

        public RoomGeneration(RoomData roomData, MapData mapData, MapGeneration mapGeneration, RoomBiome biome)
        {
            this.roomData = roomData;
            this.mapData = mapData;
            this.mapGeneration = mapGeneration;
            this.biome = biome;
        }

        public IEnumerator GenerateRoom()
        {
            yield return null;
        }
    }
}
