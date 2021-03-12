using Laresistance.Behaviours;

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
        public bool minimalPath;
        public RoomChangeBehaviour roomChangeBehaviour;
    }
}