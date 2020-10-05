using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        public PlayerMovementManager(PlayerMovementData movementData, Transform transform, Rigidbody2D body, Animator animator, GameEvent platformFallEvent)
        {
            this.MovementData = movementData;
            this.transform = transform;
            this.body = body;
            this.animator = animator;
            this.platformFallEvent = platformFallEvent;

            MovementStatus = new PlayerMovementStatus();
            HorizontalMovement = new PlayerHorizontalMovement(transform, body, MovementStatus, animator, MovementData.HorizontalSpeed);
            Jump = new PlayerJump(transform, body, MovementStatus, animator, MovementData.JumpForce, platformFallEvent, MovementData.JumpsLimit);
            ScenarioInteraction = new PlayerScenarioInteraction();
        }

        public void Tick(float delta)
        {
            HorizontalMovement.Tick(delta);
            Jump.Tick(delta);
        }

        public void Stop()
        {
            body.simulated = false;
        }

        public void Resume()
        {
            body.simulated = true;
        }
    }
}