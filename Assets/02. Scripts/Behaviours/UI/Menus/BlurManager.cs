using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Sets;

namespace Laresistance.Behaviours
{
    public class BlurManager : MonoBehaviour
    {
        [SerializeField]
        private RuntimeSingleCamera blurCameraRef = default;

        [SerializeField]
        private Material blurMaterial = default;

        private void Start()
        {
            if (blurCameraRef.Get().targetTexture != null)
            {
                blurCameraRef.Get().targetTexture.Release();
            }
            blurCameraRef.Get().targetTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32, 1);
            blurMaterial.SetTexture("_RenTex", blurCameraRef.Get().targetTexture);
        }
    }
}