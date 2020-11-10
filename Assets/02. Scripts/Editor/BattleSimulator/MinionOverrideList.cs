using Laresistance.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Simulator
{
    [CreateAssetMenu(menuName = "Laresistance/Simulator/Minions override list")]
    public class MinionOverrideList : ScriptableObject
    {
        [SerializeField]
        private List<MinionData> minions = default;
        public List<MinionData> Minions { get { return minions; } }
    }
}