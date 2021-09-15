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

        public override void Log(string message, GameObject go)
        {
            if (go == null)
                Utils.AppendText(logfileName, string.Format("[{0}] {1}", "LOG", message));
            else
                Utils.AppendText(logfileName, string.Format("[{0}][{1}] {2}", "LOG", go.name, message));
        }

        public override void LogWarning(string message, GameObject go)
        {
            if (go == null)
                Utils.AppendText(logfileName, string.Format("[{0}] {1}", "WARNING", message));
            else
                Utils.AppendText(logfileName, string.Format("[{0}][{1}] {2}", "WARNING", go.name, message));
        }

        public override void LogError(string message, GameObject go)
        {
            if (go == null)
                Utils.AppendText(logfileName, string.Format("[{0}] {1}", "ERROR", message));
            else
                Utils.AppendText(logfileName, string.Format("[{0}][{1}] {2}", "ERROR", go.name, message));
        }
    }
}