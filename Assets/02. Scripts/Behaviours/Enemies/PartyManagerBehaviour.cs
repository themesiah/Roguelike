using GamedevsToolbox.ScriptableArchitecture.Selectors;
using System.Collections;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class PartyManagerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Transform partyHolder = default;
        [SerializeField]
        private ScriptableGameObjectSelector[] partyMemberSelectors = default;

        private void Start()
        {
            SpawnParty();
        }

        public void SpawnParty()
        {
            foreach (ScriptableGameObjectSelector selector in partyMemberSelectors)
            {
                GameObject prefab = selector.Get();
                if (prefab != null)
                {
                    Instantiate(prefab, partyHolder);
                }
            }

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