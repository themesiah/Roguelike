using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class EnemySpawnerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> prefabList = default;

        private void Start()
        {
            GameObject go = Instantiate(prefabList[Random.Range(0, prefabList.Count - 1)], transform.parent);
            go.transform.position = transform.position;
            Vector3 scale = go.transform.localScale;
            if (Random.Range(0, 1) == 0)
            {
                scale.x = -1;
            }
            go.transform.localScale = scale;
            Destroy(this.gameObject);
        }
    }
}