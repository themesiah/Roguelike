using UnityEngine;

namespace Laresistance.Extensions
{
    public static class AnimatorExtension
    {

        public static bool HasParameter(this Animator animator, string paramName)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == paramName) return true;
            }
            return false;
        }
    }
}