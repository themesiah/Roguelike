using UnityEngine;
using System.Collections;
using GamedevsToolbox.ScriptableArchitecture.Values;
using System.Collections.Generic;

namespace Laresistance.Behaviours
{
    public class DisintegrationEffectBehaviour : MonoBehaviour
    {
        [SerializeField]
        private ScriptableFloatReference disintegrationTime = default;
        [SerializeField]
        private List<Material> disintegrationMaterials;

        private void Start()
        {
            disintegrationMaterials = new List<Material>();
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach(Renderer r in renderers)
            {
                disintegrationMaterials.Add(r.material);
            }
        }

        public void StartDisintegration()
        {
            StartCoroutine(DisintegrationFade());
        }

        private IEnumerator DisintegrationFade()
        {
            float timer = disintegrationTime.GetValue();
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                foreach (Material disintegrationMaterial in disintegrationMaterials)
                {
                    disintegrationMaterial.SetFloat("_Fade", timer / disintegrationTime.GetValue());
                }
                yield return null;
            }
        }
    }
}