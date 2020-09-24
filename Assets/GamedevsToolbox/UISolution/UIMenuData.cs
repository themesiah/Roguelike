using UnityEngine;

namespace GamedevsToolbox.UISolution
{
    [System.Serializable]
    public class UIMenuData
    {
        public string menuName;
        public GameObject menuObject;
        public Animator anim;
        public string inTrigger;
        public string outTrigger;
    }
}