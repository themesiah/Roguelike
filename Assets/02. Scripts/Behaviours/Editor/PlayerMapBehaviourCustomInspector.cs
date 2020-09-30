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
                EditorGUILayout.LabelField("Jumping: " + playerMapBehaviour.IsJumping);
                EditorGUILayout.LabelField("Falling: " + playerMapBehaviour.IsFalling);
                EditorGUILayout.LabelField("Platform falling: " + playerMapBehaviour.IsPlatformFalling);
            }
        }
    }
}