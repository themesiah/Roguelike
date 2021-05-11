using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Tools
{
    public class SimpleTimeProfiler : MonoBehaviour
    {
        public static SimpleTimeProfiler Instance;

        [SerializeField]
        private string filename = "timeProfilerLog.log";
        [SerializeField]
        private bool logInConsole = false;

        private Dictionary<string, long> timestamps;

        private void Awake()
        {
            Instance = this;
            timestamps = new Dictionary<string, long>();
            GamedevsToolbox.Utils.Utils.DeleteFile(filename);
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void AddTime(string key)
        {
            if (timestamps.ContainsKey(key))
            {
                long startingTime = timestamps[key];
                long difference = GamedevsToolbox.Utils.Utils.GetTimestampInMillis() - startingTime;
                LogTime(key, difference);
            } else
            {
                timestamps.Add(key, GamedevsToolbox.Utils.Utils.GetTimestampInMillis());
            }
        }

        private void LogTime(string key, long time)
        {
            string text = string.Format("[{0}] {1} ms", key, time);
            GamedevsToolbox.Utils.Utils.AppendText(filename, text);
            timestamps.Remove(key);
            if (logInConsole)
            {
                Debug.LogFormat("[TimeProfiler] {0}", string.Format("[{0}] {1} ms", key, time));
            }
        }
    }
}