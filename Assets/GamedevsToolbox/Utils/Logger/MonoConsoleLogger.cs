using UnityEngine;

namespace GamedevsToolbox.Utils.Logger
{
    public class MonoConsoleLogger : MonoLogger
    {
        public override void Log(string message, GameObject go)
        {
            Debug.Log(message, go);
        }

        public override void LogWarning(string message, GameObject go)
        {
            Debug.LogWarning(message, go);
        }

        public override void LogError(string message, GameObject go)
        {
            Debug.LogError(message, go);
        }
    }
}