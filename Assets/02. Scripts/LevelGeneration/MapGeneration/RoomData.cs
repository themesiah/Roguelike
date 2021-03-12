using GamedevsToolbox.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.LevelGeneration
{
    [System.Serializable]
    public class RoomData
    {
        public static int LEFT_LINK_OFFSET = 1;
        public static int RIGHT_LINK_OFFSET = 1;
        public static int BOTTOM_LINK_OFFSET = 1;
        public static int TOP_LINK_OFFSET = 3;

        public static float ROOM_POSITION_OFFSET = 300f;

        public static Vector2 NOISE_SCALE_MIN_MAX = new Vector2() { x = 0.1f, y = 10f };
        public static Vector2 NOISE_WEIGHT_MIN_MAX = new Vector2() { x = 0.1f, y = 10f };
        public static Vector2 NOISE_OFFSET_MIN_MAX = new Vector2() { x = 0f, y = 10f };

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
        [System.NonSerialized]
        private MapData mapData;
        public int RoomIndex => roomIndex;
        public bool HaveMovementTest => haveMovementTest;
        public bool IsPathEnd => !IsMinimalPathRoom && roomConnections.Count == 1;
        public bool IsMinimalPathRoom { get; private set; }
        public bool IsFirstRoom { get; private set; }
        public bool IsLastRoom { get; private set; }
        public Vector2 RoomPosition => GetRoomPosition();

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
            foreach (var link in roomConnections)
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

        public void SetLink(RoomLink link, int index)
        {
            roomConnections[index] = link;
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

        private Vector2 GetRoomPosition()
        {
            XYPair mapSize = mapData.GetMapSize();
            XYPair roomPosInMap = MapGenerationUtils.IndexToCoordinates(RoomIndex, mapSize.x);
            Vector2 centerPosition = new Vector2() { x = roomPosInMap.x * ROOM_POSITION_OFFSET, y = roomPosInMap.y * ROOM_POSITION_OFFSET };
            return centerPosition;
        }
        #endregion
    }
}
