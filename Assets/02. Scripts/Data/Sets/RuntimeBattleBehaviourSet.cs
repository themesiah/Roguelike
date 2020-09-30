using GamedevsToolbox.ScriptableArchitecture.Sets;
using UnityEngine;
using Laresistance.Behaviours;

namespace Laresistance.Data
{
    [CreateAssetMenu(menuName = "Laresistance/Sets/Battle Behaviour")]
    public class RuntimeBattleBehaviourSet : RuntimeSet<CharacterBattleBehaviour>
    {
    }
}