using System.Collections;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class PartyManagerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Transform partyHolder = default;
        [SerializeField]
        private GameObject[] partyPrefabs = default;

        private void Awake()
        {
            foreach(GameObject prefab in partyPrefabs)
            {
                Instantiate(prefab, partyHolder);
            }
        }

        private void Start()
        {
            SpriteRenderer[] renderers = partyHolder.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer renderer in renderers)
            {
                renderer.sortingOrder -= 1;
            }
        }

        public GameObject[] GetFullParty()
        {
            GameObject[] objects = new GameObject[partyHolder.childCount + 1];
            objects[0] = gameObject;
            for(int i = 0; i < partyHolder.childCount; ++i)
            {
                objects[i + 1] = partyHolder.GetChild(i).gameObject;
            }
            return objects;
        }
    }
}