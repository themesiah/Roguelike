#if UNITY_EDITOR && UNITY_STANDALONE
using UnityEngine;
using UnityEditor;

namespace Laresistance.Behaviours
{
    [CustomEditor(typeof(Character2DController))]
    public class Character2DControllerCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            ShowCustomInfo();
            base.OnInspectorGUI();
        }

        private void ShowCustomInfo()
        {
            Character2DController characterControler = (Character2DController)target;
            GUILayout.Label(string.Format("IsGrounded: {0}", characterControler.IsGrounded));
            GUILayout.Label(string.Format("CurrentMovement: {0}", characterControler.CurrentMovement));
            GUILayout.Label(string.Format("CurrentVelocity: {0}", characterControler.CurrentVelocity));
            GUILayout.Label(string.Format("IsJumping: {0}", characterControler.IsJumping));
            GUILayout.Label(string.Format("IsFalling: {0}", characterControler.IsFalling));
            GUILayout.Label(string.Format("IsOnSlope: {0}", characterControler.IsOnSlope));
            GUILayout.Label(string.Format("SlopeDownAngle: {0}", characterControler.SlopeDownAngle));
            GUILayout.Label(string.Format("SlopeSideAngle: {0}", characterControler.SlopeSideAngle));
            GUILayout.Label(string.Format("CanWalkOnSlope: {0}", characterControler.CanWalkOnSlope));
        }
    }
}
#endif