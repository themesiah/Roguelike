using UnityEngine;
using GamedevsToolbox.StateMachine;
using System;
using System.Collections;
using Laresistance.Behaviours;
using GamedevsToolbox.ScriptableArchitecture.Events;
using Laresistance.Systems;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Data;

namespace Laresistance.StateMachines
{
    public class GameContextMapState : ICoroutineState
    {
        private GameObject playerObject;
        private Camera playerCamera;
        private string signal = null;
        private PlayerMapBehaviour playerMapBehaviour;
        private Rigidbody2D playerBody;
        private bool paused = false;
        private StringGameEvent actionMapSwitchEvent;
        private BloodLossSystem bloodLossSystem;
        private RuntimeMapBehaviourSet mapBehavioursRef;
        private StringGameEvent virtualCameraChangeEvent;

        public GameContextMapState(GameObject playerObject, Camera playerCamera, StringGameEvent actionMapSwitchEvent, ScriptableIntReference bloodReference, RuntimeMapBehaviourSet mapBehavioursRef,
            StringGameEvent virtualCameraChangeEvent)
        {
            this.playerObject = playerObject;
            this.playerCamera = playerCamera;
            this.actionMapSwitchEvent = actionMapSwitchEvent;
            this.mapBehavioursRef = mapBehavioursRef;
            this.virtualCameraChangeEvent = virtualCameraChangeEvent;
            playerMapBehaviour = playerObject.GetComponent<PlayerMapBehaviour>();
            playerBody = playerObject.GetComponent<Rigidbody2D>();
            bloodLossSystem = new BloodLossSystem(playerObject.GetComponent<PlayerDataBehaviour>().player.GetEquipmentContainer(), bloodReference);
        }

        private void ObjectActivationAndDesactivation(bool enter)
        {
            playerMapBehaviour.enabled = enter;
            playerBody.velocity = Vector2.zero;
            mapBehavioursRef.ForEach((MapBehaviour mb) =>
            {
                if (enter)
                {
                    mb.ResumeMapBehaviour();
                } else
                {
                    mb.PauseMapBehaviour();
                }
            });
        }

        public IEnumerator EnterState()
        {
            actionMapSwitchEvent.Raise("PlayerMap");
            virtualCameraChangeEvent.Raise("MapCamera");
            ObjectActivationAndDesactivation(true);
            //playerObject.GetComponent<PlayerMapBehaviour>().ResumeMapBehaviour(); // No need to do this, as the player map behaviour is part of the map behaviours set.
            // Move camera
            GamedevsToolbox.Utils.Logger.Logger.Log("Entering map state");
            yield return null;
        }

        public IEnumerator ExitState()
        {
            if (signal != "RoomChange")
            {
                ObjectActivationAndDesactivation(false);
            }
            playerObject.GetComponent<PlayerMapBehaviour>().PauseMapBehaviour();
            signal = null;
            yield return null;
        }

        public void Pause()
        {
            playerMapBehaviour.PauseMapBehaviour();
            playerBody.simulated = false;
            paused = true;
        }

        public void ReceiveSignal(string signal)
        {
            Debug.LogFormat("Received signal {0} on Map State", signal);
            if (!string.IsNullOrEmpty(signal) && signal != "Map")
            {
                this.signal = signal;
            } else
            {
                this.signal = null;
            }
        }

        public void Resume()
        {
            playerMapBehaviour.ResumeMapBehaviour();
            playerBody.simulated = true;
            paused = false;
        }

        public IEnumerator Update(Action<string> resolve)
        {
            bool resolved = false;
            bloodLossSystem.Tick(Time.deltaTime);
            if (!paused)
            {
                if (!string.IsNullOrEmpty(signal))
                {
                    resolve(signal);
                    resolved = true;
                }
            }
            if (!resolved)
                yield return null;
        }
    }
}