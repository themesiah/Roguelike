using GamedevsToolbox.ScriptableArchitecture.Selectors;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Laresistance.Behaviours
{
    public class PartyManagerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Transform partyHolder = default;
        [SerializeField]
        private ScriptableAssetReferenceSelector[] partyMemberSelectorsRef = default;

        private void Start()
        {
            SpawnParty();
        }

        public void SpawnParty()
        {
            foreach(ScriptableAssetReferenceSelector selector in partyMemberSelectorsRef)
            {
                AssetReference assetRef = selector.Get();
                if (assetRef.IsValid())
                {
                    var op = assetRef.InstantiateAsync(partyHolder);
                    op.Completed += (handle) =>
                    {
                        handle.Result.GetComponent<EnemyBattleBehaviour>().InitEnemy(0);
                    };
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