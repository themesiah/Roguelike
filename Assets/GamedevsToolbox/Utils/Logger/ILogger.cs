using UnityEngine;

namespace GamedevsToolbox.Utils.Logger
{
    public interface ILogger
    {
        void Log(string message, GameObject go);
        void LogWarning(string message, GameObject go);
        void LogError(string message, GameObject go);
    }
}