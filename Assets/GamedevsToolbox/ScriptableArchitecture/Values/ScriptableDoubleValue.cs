using UnityEngine;

namespace GamedevsToolbox.ScriptableArchitecture.Values
{
    [CreateAssetMenu(menuName = "Scriptable Architecture/Values/Double")]
    public class ScriptableDoubleValue : ScriptableValue<double>
    {
        #region Public Methods
        public void IncrementValue(double increment)
        {
            SetValue(GetValue() + increment);
        }
        #endregion
    }
}