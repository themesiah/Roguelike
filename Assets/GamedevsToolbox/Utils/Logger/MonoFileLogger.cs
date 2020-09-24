using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.Utils.Logger
{
    public class MonoFileLogger : MonoLogger
    {
        [SerializeField]
        private string logfileName = "log.txt";
        private void Awake()
        {
            Utils.DeleteFile(logfileName);
        }

        public override void Log(string message)
        {
            Utils.AppendText(logfileName, string.Format("[{0}] {1}", "LOG", message));
        }

        public override void LogWarning(string message)
        {
            Utils.AppendText(logfileName, string.Format("[{0}] {1}", "WARNING", message));
        }

        public override void LogError(string message)
        {
            Utils.AppendText(logfileName, string.Format("[{0}] {1}", "ERROR", message));
        }
    }
}