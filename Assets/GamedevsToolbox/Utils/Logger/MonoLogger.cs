using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.Utils.Logger
{
    public abstract class MonoLogger : MonoBehaviour, ILogger
    {
        private void OnEnable()
        {
            Logger.RegisterLogger(this);
        }

        private void OnDisable()
        {
            Logger.UnregisterLogger(this);
        }

        public abstract void Log(string message);
        public abstract void LogError(string message);
        public abstract void LogWarning(string message);
    }
}