using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Events;
using Laresistance.Behaviours;

namespace Laresistance.Extensions
{
    [CreateAssetMenu(menuName = "Laresistance/Events/Rumble Signal")]
    public class RumbleSignalEvent : TemplatedGameEvent<RumbleSystem.RumbleSignal>
    {
    }
}
