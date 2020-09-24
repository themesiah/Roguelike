using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.Examples
{
    public class CubeRotate : MonoBehaviour
    {
        void Update()
        {
            transform.Rotate(0f, 100f * Time.deltaTime, 0f);
        }
    }
}