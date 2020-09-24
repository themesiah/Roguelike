using UnityEngine;

namespace GamedevsToolbox.ScriptableArchitecture.Values
{
    [CreateAssetMenu(menuName = "Scriptable Architecture/Values/Float")]
    [System.Serializable]
    public class ScriptableFloatValue : ScriptableValue<float>
    {
        #region Public Methods
        public void IncrementValue(float increment)
        {
            SetValue(GetValue() + increment);
        }
        #endregion
    }
}