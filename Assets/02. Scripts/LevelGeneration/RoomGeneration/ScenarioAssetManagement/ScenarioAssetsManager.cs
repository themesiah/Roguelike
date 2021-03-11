using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.LevelGeneration
{
    [System.Serializable]
    public class ScenarioAssetsManager
    {   
        [Tooltip("Conexión entre partes izquierda y derecha, hace el cambio de inclinación")]
        public ScenarioAssetData middleGround;
        [Tooltip("Una sola rachola de suelo que va a los lados del middle ground")]
        public ScenarioAssetData groundTile;
        [Tooltip("Conexión entre partes izquierda y derecha de una plataforma blanda, hace el cambio de inclinación")]
        public ScenarioAssetData platform;
        [Tooltip("Una sola rachola de plataforma blanda que va a los lados de platform")]
        public ScenarioAssetData platformTile;
        [Tooltip("Decoración del final de una plataforma blanda")]
        public ScenarioAssetData platformEnd;
        [Tooltip("Conexión entre partes izquierda y derecha de una plataforma dura, hace el cambio de inclinación")]
        public ScenarioAssetData hardPlatform;
        [Tooltip("Una sola rachola de plataforma dura que va a los lados de hardPlatform")]
        public ScenarioAssetData hardPlatformTile;
        [Tooltip("Decoración del final de una plataforma dura")]
        public ScenarioAssetData hardPlatformEnd;
        [Tooltip("Escaleras flotantes")]
        public ScenarioAssetData floatingStairs;
        [Tooltip("Pared")]
        public ScenarioAssetData walls;
        [Tooltip("Techo")]
        public ScenarioAssetData ceiling;
        [Tooltip("Union de suelo con pared")]
        public ScenarioAssetData wallBase;
        [Tooltip("Union de techo con pared")]
        public ScenarioAssetData wallCorner;
    }
}