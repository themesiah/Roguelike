using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class EnemySpawnerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> prefabList = default;

        [SerializeField]
        private bool hasParty = true;

        [SerializeField]
        private bool manualSpawn = false;

        [SerializeField]
        private bool fixedDirection = false;

        [SerializeField]
        private int levelOverride = 0;

        private void Start()
        {
            if (!manualSpawn)
            {
                SpawnEnemy();
            }
        }

        public void SpawnEnemy()
        {
            GameObject go = Instantiate(prefabList[Random.Range(0, prefabList.Count)], transform.parent);
            go.GetComponent<EnemyBattleBehaviour>().InitEnemy(levelOverride);
            go.transform.position = transform.position;
            Vector3 scale = go.transform.localScale;
            if (fixedDirection)
            {
                if (transform.localScale.x < 0f)
                {
                    scale.x = -1f;
                } else
                {
                    scale.x = 1f;
                }
            }
            else
            {
                if (Random.Range(0, 1) == 0)
                {
                    scale.x = -1;
                }
            }
            go.transform.localScale = scale;
            if (hasParty == false)
            {
                go.GetComponent<PartyManagerBehaviour>().enabled = false;
            }

            Destroy(this.gameObject);
        }
    }
}