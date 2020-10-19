using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Laresistance.Behaviours
{
    public class CoroutineHelperBehaviour : MonoBehaviour
    {
        private static CoroutineHelperBehaviour instance;

        private void Awake()
        {
            instance = this;
        }

        public static CoroutineHelperBehaviour GetInstance()
        {
            if (instance == null)
            {
                GameObject go = new GameObject("Coroutine Helper");
                instance = go.AddComponent<CoroutineHelperBehaviour>();
            }
            return instance;
        }
    }
}