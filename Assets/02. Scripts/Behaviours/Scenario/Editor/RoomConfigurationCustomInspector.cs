using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Laresistance.Battle;

namespace Laresistance.Behaviours
{
    [CustomEditor(typeof(RoomConfiguration))] [CanEditMultipleObjects]
    public class RoomConfigurationCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            

            if (GUILayout.Button("Check enemy space"))
            {
                RoomConfiguration rc = (RoomConfiguration)target;
                Transform[] enemies = rc.EnemySpawns;
                Transform miniboss = rc.MinibossSpawn;
                bool foundProblem = false;

                foreach (var enemy in enemies) {
                    try
                    {
                        BattlePosition.CheckSpace(enemy.position, rc.enemySpaceLayerMask);
                    } catch(System.Exception e)
                    {
                        Debug.LogError(e.Message);
                        Debug.LogErrorFormat("{0} doesn't have enough space.", enemy.name);
                        foundProblem = true;
                    }
                }
                if (miniboss != null)
                {
                    try
                    {
                        BattlePosition.CheckSpace(miniboss.position, rc.enemySpaceLayerMask);
                    } catch (System.Exception e) {
                        Debug.LogError(e.Message);
                        Debug.LogErrorFormat("{0} doesn't have enough space.", miniboss.name);
                        foundProblem = true;
                    }
                }
                if (!foundProblem)
                {
                    Debug.Log("All ok!");
                }
            }
        }
    }
}