using GamedevsToolbox.ScriptableArchitecture.Events;
using Laresistance.Behaviours;
using UnityEngine;

namespace Laresistance.Extensions
{
    [CreateAssetMenu(menuName = "Laresistance/Events/Room Change event")]
    public class RoomChangeBehaviourEvent : TemplatedGameEvent<RoomChangeBehaviour>
    {
    }
}