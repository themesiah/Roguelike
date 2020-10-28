using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Data
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Minion Data list")]
    public class MinionList : ScriptableObject
    {
        public List<MinionData> minionList;
    }
}