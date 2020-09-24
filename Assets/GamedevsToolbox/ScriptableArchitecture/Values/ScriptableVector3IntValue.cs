#pragma warning disable CS0649
using UnityEngine;

namespace GamedevsToolbox.ScriptableArchitecture.Values
{
    [CreateAssetMenu(menuName = "Scriptable Architecture/Values/Vector3Int")]
    public class ScriptableVector3IntValue : ScriptableValue<Vector3Int>
    {
        #region Public Methods
        public void IncrementValue(Vector3Int increment)
        {
            SetValue(GetValue() + increment);
        }
        #endregion
    }
}