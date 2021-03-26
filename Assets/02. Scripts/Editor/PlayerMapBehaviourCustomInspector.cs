using UnityEditor;

namespace Laresistance.Behaviours
{
    [CustomEditor(typeof(PlayerMapBehaviour))]
    public class PlayerMapBehaviourCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (EditorApplication.isPlaying)
            {
                PlayerMapBehaviour playerMapBehaviour = (PlayerMapBehaviour)target;
                if (playerMapBehaviour.isActiveAndEnabled)
                {
                    EditorGUILayout.LabelField(string.Format("Jumping or falling: {0}", playerMapBehaviour.IsJumpingOrFalling));
                    EditorGUILayout.LabelField(string.Format("Velocity: {0}x{1}y", playerMapBehaviour.CurrentVelocity.x, playerMapBehaviour.CurrentVelocity.y));
                    EditorGUILayout.LabelField(string.Format("Platform falling: {0}", playerMapBehaviour.FallingSignal));
                }
            }
        }
    }
}