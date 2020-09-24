using UnityEngine;
using UnityEditor;

namespace GamedevsToolbox.Utils {
    public class SnapToGroundEditor : MonoBehaviour
    {
        [MenuItem("Snap", menuItem = "Gamedevs Toolbox/Utils/SnapToGround")]
        public static void SnapToGrid()
        {
            GameObject go = Selection.activeGameObject;
            RaycastHit hitInfo;
            if (Physics.Raycast(go.transform.position, Vector3.down, out hitInfo))
            {
                go.transform.position = hitInfo.point;
            }
        }

        [MenuItem("Snap2", menuItem = "Gamedevs Toolbox/Utils/SnapToGroundBounds")]
        public static void SnapToGridBounds()
        {
            GameObject go = Selection.activeGameObject;
            RaycastHit hitInfo;
            if (Physics.Raycast(go.transform.position, Vector3.down, out hitInfo))
            {

                Collider c = go.GetComponent<Collider>();
                if (c)
                {
                    float ysize = c.bounds.size.y;
                    go.transform.position = hitInfo.point + Vector3.up * ysize / 2f;
                }
                else
                {
                    go.transform.position = hitInfo.point;
                }
            }
        }
    }
}