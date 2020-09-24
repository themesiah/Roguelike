using UnityEngine.UI;
using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Localization;

namespace GamedevsToolbox.Examples
{
    public class LocalizedTextExample1 : MonoBehaviour
    {
        [SerializeField]
        private Text text1 = default;
        [SerializeField]
        private Text text2 = default;

        [SerializeField]
        private ScriptableTextRef text1Ref = default;
        [SerializeField]
        private ScriptableTextRef text2Ref = default;

        [SerializeField]
        private float cost = 0f;
        
        void Start()
        {

        }
        
        void Update()
        {
            text1.text = text1Ref.GetText(cost);
            string dateFormat = text2Ref.GetDateFormat();
            string dateText = System.DateTime.Now.ToString(dateFormat);
            text2.text = text2Ref.GetText(dateText);
        }
    }
}