﻿using Laresistance.Battle;
using System.Collections;
using Laresistance.Data;
using Laresistance.Behaviours;
using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using System.Text;

namespace Laresistance.Core
{
    public class Minion : ShowableElement, ISlot
    {
        private static int MAX_MINION_LEVEL = 10;
        public string Name { get { return Texts.GetText(Data.NameRef); }}
        public MinionData Data { get; private set; }
        private BattleAbility[] abilities = default;
        private EquipmentsContainer equipmentsContainer;
        public BattleStatusManager StatusManager;
        public int Level { get; set; }

        public Minion(MinionData data, BattleAbility[] abilities, int level, BattleStatusManager statusManager)
        {
            Data = data;
            if (abilities == null)
                throw new System.Exception("A minion should have an ability");
            this.abilities = abilities;
            foreach(var ability in abilities)
            {
                ability.SetParentMinion(this);
            }
            if (level <= 0 || level > MAX_MINION_LEVEL)
                throw new System.Exception("A minion level must be at least 1 and " + MAX_MINION_LEVEL + " at max");
            Level = level;
            this.StatusManager = statusManager;
        }

        public void SetEquipmentsContainer(EquipmentsContainer equipments)
        {
            equipmentsContainer = equipments;
            foreach (var ability in abilities)
            {
                ability.SetEquipmentsContainer(equipments);
            }
        }

        public void SetStatusManager(BattleStatusManager selfStatus)
        {
            StatusManager = selfStatus;
            foreach (var ability in abilities)
            {
                ability.SetStatusManager(selfStatus);
            }
        }

        public int GetEffectPower(int index)
        {
            return 0;
        }

        public bool SetInSlot(Player player, int slot)
        {
            return player.EquipMinion(this);
        }

        public void SetLevel(int level)
        {
            Level = level;
        }

        public IEnumerator ExecuteAbility(int skillIndex, BattleStatusManager[] allies, BattleStatusManager[] enemies, IBattleAnimator animator, ScriptableIntReference bloodRef)
        {
            yield return abilities[skillIndex].ExecuteAbility(allies, enemies, Level, animator, bloodRef);
        }

        public BattleAbility[] Abilities => abilities;
        public string GetAbilityText()
        {
            //return Abilities[0].GetAbilityText(Level);
            StringBuilder builder = new StringBuilder();
            foreach (var ability in Abilities)
            {
                builder.Append("- ");
                builder.Append(ability.GetShortAbilityText(Level));
                builder.Append("\n");
            }
            return builder.ToString();
        }
        public string GetNextLevelAbilityText()
        {
            //return Abilities[0].GetAbilityText(Level+1);
            StringBuilder builder = new StringBuilder();
            foreach (var ability in Abilities)
            {
                builder.Append("- ");
                builder.Append(ability.GetShortAbilityText(Level+1));
                builder.Append("\n");
            }
            return builder.ToString();
        }

        public int GetUpgradeCost(EquipmentsContainer equipments)
        {
            //return Data.BaseBloodPrice + Data.BaseBloodPrice / 10 * (Level-1);
            int cost = Data.BaseBloodPrice * Level;
            cost = equipments.ModifyValue(Equipments.EquipmentSituation.UpgradePrice, cost);
            return cost;
        }

        public int GetFullCost(EquipmentsContainer equipments)
        {
            int cost = Data.BaseBloodPrice;
            for (int i = 1; i <= Level; ++i)
            {
                cost += Data.BaseBloodPrice * i;
            }
            //cost = equipments.ModifyValue(Equipments.EquipmentSituation.UpgradePrice, cost);
            return cost;
        }

        public bool Upgrade()
        {
            if (CanUpgrade())
            {
                Level++;
                return true;
            }
            return false;
        }

        public bool CanUpgrade()
        {
            return Level < MAX_MINION_LEVEL;
        }

        public void CutTo4Abilities()
        {
            if (abilities.Length != 4)
            {
                BattleAbility[] tempAbilities = new BattleAbility[4];
                for (int i = 0; i < 4; ++i)
                {
                    tempAbilities[i] = abilities[i];
                }
                abilities = tempAbilities;
            }
        }
    } 
}