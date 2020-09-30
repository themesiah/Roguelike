using UnityEngine;

namespace GamedevsToolbox.Utils
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        public void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}