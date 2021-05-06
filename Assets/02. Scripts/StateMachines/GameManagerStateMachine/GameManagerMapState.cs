using System;
using System.Collections;
using GamedevsToolbox.StateMachine;
using Laresistance.Behaviours;
using UnityEngine.AddressableAssets;

namespace Laresistance.StateMachines
{
    public class GameManagerMapState : ICoroutineState
    {
        private string nextState = null;
        private AssetReference targetScene;

        public GameManagerMapState(AssetReference targetScene)
        {
            this.targetScene = targetScene;
        }

        public IEnumerator EnterState()
        {
            yield return GameSceneManager.Instance.SceneLoadAndChange(targetScene);
        }

        public IEnumerator ExitState()
        {
            nextState = null;
            yield return null;
        }

        public void Pause()
        {
        }

        public void ReceiveSignal(string signal)
        {
            nextState = signal;
        }

        public void Resume()
        {
        }

        public IEnumerator Update(Action<string> resolve)
        {
            if (nextState != null)
            {
                resolve(nextState);
            }
            yield return null;
        }
    }
}