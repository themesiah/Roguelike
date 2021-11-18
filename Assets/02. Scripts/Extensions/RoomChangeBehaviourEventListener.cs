using Laresistance.Behaviours;
using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Events;

namespace Laresistance.Extensions
{
    public class RoomChangeBehaviourEventListener : TemplatedGameEventListener<RoomChangeBehaviour>
    {
        public override void OnEventRaised(RoomChangeBehaviour data)
        {
            base.OnEventRaised(data);
            Debug.LogWarningFormat("Room change raised on object {0}", gameObject.name);
        }
    }
}