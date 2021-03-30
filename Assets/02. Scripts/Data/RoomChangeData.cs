using Laresistance.Behaviours;
using UnityEngine;

namespace Laresistance.Data
{
    public struct RoomChangeData
    {
        public RoomChangeBehaviour nextRoom;
        public Transform enterPoint;
        public Transform exitPoint;
        public Collider2D bounds;
    }
}