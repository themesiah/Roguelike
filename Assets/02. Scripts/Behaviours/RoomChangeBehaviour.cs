using UnityEngine;
using Laresistance.Data;

namespace Laresistance.Behaviours
{
    [RequireComponent(typeof(Collider2D))]
    public class RoomChangeBehaviour : MonoBehaviour
    {
        [SerializeField]
        public Transform roomEnterPoint = default;
        [SerializeField]
        public Transform roomExitPoint = default;

        [SerializeField]
        private RoomChangeBehaviour nextRoom = default;

        private void Awake()
        {
            var hit = Physics2D.Raycast(roomEnterPoint.position, Vector2.down);
            roomEnterPoint.position = hit.point;
            var pos = roomExitPoint.position;
            pos.y = roomEnterPoint.position.y;
            roomExitPoint.position = pos;
        }

        public RoomChangeData GetRoomChangeData()
        {
            return new RoomChangeData(){ enterPoint = roomEnterPoint, exitPoint = roomExitPoint, nextRoom = nextRoom };
        }
    }
}