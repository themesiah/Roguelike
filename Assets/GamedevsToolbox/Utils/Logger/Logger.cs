using UnityEngine;
using UnityEngine.Events;

namespace GamedevsToolbox.Utils.Logger
{
    public static class Logger
    {
        private static UnityAction<string, GameObject> logAction = delegate { };
        private static UnityAction<string, GameObject> warningAction = delegate { };
        private static UnityAction<string, GameObject> errorAction = delegate { };

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

        public static void Log(string text, GameObject go = null)
        {
            logAction.Invoke(text, go);
#if UNITY_EDITOR
            if (!IsPlaying())
                Debug.Log(text, go);
#endif
        }

        public static void LogWarning(string text, GameObject go = null)
        {
            warningAction.Invoke(text, go);
#if UNITY_EDITOR
            if (!IsPlaying())
                Debug.LogWarning(text, go);
#endif
        }

        public static void LogError(string text, GameObject go = null)
        {
            errorAction.Invoke(text, go);
#if UNITY_EDITOR
            if (!IsPlaying())
                Debug.LogError(text, go);
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