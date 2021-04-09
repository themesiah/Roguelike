using UnityEngine;

namespace GamedevsToolbox.ScriptableArchitecture.Sets
{
    [CreateAssetMenu(menuName = "Scriptable Architecture/Singles/Transform")]
    public class RuntimeSingleTransform : RuntimeSingle<Transform>
    {
        public void ReparentTransformReference(Transform newParent)
        {
            if (Get() != null)
            {
                Get().SetParent(newParent);
            }
        }

        public void UnparentTransformReference()
        {
            if (Get() != null)
            {
                Get().SetParent(null);
            }
        }
    }
}