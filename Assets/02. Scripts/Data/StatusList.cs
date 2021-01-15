using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Data
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Statuses List")]
    public class StatusList : ScriptableObject
    {
        [SerializeField]
        private StatusData[] statuses = default;
        public StatusData[] Statuses { get{ return statuses; } }
    }
}