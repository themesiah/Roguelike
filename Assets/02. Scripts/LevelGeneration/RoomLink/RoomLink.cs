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
        public XYPair gridPosition;
        public int linkPosition; // (-1~1)
        public bool minimalPath;
    }
}