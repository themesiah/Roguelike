using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Selectors;
using GamedevsToolbox.ScriptableArchitecture.Sets;

namespace Laresistance.Behaviours
{
    [CreateAssetMenu(menuName = "Laresistance/Utils/Runtime Single GameObject Selector")]
    public class HealthBattleUIParentSelector : ScriptableArraySelector<RuntimeSingleGameObject>
    {
    }
}