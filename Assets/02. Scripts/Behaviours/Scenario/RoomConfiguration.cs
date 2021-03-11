using UnityEngine;
using Laresistance.LevelGeneration;

namespace Laresistance.Behaviours
{
    public class RoomConfiguration : MonoBehaviour
    {
        // How many more space than actual content is needed to consider the room too big for the requirements.
        // For example, assigning a 1 enemy room to a room with 6 enemy spots available may result in a boring, empty room.
        private static int TOO_MUCH_SPACE_THRESHOLD = 2;

        [SerializeField]
        private Transform[] possibleEnemySpawnPoints = default;

        [SerializeField]
        private Transform possibleMinibossSpawnPoint = default;

        [SerializeField]
        private Transform[] possibleInteractablePositions = default;

        [SerializeField]
        private RoomChangeBehaviour topRoomConnection = null;

        [SerializeField]
        private RoomChangeBehaviour bottomRoomConnection = null;

        [SerializeField]
        private RoomChangeBehaviour rightRoomConnection = null;

        [SerializeField]
        private RoomChangeBehaviour leftRoomConnection = null;

        [SerializeField]
        private bool hasMovementTest = default;


        public bool CheckRoomRequirements(RoomData roomData, int thresholdReduction = 0)
        {
            // Check available links
            foreach(var link in roomData.GetLinks())
            {
                if (link.linkLocation == RoomLinkLocation.Top && topRoomConnection == null)
                {
                    return false;
                } else if (link.linkLocation == RoomLinkLocation.Bottom && bottomRoomConnection == null)
                {
                    return false;
                } else if (link.linkLocation == RoomLinkLocation.Right && rightRoomConnection == null)
                {
                    return false;
                } else if (link.linkLocation == RoomLinkLocation.Left && leftRoomConnection == null)
                {
                    return false;
                }
            }

            // Check enemy spots
            int nonMinibossEnemies = 0;
            foreach(var enemy in roomData.GetRoomEnemies())
            {
                if (enemy.roomEnemyType == RoomEnemyType.Miniboss)
                {
                    if (possibleMinibossSpawnPoint == null)
                    {
                        return false;
                    }
                } else
                {
                    nonMinibossEnemies++;
                }
            }
            if (nonMinibossEnemies > possibleEnemySpawnPoints.Length || possibleEnemySpawnPoints.Length - nonMinibossEnemies > TOO_MUCH_SPACE_THRESHOLD + thresholdReduction) // If there is not enough space or there are TOO MUCH space.
            {
                return false;
            }

            // Check interactable spots
            if (roomData.GetInteractables().Length > possibleInteractablePositions.Length || possibleInteractablePositions.Length - roomData.GetInteractables().Length > TOO_MUCH_SPACE_THRESHOLD + thresholdReduction)
            {
                return false;
            }

            // Check for movement test
            if (hasMovementTest != roomData.HaveMovementTest)
            {
                return false;
            }

            return true;
        }

        public void ConfigureRoom(RoomData roomData)
        {
        }
    }
}