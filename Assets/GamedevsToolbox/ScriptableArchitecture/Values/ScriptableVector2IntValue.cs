#pragma warning disable CS0649
using UnityEngine;

namespace GamedevsToolbox.ScriptableArchitecture.Values
{
    [CreateAssetMenu(menuName = "Scriptable Architecture/Values/Vector2Int")]
    public class ScriptableVector2IntValue : ScriptableValue<Vector2Int>
    {
        #region Public Methods
        public void IncrementValue(Vector2Int increment)
        {
            SetValue(GetValue() + increment);
        }
        #endregion
    }
}