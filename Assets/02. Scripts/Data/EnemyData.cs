﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Data
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Enemy")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField]
        private string nameRef = default;
        public string NameRef { get { return nameRef; } }

        [SerializeField]
        private int maxHealth = default;
        public int MaxHealth { get { return maxHealth; } }

        [SerializeField]
        private GameObject prefab = default;
        public GameObject Prefab { get { return prefab; } }

        [SerializeField]
        private AbilityData[] abilitiesData = default;
        public AbilityData[] AbilitiesData { get { return abilitiesData; } }
    }
}