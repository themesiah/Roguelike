using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Laresistance.Data;
using GamedevsToolbox.ScriptableArchitecture.Events;
using Laresistance.Interaction;

namespace Laresistance.Movement
{
    public class PlayerMovementManager : IMovementManager
    {
        public PlayerMovementData MovementData;
        public PlayerMovementStatus MovementStatus;
        public PlayerHorizontalMovement HorizontalMovement;
        public PlayerJump Jump;
        public PlayerScenarioInteraction ScenarioInteraction;

        private Transform transform;
        private Rigidbody2D body;
        private Animator animator;
        private GameEvent platformFallEvent;
        private bool paused = false;

        public PlayerMovementManager(PlayerMovementData movementData, Transform transform, Rigidbody2D body, Animator animator, GameEvent platformFallEvent, UnityAction<bool> onTurn)
        {
            this.MovementData = movementData;
            this.transform = transform;
            this.body = body;
            this.animator = animator;
            this.platformFallEvent = platformFallEvent;

            MovementStatus = new PlayerMovementStatus();
            HorizontalMovement = new PlayerHorizontalMovement(transform, body, MovementStatus, animator, MovementData.HorizontalSpeed, onTurn);
            Jump = new PlayerJump(transform, body, MovementStatus, animator, MovementData.JumpForce, platformFallEvent, MovementData.JumpsLimit);
            ScenarioInteraction = new PlayerScenarioInteraction();
        }

        public void Tick(float delta)
        {
            if (paused)
                return;
            HorizontalMovement.Tick(delta);
            Jump.Tick(delta);
        }

        public void Pause()
        {
            body.simulated = false;
            body.velocity = Vector3.zero;
            HorizontalMovement.Stop();
            paused = true;
        }

        public void Resume()
        {
            body.simulated = true;
            paused = false;
        }

        public void Turn()
        {
            HorizontalMovement.Turn();
        }

        public void Turn(bool r)
        {
            HorizontalMovement.Turn(r);
        }
    }
}