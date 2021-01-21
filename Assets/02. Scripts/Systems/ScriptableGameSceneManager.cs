using Laresistance.Behaviours;
using UnityEngine;

namespace Laresistance.Systems
{
    [CreateAssetMenu(menuName = "Laresistance/Systems/Game Scene Manager")]
    public class ScriptableGameSceneManager : ScriptableObject
    {
        public void RestartGame()
        {
            GameSceneManager gsm = GameSceneManager.Instance;
            gsm.ChangeScene("Scenario2");
        }
    }
}