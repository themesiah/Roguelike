﻿using GamedevsToolbox.ScriptableArchitecture.Events;
using Laresistance.Core;
using Laresistance.Data;
using Laresistance.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class MapEquipment : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private RewardEvent rewardEvent = default;
        [SerializeField]
        private RuntimePlayerDataBehaviourSingle playerDataRef = default;
        [SerializeField]
        private EquipmentData data = default;
        [SerializeField]
        private StringGameEvent gameContextSignal = default;
        [SerializeField]
        private EquipmentData[] possibleDatas = default;
        [SerializeField]
        private bool randomDataFromDatas = false;

        public static List<string> GlobalEquipList;

        private void Start()
        {
            if (GlobalEquipList == null)
            {
                GlobalEquipList = new List<string>();
            }
            if (randomDataFromDatas && data == null)
            {
                var data = GetRandomData();
                AddToGlobalEquipList(data);
                SetData(data);
            }
        }

        public static void AddToGlobalEquipList(EquipmentData data)
        {
            if (GlobalEquipList == null)
                GlobalEquipList = new List<string>();
            GlobalEquipList.Add(data.EquipmentNameReference);
        }

        private EquipmentData GetRandomData()
        {
            List<EquipmentData> nonRepeatedDatas = new List<EquipmentData>();
            foreach(var data in possibleDatas)
            {
                if (!GlobalEquipList.Contains(data.EquipmentNameReference))
                {
                    nonRepeatedDatas.Add(data);
                }
            }
            return nonRepeatedDatas[Random.Range(0, nonRepeatedDatas.Count)];
        }

        public void SetData(EquipmentData data)
        {
            this.data = data;
        }

        public void Interact()
        {
            Equipment e = EquipmentFactory.GetEquipment(data, playerDataRef.Get().StatusManager);
            StartCoroutine(RewardCoroutine(e));
        }

        private IEnumerator RewardCoroutine(Equipment e)
        {
            gameContextSignal.Raise("UI");
            yield return rewardEvent?.Raise(new RewardData(0, 0, null, null, e, null, null, null));
            gameContextSignal.Raise("Map");
            Destroy(gameObject);
        }
    }
}