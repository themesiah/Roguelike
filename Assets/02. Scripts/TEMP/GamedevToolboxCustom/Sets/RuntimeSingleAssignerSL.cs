using GamedevsToolbox.ScriptableArchitecture.Sets;
using UnityEngine;

namespace Laresistance.Sets
{
    public class RuntimeSingleAssignerSL<T, T2> : MonoBehaviour where T : RuntimeSingle<T2>
    {
        [SerializeField] [ServiceName]
        private string serviceName = default;
        [SerializeField]
        private T2 component = default;

        protected void OnEnable()
        {
            Management.MonoServiceLocator.GetServiceSync<T>(serviceName).Set(component);
        }

        protected void OnDisable()
        {
            Management.MonoServiceLocator.GetServiceSync<T>(serviceName).Set(default);
        }
    }
}