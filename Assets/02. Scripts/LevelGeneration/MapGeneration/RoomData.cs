using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.LevelGeneration
{
    [System.Serializable]
    public class RoomData
    {
        private static int MAX_AREA = 225;
        private static float AREA_PER_ENEMY = 36f;
        private static float AREA_PER_INTERACTABLE = 18f;
        private static float AREA_PER_MOVEMENT_TEST = 24f;
        private static float AREA_PER_ROOM_LINK = 18f;

        [SerializeField]
        private List<RoomLink> roomConnections;
        [SerializeField]
        private MovementTest movementTest;
        [SerializeField]
        private List<RoomInteractable> roomInteractables;
        [SerializeField]
        private List<RoomEnemy> roomEnemies;

        [SerializeField]
        private int roomIndex;
        [SerializeField]
        private bool haveMovementTest;
        private MapData mapData;
        public int RoomIndex => roomIndex;
        public bool HaveMovementTest => haveMovementTest;
        public bool IsPathEnd => !IsMinimalPathRoom && roomConnections.Count == 1;
        public bool IsMinimalPathRoom { get; private set; }
        public bool IsFirstRoom { get; private set; }
        public bool IsLastRoom { get; private set; }

        public RoomData(int roomIndex, MapData mapData)
        {
            this.roomIndex = roomIndex;
            this.mapData = mapData;
            roomConnections = new List<RoomLink>();
            roomInteractables = new List<RoomInteractable>();
            roomEnemies = new List<RoomEnemy>();
        }

        #region RoomInitialization
        public void AddLink(RoomLink roomLink)
        {
            if (!roomConnections.Exists((link) => link.linkedRoom == roomLink.linkedRoom)) // The room we want to link is not linked yet
            {
                roomConnections.Add(roomLink);
            }
        }

        public RoomLink GetLinkFromRoom(RoomData roomData)
        {
            foreach(var link in roomConnections)
            {
                if (link.linkedRoom == roomData)
                {
                    return link;
                }
            }
            return new RoomLink();
        }

        public void RemoveLink(int linkIndex)
        {
            RoomLink link = roomConnections[linkIndex].linkedRoom.GetLinkFromRoom(this);
            if (link.linkedRoom != null)
            {
                roomConnections[linkIndex].linkedRoom.RemoveLink(link);
            }
            roomConnections.RemoveAt(linkIndex);
        }

        public void RemoveLink(RoomLink link)
        {
            roomConnections.Remove(link);
        }

        public RoomLink[] GetLinks()
        {
            return roomConnections.ToArray();
        }

        public void AddInteractable(RoomInteractable roomInteractable)
        {
            roomInteractables.Add(roomInteractable);
        }

        public RoomInteractable[] GetInteractables()
        {
            return roomInteractables.ToArray();
        }

        public void AddEnemy(RoomEnemy roomEnemy)
        {
            roomEnemies.Add(roomEnemy);
        }

        public RoomEnemy[] GetRoomEnemies()
        {
            return roomEnemies.ToArray();
        }

        public void SetAsMinimalPathRoom()
        {
            IsMinimalPathRoom = true;
        }

        public void SetAsFirstRoom()
        {
            IsFirstRoom = true;
        }

        public void SetAsLastRoom()
        {
            IsLastRoom = true;
        }

        public void SetMovementTest(MovementTest movementTest)
        {
            haveMovementTest = true;
            this.movementTest = movementTest;
        }

        public MovementTest GetMovementTest()
        {
            return movementTest;
        }
        #endregion

        #region RoomGeneration
        public void GenerateRoom()
        {
            // Get necessary data
            int numberOfEnemies = roomEnemies.Count;
            bool movementTest = haveMovementTest;
            int numberOfConnections = roomConnections.Count;
            int numberOfInteractables = roomInteractables.Count;
            // Generate size
            XYPair roomSize = GetRoomSize(numberOfEnemies, numberOfInteractables, numberOfConnections, movementTest);
            // Determinate center position. It works as an offset for all that is placed on the room.
            Vector2 centerPosition = GetRoomPosition();
            // Determinate link points on the limits of the room
            // Create at least one minimal path between every link point with each other link point. If the room have a movement test that blocks the path, the minimal path will have to follow other rules. For a minimal path between the bottom of the room and the top, there is some % that there is an elevator involved.
            // Determinate rewards position
            // Create at least one minimal path between somewhere on the minimal path of the links and the rewards. If there are no minimal paths between links (just one link, end of path room), use the start of the room. As for the links minimal paths, there could be an elevator or a movement test.
            // Determinate nodes IN THE PATH that will contain enemies.
            // Setup enemies
            // Setup decoration

        }

        private XYPair GetRoomSize(int numberOfEnemies, int numberOfInteractables, int numberOfConnections, bool haveMovementTest)
        {
            int totalRoomArea = 0;
            totalRoomArea += (int)(numberOfEnemies * AREA_PER_ENEMY);
            totalRoomArea += (int)(numberOfInteractables * AREA_PER_INTERACTABLE);
            totalRoomArea += (int)(numberOfConnections * AREA_PER_ROOM_LINK);
            if (haveMovementTest)
            {
                totalRoomArea += (int)AREA_PER_MOVEMENT_TEST;
            }
            totalRoomArea = System.Math.Min(totalRoomArea, MAX_AREA);
            float side = Mathf.Sqrt(totalRoomArea);
            int intSide = Mathf.CeilToInt(side);

            return new XYPair() { x = intSide, y = intSide };
        }

        private Vector2 GetRoomPosition()
        {
            XYPair mapSize = mapData.GetMapSize();
            XYPair roomPosInMap = MapGenerationUtils.IndexToCoordinates(RoomIndex, mapSize.x);
            Vector2 centerPosition = new Vector2() { x = roomPosInMap.x * MAX_AREA * 1.5f, y = roomPosInMap.y * MAX_AREA * 1.5f };
            return centerPosition;
        }
        #endregion
    }
}
