using UnityEngine;

namespace Laresistance.LevelGeneration
{
    [System.Serializable]
    public struct RoomLink
    {
        [System.NonSerialized]
        public RoomData linkedRoom;
        public int linkedRoomIndex;
        public RoomLinkType linkType;
        public RoomLinkLocation linkLocation;
        public XYPair gridPosition;
        public int linkPosition; // (1~4)
        public bool minimalPath;
    }
}