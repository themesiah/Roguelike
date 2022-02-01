using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Systems
{
    [System.Serializable]
    public class SavedRun
    {
        // seed
        // save game name
        // character (int)
        // shards and their levels
        // character level
        // equipments
        // stats
        // current hp
        // current level (biome 1, 2, 3, 4 or boss 1, 2, 3, 4)
        // enemies defeated in the map
        // obtained equipment in the map
        // used altars, sanctuary, nests in the map
        // position in the map

        [System.Serializable]
        public struct MinionSerializedData
        {
            public string minionId;
            public int minionLevel;
        }

        public int seed = 0;
        public string name = "save";
        public int characterType = 0;
        public int characterLevel = 1;
        public List<MinionSerializedData> minions = new List<MinionSerializedData>();
        public List<MinionSerializedData> minionsReserve = new List<MinionSerializedData>();
        public List<string> equipments = new List<string>();
        public List<int> stats = new List<int>();
        public int currentHp;
        public int currentLevel;

        // TODO: enemies defeated, equipment obtained, used interactables, position
    }
}