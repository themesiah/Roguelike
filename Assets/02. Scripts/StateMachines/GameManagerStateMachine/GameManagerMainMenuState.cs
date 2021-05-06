using System;
using System.Collections;
using GamedevsToolbox.StateMachine;
using Laresistance.Behaviours;
using UnityEngine.AddressableAssets;

namespace Laresistance.StateMachines
{
    public class GameManagerMainMenuState : ICoroutineState
    {
        private string nextScene = null;
        private AssetReference menuSceneReference;

        public GameManagerMainMenuState(AssetReference menuSceneReference)
        {
            this.menuSceneReference = menuSceneReference;
        }

        public IEnumerator EnterState()
        {
            yield return GameSceneManager.Instance.SceneLoadAndChange(menuSceneReference);
        }

        public IEnumerator ExitState()
        {
            nextScene = null;
            yield return null;
        }

        public void Pause()
        {
            // Nothing
        }

        public void ReceiveSignal(string signal)
        {
            nextScene = signal;
        }

        public void Resume()
        {
            // Nothing
        }

        public IEnumerator Update(Action<string> resolve)
        {
            if (nextScene != null)
            {
                resolve(nextScene);
            }
            yield return null;
        }
    }
}