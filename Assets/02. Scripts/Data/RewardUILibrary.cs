using Laresistance.Behaviours;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Laresistance.Data
{
    [CreateAssetMenu(menuName = "Laresistance/Sets/Reward UI Library")]
    public class RewardUILibrary : ScriptableObject
    {
        private Dictionary<RewardUIType, RewardPanelBehaviour> behaviourLibrary = null;

        private void InitLibrary()
        {
            if (behaviourLibrary == null)
            {
                behaviourLibrary = new Dictionary<RewardUIType, RewardPanelBehaviour>();
            }
        }
        public void RegisterBehaviour(RewardPanelBehaviour behaviour)
        {
            InitLibrary();
            //Assert.IsFalse(behaviourLibrary.ContainsKey(behaviour.RewardType), string.Format("Reward behaviour of type {0} is already registered on object {1}, and you are trying to register it again from object {2}",
            //    behaviour.RewardType.ToString(), behaviourLibrary[behaviour.RewardType].name, behaviour.name));
            behaviourLibrary.Add(behaviour.RewardType, behaviour);
        }

        public void UnregisterBehaviour(RewardPanelBehaviour behaviour)
        {
            InitLibrary();
            //Assert.IsTrue(behaviourLibrary.ContainsKey(behaviour.RewardType), string.Format("You are trying to unregister a Reward behaviour of type {0}, but it is not registered.", behaviour.RewardType));
            behaviourLibrary.Remove(behaviour.RewardType);
        }

        public RewardPanelBehaviour GetBehaviour(RewardUIType behaviourType)
        {
            InitLibrary();
            //Assert.IsTrue(behaviourLibrary.ContainsKey(behaviourType), string.Format("You are trying to get a Reward behaviour of type {0}, but it is not registered.", behaviourType));
            return behaviourLibrary[behaviourType];
        }
    }
}