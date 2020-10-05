using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Sets;

namespace GamedevsToolbox.Utils
{
    public class LookToCamera2D : MonoBehaviour
    {
        private bool right = true;

        private void Start()
        {
            //if (transform.parent.lossyScale.x < 0f)
            //{
            //    right = false;
            //} else
            //{
            //    right = true;
            //}
        }

        private void Update()
        {
            if (transform.parent.lossyScale.x < 0f && right)
            {
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x) * -1f;
                transform.localScale = scale;
                right = false;
            } else if (transform.parent.lossyScale.x > 0f && !right)
            {
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x);
                transform.localScale = scale;
                right = true;
            }
        }
    }
}