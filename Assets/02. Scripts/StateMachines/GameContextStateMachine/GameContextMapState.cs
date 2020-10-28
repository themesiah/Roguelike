using UnityEngine;
using GamedevsToolbox.StateMachine;
using System;
using System.Collections;
using Laresistance.Behaviours;
using UnityEngine.InputSystem;

namespace Laresistance.StateMachines
{
    public class GameContextMapState : ICoroutineState
    {
        private GameObject playerObject;
        private Camera playerCamera;
        private PlayerInput playerInput;
        private string signal = null;

        public GameContextMapState(GameObject playerObject, Camera playerCamera, PlayerInput playerInput)
        {
            this.playerObject = playerObject;
            this.playerCamera = playerCamera;
            this.playerInput = playerInput;
        }

        private void ObjectActivationAndDesactivation(bool enter)
        {
            playerObject.GetComponent<PlayerMapBehaviour>().enabled = enter;
            playerObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        public IEnumerator EnterState()
        {
            playerInput.SwitchCurrentActionMap("PlayerMap");
            ObjectActivationAndDesactivation(true);
            playerObject.GetComponent<PlayerMapBehaviour>().ResumeMapBehaviour();
            // Move camera
            GamedevsToolbox.Utils.Logger.Logger.Log("Entering map state");
            yield return null;
        }

        public IEnumerator ExitState()
        {
            ObjectActivationAndDesactivation(false);
            playerObject.GetComponent<PlayerMapBehaviour>().PauseMapBehaviour();
            signal = null;
            yield return null;
        }

        public void Pause()
        {
        }

        public void ReceiveSignal(string signal)
        {
            if (signal != null)
            {
                this.signal = signal;
            }
        }

        public void Resume()
        {
        }

        public IEnumerator Update(Action<string> resolve)
        {
            if (signal != null)
            {
                resolve(signal);
            }
            yield return null;
        }
    }
}