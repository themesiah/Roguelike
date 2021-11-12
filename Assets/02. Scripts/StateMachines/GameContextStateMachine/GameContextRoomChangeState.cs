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
        private GameEvent finishedChangingRoomEvent;

        public GameContextRoomChangeState(GameObject playerObject, Camera camera, PlayerMovementData playerMovementData, Collider2DGameEvent boundsChangeEvent, GameEvent finishedChangingRoomEvent)
        {
            body = playerObject.GetComponent<Rigidbody2D>();
            characterController = playerObject.GetComponent<Character2DController>();
            playerTransform = playerObject.transform;
            this.camera = camera;
            this.playerMovementData = playerMovementData;
            this.boundsChangeEvent = boundsChangeEvent;
            this.finishedChangingRoomEvent = finishedChangingRoomEvent;
        }

        public IEnumerator EnterState()
        {
            // Let the player fall
            while (!characterController.IsGrounded)
            {
                yield return null;
            }
            // Put rigid body to non simulate
            body.simulated = false;
            characterController.Pause();
            // Keep animating movement
            characterController.StartChangingRoom();
            // Move character to border
            Vector3 targetPoint = playerTransform.position;
            targetPoint.x = currentRoomData.exitPoint.position.x;
            targetPoint.y = currentRoomData.exitPoint.position.y;
            // Flip in case player moved just before entering the door
            if (playerTransform.position.x > targetPoint.x)
            {
                // Flip left
                characterController.Flip(false);
            }
            else
            {
                // Flip right
                characterController.Flip(true);
            }
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
            Vector3 enterPointPosition = currentRoomData.nextRoom.GetRoomChangeData().enterPoint.position;
            targetPoint.x = enterPointPosition.x;

            RaycastHit2D hit = Physics2D.Raycast(enterPointPosition + Vector3.up * 0.5f, Vector2.down);
            targetPoint.y = hit.point.y;
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
            finishedChangingRoomEvent?.Raise();
            characterController.ManualLanding();
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
            else
            {
                yield return null;
            }
        }

        public void SetRoomData(RoomChangeData rcd)
        {
            currentRoomData = rcd;
        }
    }
}