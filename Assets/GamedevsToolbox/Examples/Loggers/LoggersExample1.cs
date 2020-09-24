using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.Examples
{
    public class LoggersExample1 : MonoBehaviour
    {
        void Start()
        {
            Utils.Logger.Logger.Log("This is a info log.");
            Utils.Logger.Logger.LogWarning("This is a warning. If you didn't deactivate the loggers game objects this should be printed in the log.txt file in your project too.");
            Utils.Logger.Logger.LogError("Why don't you try out to deactivate the game object and press play again?");
        }
    }
}