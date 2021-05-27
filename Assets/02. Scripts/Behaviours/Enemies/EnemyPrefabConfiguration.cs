﻿using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Laresistance.Behaviours
{
    public class EnemyPrefabConfiguration : MonoBehaviour
    {
        public void ConfigurePrefab(AssetReference prefabRef, UnityAction<AnimatorWrapperBehaviour> animatorSetup, bool partyMember)
        {
            prefabRef.InstantiateAsync(transform).Completed += (handler) => {
                GameObject go = handler.Result;
                AnimatorWrapperBehaviour anim = go.GetComponent<AnimatorWrapperBehaviour>();
                Assert.IsNotNull(anim);
                animatorSetup?.Invoke(anim);
                if (partyMember)
                {
                    Flipper flipper = go.GetComponent<Flipper>();
                    if (flipper != null)
                    {
                        Destroy(flipper);
                    }
                }
            };
        }
    }
}