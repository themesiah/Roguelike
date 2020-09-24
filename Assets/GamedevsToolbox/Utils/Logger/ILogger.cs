using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.Utils.Logger
{
    public interface ILogger
    {
        void Log(string message);
        void LogWarning(string message);
        void LogError(string message);
    }
}