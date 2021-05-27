using GamedevsToolbox.StateMachine;
using GamedevsToolbox.ScriptableArchitecture.Events;
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
        private Character2DController characterController;
        private Transform playerTransform;
        private Camera camera;
        private RoomChangeData currentRoomData;
        private PlayerMovementData playerMovementData;
        private Collider2DGameEvent boundsChangeEvent;

        public GameContextRoomChangeState(GameObject playerObject, Camera camera, PlayerMovementData playerMovementData, Collider2DGameEvent boundsChangeEvent)
        {
            body = playerObject.GetComponent<Rigidbody2D>();
            characterController = playerObject.GetComponent<Character2DController>();
            playerTransform = playerObject.transform;
            this.camera = camera;
            this.playerMovementData = playerMovementData;
            this.boundsChangeEvent = boundsChangeEvent;
        }

        public IEnumerator EnterState()
        {
            // Put rigid body to non simulate
            body.simulated = false;
            // Keep animating movement
            characterController.StartChangingRoom();
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
            finished = true;
            // Set next room bounds for the camera confinement
            boundsChangeEvent?.Raise(currentRoomData.nextRoom.GetRoomChangeData().bounds);
            yield return null;
        }

        public IEnumerator ExitState()
        {
            Vector3 targetPoint = playerTransform.position;
            targetPoint.x = currentRoomData.nextRoom.GetRoomChangeData().enterPoint.position.x;
            targetPoint.y = currentRoomData.nextRoom.GetRoomChangeData().enterPoint.position.y;
            // Flip character
            if (playerTransform.position.x > targetPoint.x)
            {
                // Flip left
                characterController.Flip(false);
            } else
            {
                // Flip right
                characterController.Flip(true);
            }
            // Move character from border
            while (playerTransform.position != targetPoint)
            {
                playerTransform.position = Vector3.MoveTowards(playerTransform.position, targetPoint, playerMovementData.HorizontalSpeed.GetValue() * Time.deltaTime);
                yield return null;
            }
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