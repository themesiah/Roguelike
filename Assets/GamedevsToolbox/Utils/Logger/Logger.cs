using UnityEngine;
using UnityEngine.Events;

namespace GamedevsToolbox.Utils.Logger
{
    public static class Logger
    {
        private static UnityAction<string> logAction = delegate { };
        private static UnityAction<string> warningAction = delegate { };
        private static UnityAction<string> errorAction = delegate { };

        public static void RegisterLogger(GamedevsToolbox.Utils.Logger.ILogger loggerImplementation)
        {
            logAction += loggerImplementation.Log;
            warningAction += loggerImplementation.LogWarning;
            errorAction += loggerImplementation.LogError;
        }

        public static void UnregisterLogger(GamedevsToolbox.Utils.Logger.ILogger loggerImplementation)
        {
            logAction -= loggerImplementation.Log;
            warningAction -= loggerImplementation.LogWarning;
            errorAction -= loggerImplementation.LogError;
        }

        public static void Log(string text)
        {
            logAction.Invoke(text);
#if UNITY_EDITOR
            if (!IsPlaying())
                Debug.Log(text);
#endif
        }

        public static void LogWarning(string text)
        {
            warningAction.Invoke(text);
#if UNITY_EDITOR
            if (!IsPlaying())
                Debug.LogWarning(text);
#endif
        }

        public static void LogError(string text)
        {
            errorAction.Invoke(text);
#if UNITY_EDITOR
            if (!IsPlaying())
                Debug.LogError(text);
#endif
        }

#if UNITY_EDITOR
        private static bool IsPlaying()
        {
            return UnityEditor.EditorApplication.isPlaying;
        }
#endif
    }
}