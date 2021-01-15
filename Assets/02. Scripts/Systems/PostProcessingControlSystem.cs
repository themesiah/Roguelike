using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Laresistance.Systems
{
    [CreateAssetMenu(menuName = "Laresistance/Systems/Post Processing Control")]
    public class PostProcessingControlSystem : ScriptableObject
    {
        private ChromaticAberration ca;

        public void ActivateChromaticAberration(VolumeProfile volumeProfile)
        {
            volumeProfile.TryGet(out ca);
            if (ca != null)
            {
                ca.active = true;
            }
        }

        public void DeactivateChromaticAberration(VolumeProfile volumeProfile)
        {
            volumeProfile.TryGet(out ca);
            if (ca != null)
            {
                ca.active = false;
            }
        }
    }
}