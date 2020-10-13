using Laresistance.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Battle
{
    public class PlayerMinionCompanionSpawner
    {
        public static float MINION_HORIZONTAL_OFFSET = 2f;
        public static float MINION_VERTICAL_OFFSET = 1f;

        private Player player;
        private List<GameObject> minionObjects;
        public PlayerMinionCompanionSpawner(Player player)
        {
            this.player = player;
            minionObjects = new List<GameObject>();
        }

        public void Spawn(Vector3 center, GameObject playerObject)
        {
            for(int i = 0; i < player.GetMinions().Length; ++i)
            {
                if (player.GetMinions()[i] != null)
                {
                    GameObject go = GameObject.Instantiate(player.GetMinions()[i].Data.Prefab, playerObject.transform);
                    switch (i)
                    {
                        case 0:
                            go.transform.localPosition = go.transform.localPosition + Vector3.right * MINION_HORIZONTAL_OFFSET;
                            break;
                        case 1:
                            go.transform.localPosition = go.transform.localPosition + Vector3.down * MINION_VERTICAL_OFFSET;
                            break;
                        case 2:
                            go.transform.localPosition = go.transform.localPosition + Vector3.left * MINION_HORIZONTAL_OFFSET;
                            break;
                    }
                    minionObjects.Add(go);
                }
            }
        }

        public void Despawn()
        {
            foreach(var go in minionObjects)
            {
                GameObject.Destroy(go);
            }
        }
    }
}