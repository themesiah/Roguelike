/*
 * https://docs.google.com/document/d/1BU7K4vIgCer1zyFsEUf82N1KuQoETkaBSPCyC7hUHP8/edit
 */

namespace Laresistance.LevelGeneration
{
    public class MapGeneration
    {
        public void GenerateMap()
        {
            XYPair size = GenerateMapSize();
            MapData mapData = new MapData(size);
            mapData.GenerateMontecarloMinimalPath();
            mapData.GenerateMontecarloExtraPaths();
            mapData.GenerateInteractables();
            mapData.GenerateMovementTestRooms();
            mapData.GenerateRoomEnemies();
        }

        private XYPair GenerateMapSize()
        {
            return new XYPair() { x = 4, y = 4 };
        }
    }
}