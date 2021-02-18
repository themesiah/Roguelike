using UnityEngine;

namespace Laresistance.LevelGeneration
{
    public class RoomGeneration
    {
        private RoomData roomData;
        private MapData mapData;
        private MapGeneration mapGeneration;


        public RoomGeneration(RoomData roomData, MapData mapData, MapGeneration mapGeneration)
        {
            this.roomData = roomData;
            this.mapData = mapData;
            this.mapGeneration = mapGeneration;
        }        
    }
}
