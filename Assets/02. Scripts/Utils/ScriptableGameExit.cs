using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Laresistance.ScriptableUtils
{
    [CreateAssetMenu(menuName = "Laresistance/Utils/Game Exit")]
    public class ScriptableGameExit : ScriptableObject
    {
        public void ExitGame()
        {
            if (Application.isEditor)
            {
#if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
#endif
            }
            else
            {
                Application.Quit();
            }
        }
    }
}