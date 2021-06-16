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

        public void SpawnParty(int level)
        {
            int index = 1;
            foreach(ScriptableAssetReferenceSelector selector in partyMemberSelectorsRef)
            {
                int i = index;
                AssetReference assetRef = selector.Get();
                if (!string.IsNullOrEmpty((string)assetRef.RuntimeKey))
                {
                    var op = assetRef.InstantiateAsync(partyHolder);
                    op.Completed += (handle) =>
                    {
                        GameObject go = handle.Result;
                        UnityEngine.Assertions.Assert.IsNotNull(go);
                        go.transform.localPosition = Vector3.zero - Vector3.right * GameConstantsBehaviour.Instance.enemyMapPartyOffset.GetValue() * i;
                        go.GetComponent<EnemyBattleBehaviour>().InitEnemy(level);
                    };
                }
                index++;
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