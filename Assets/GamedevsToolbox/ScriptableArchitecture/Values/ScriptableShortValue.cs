using UnityEngine;

namespace GamedevsToolbox.ScriptableArchitecture.Values
{
    [CreateAssetMenu(menuName = "Scriptable Architecture/Values/Short")]
    public class ScriptableShortValue : ScriptableValue<short>
    {
        #region Public Methods
        public void IncrementValue(short increment)
        {
            SetValue((short)(GetValue() + increment));
        }
        #endregion
    }
}