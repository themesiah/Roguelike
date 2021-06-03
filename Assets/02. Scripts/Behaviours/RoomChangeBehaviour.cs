using UnityEngine;
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
        private bool changeRoomOnEnterTrigger = true;

        [SerializeField]
        private RoomChangeBehaviour nextRoom = default;
        [SerializeField]
        private RoomChangeBehaviourEvent roomChangeEvent = default;

        [SerializeField]
        [Tooltip("Alternative objects to activate in case this connection is not used")]
        private GameObject[] alternativeObjects = default;

        [SerializeField]
        [Tooltip("Aditional objects to deactivate in case this connection is not used")]
        private GameObject[] aditionalObjects = default;

        public bool ChangeRoomOnEnterTrigger => changeRoomOnEnterTrigger;
        public bool IsLevelEnd => isLevelEnd;

        private bool isLevelEnd = false;
        private Collider2D roomBounds;

        public Transform RoomEnterPoint => roomEnterPoint;
        public Transform RoomExitPoint => roomExitPoint;
        public RoomChangeBehaviour NextRoom => nextRoom;

        private void Awake()
        {
            InitRoom();
        }

        public void ActivateAlternativeObjects()
        {
            foreach(var go in alternativeObjects)
            {
                go.SetActive(true);
            }
            foreach(var go in aditionalObjects)
            {
                go.SetActive(false);
            }
            gameObject.SetActive(false);
        }

        public void InitRoom()
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

        public void SetNextRoom(RoomChangeBehaviour nextRoom)
        {
            this.nextRoom = nextRoom;
        }

        public void SetRoomBounds(Collider2D collider)
        {
            roomBounds = collider;
        }

        public RoomChangeData GetRoomChangeData()
        {
            return new RoomChangeData(){ enterPoint = roomEnterPoint, exitPoint = roomExitPoint, nextRoom = nextRoom, bounds = roomBounds };
        }

        public void SetAsLevelEnd()
        {
            isLevelEnd = true;
        }

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
    }
}