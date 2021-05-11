using UnityEngine;

namespace Laresistance.Behaviours
{
    public class MaterialValueChanger : MonoBehaviour
    {
        [SerializeField]
        private Renderer rendererToChange = default;
        [SerializeField]
        private string variableName = default;

        private Material mat;

        private void Awake()
        {
            if (rendererToChange != null)
            {
                mat = rendererToChange.material;
            }
        }

        public void ChangeVariableFloatValue(float value)
        {
            mat?.SetFloat(variableName, value);
        }

        public void ChangeVariableIntValue(int value)
        {
            mat?.SetInt(variableName, value);
        }
    }
}