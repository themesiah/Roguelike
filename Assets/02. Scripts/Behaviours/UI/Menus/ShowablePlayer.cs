﻿using UnityEngine;
using UnityEngine.UI;
using Laresistance.Core;
using GamedevsToolbox.ScriptableArchitecture.Pools;

namespace Laresistance.Behaviours
{
    public class ShowablePlayer : MonoBehaviour, IShowableGameElement
    {
        [SerializeField]
        private ShowableMinion[] minionElements = default;
        [SerializeField]
        private ShowableAbility[] abilityElements = default;
        [SerializeField]
        private ShowableEquipment[] equipmentElements = default;
        [SerializeField]
        private Transform comboHolder = default;
        [SerializeField]
        private ScriptablePool comboSlotPool = default;
        [SerializeField]
        private RectTransform portraitParent = default;
        [SerializeField]
        private Text playerName = default;
        [SerializeField]
        private Text playerLevel = default;

        public void SetupShowableElement(ShowableElement showableElement)
        {
            Player player = (Player)showableElement;

            var minions = player.GetMinions();

            for (int i = 0; i < minionElements.Length; ++i)
            {
                minionElements[i].SetupShowableElement(minions[i]);
            }

            if (portraitParent != null)
            {
                //Portrait
            }

            if (playerName != null)
            {
                playerName.text = "Player";
            }

            for (int i = 0; i < abilityElements.Length; ++i)
            {
                abilityElements[i].SetupShowableElement(player.characterAbilities[i]);
            }

            for (int i = 0; i < equipmentElements.Length; ++i)
            {
                equipmentElements[i].SetupShowableElement(player.GetEquipments()[i]);
            }

            if (comboHolder != null && comboSlotPool != null)
            {
                comboSlotPool.InitPool();
                comboSlotPool.SoftFreeAll();
                for (int i = 0; i < player.combos.Length; ++i)
                {
                    GameObject go = comboSlotPool.GetInstance(comboHolder);
                    go.GetComponent<ShowableCombo>().SetupShowableElement(player.combos[i]);
                    go.transform.localScale = Vector3.one;
                }
            }

            if (playerLevel != null)
            {
                playerLevel.text = player.Level.ToString();
            }
        }
    }
}