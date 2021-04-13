using UnityEngine;

namespace Laresistance.Audio
{
    [CreateAssetMenu(menuName = "Laresistance/Audio/FMOD Global Parameter")]
    public class ScriptableFMODGlobalParameter : ScriptableObject
    {
        [FMODUnity.ParamRef]
        [SerializeField]
        private string parameterPath = default;

        private FMOD.Studio.PARAMETER_DESCRIPTION parameterDescription;

        private void Init()
        {
            if (string.IsNullOrEmpty(parameterDescription.name))
            {
                UnityEngine.Assertions.Assert.IsTrue(!string.IsNullOrEmpty(parameterPath));
                FMOD.RESULT result = FMODUnity.RuntimeManager.StudioSystem.getParameterDescriptionByName(parameterPath, out parameterDescription);
                UnityEngine.Assertions.Assert.IsTrue(result == FMOD.RESULT.OK);
            }
        }

        public void TriggerParameter(float value)
        {
            Init();
            FMOD.RESULT result = FMODUnity.RuntimeManager.StudioSystem.setParameterByID(parameterDescription.id, value);
            if (result != FMOD.RESULT.OK)
            {
                Debug.LogError(string.Format(("[FMOD] StudioGlobalParameterTrigger failed to set parameter {0} : result = {1}"), parameterPath, result));
            }
        }
    }
}