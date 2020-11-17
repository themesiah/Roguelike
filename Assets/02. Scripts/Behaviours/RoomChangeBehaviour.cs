﻿using UnityEngine;
using Laresistance.Data;
using Laresistance.Extensions;

namespace Laresistance.Behaviours
{
    [RequireComponent(typeof(Collider2D))]
    public class RoomChangeBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Transform roomEnterPoint = default;
        [SerializeField]
        private Transform roomExitPoint = default;
        [SerializeField]
        private bool snapEnterPointToFloor = true;
        [SerializeField]
        private bool snapExitPointToFloor = true;

        [SerializeField]
        private RoomChangeBehaviour nextRoom = default;
        [SerializeField]
        private RoomChangeBehaviourEvent roomChangeEvent = default;

        private void Awake()
        {
            var hit = Physics2D.Raycast(roomEnterPoint.position, Vector2.down);
            if (snapEnterPointToFloor)
            {
                roomEnterPoint.position = hit.point;
            }
            var pos = roomExitPoint.position;
            pos.y = roomEnterPoint.position.y;
            if (snapExitPointToFloor)
            {
                roomExitPoint.position = pos;
            }
        }

        public void ChangeRoom()
        {
            roomChangeEvent.Raise(this);
        }

        public RoomChangeData GetRoomChangeData()
        {
            return new RoomChangeData(){ enterPoint = roomEnterPoint, exitPoint = roomExitPoint, nextRoom = nextRoom };
        }

#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            if (nextRoom != null && nextRoom.GetRoomChangeData().exitPoint != null)
            {
                if (roomEnterPoint != null)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(roomEnterPoint.position, 1f);
                }
                if (roomExitPoint != null)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(roomExitPoint.position, 1f);
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(roomExitPoint.position, nextRoom.GetRoomChangeData().exitPoint.position);
                }
            }
        }
#endif
    }
}