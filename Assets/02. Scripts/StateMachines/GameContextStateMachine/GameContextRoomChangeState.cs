using GamedevsToolbox.StateMachine;
using Laresistance.Behaviours;
using Laresistance.Data;
using System;
using System.Collections;
using UnityEngine;

namespace Laresistance.StateMachines
{
    public class GameContextRoomChangeState : ICoroutineState
    {
        private bool finished = false;
        private Rigidbody2D body;
        private Transform playerTransform;
        private Camera camera;
        private CameraMovementBehaviour cameraMovement;
        private RoomChangeData currentRoomData;
        private PlayerMovementData playerMovementData;

        public GameContextRoomChangeState(GameObject playerObject, Camera camera, PlayerMovementData playerMovementData)
        {
            body = playerObject.GetComponent<Rigidbody2D>();
            playerTransform = playerObject.transform;
            this.camera = camera;
            cameraMovement = camera.GetComponent<CameraMovementBehaviour>();
            this.playerMovementData = playerMovementData;
        }

        public IEnumerator EnterState()
        {
            // Put rigid body to non simulate
            body.simulated = false;
            // Deactivate camera auto movement
            cameraMovement.enabled = false;
            // Move character to border
            Vector3 targetPoint = playerTransform.position;
            targetPoint.x = currentRoomData.exitPoint.position.x;
            targetPoint.y = currentRoomData.exitPoint.position.y;
            while (playerTransform.position != targetPoint)
            {
                playerTransform.position = Vector3.MoveTowards(playerTransform.position, targetPoint, playerMovementData.HorizontalSpeed.GetValue() * Time.deltaTime);
                yield return null;
            }
            // Auto-move character to next room
            playerTransform.position = currentRoomData.nextRoom.GetRoomChangeData().exitPoint.position;
            cameraMovement.InstantUpdate(currentRoomData.nextRoom.GetRoomChangeData().enterPoint.position);
            finished = true;
            yield return null;
        }

        public IEnumerator ExitState()
        {
            // Move character from border
            Vector3 targetPoint = playerTransform.position;
            targetPoint.x = currentRoomData.nextRoom.GetRoomChangeData().enterPoint.position.x;
            targetPoint.y = currentRoomData.nextRoom.GetRoomChangeData().enterPoint.position.y;
            while (playerTransform.position != targetPoint)
            {
                playerTransform.position = Vector3.MoveTowards(playerTransform.position, targetPoint, playerMovementData.HorizontalSpeed.GetValue() * Time.deltaTime);
                yield return null;
            }
            // Activate camera movement
            cameraMovement.enabled = true;
            // Put rigid body to simulate
            body.simulated = true;
            yield return null;
        }

        public void Pause()
        {
        }

        public void ReceiveSignal(string signal)
        {
        }

        public void Resume()
        {
        }

        public IEnumerator Update(Action<string> resolve)
        {
            if (finished)
            {
                resolve("Map");
            }
            yield return null;
        }

        public void SetRoomData(RoomChangeData rcd)
        {
            currentRoomData = rcd;
        }
    }
}