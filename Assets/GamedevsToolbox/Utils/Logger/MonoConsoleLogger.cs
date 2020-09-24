using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.Utils.Logger
{
    public class MonoConsoleLogger : MonoLogger
    {
        public override void Log(string message)
        {
            Debug.Log(message);
        }

        public override void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }

        public override void LogError(string message)
        {
            Debug.LogError(message);
        }
    }
}