using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Battle;
using Laresistance.Equipments;
using System.Collections.Generic;

namespace Laresistance.Core
{
    public class Player : ShowableElement
    {
        private static int MAX_MINIONS = 3;
        private static int MAX_CONSUMABLES = 3;

        private Minion[] minions;
        private List<Minion> reservedMinions;
        private Consumable[] consumables;
        private EquipmentsContainer equipmentsContainer;
        public BattleStatusManager statusManager { get; private set; }
        public BattleAbility[] characterAbilities { get; private set; }
        public BattleAbility ultimateAbility { get; private set; }
        public Combo[] combos { get; private set; }

        public Player(BattleStatusManager statusManager)
        {
            minions = new Minion[MAX_MINIONS];
            reservedMinions = new List<Minion>();
            equipmentsContainer = new EquipmentsContainer();
            consumables = new Consumable[MAX_CONSUMABLES];
            this.statusManager = statusManager;
        }

        #region Minions
        public Minion[] GetMinions()
        {
            return minions;
        }

        public int EquippedMinionsQuantity {
            get {
                int minionQuantity = 0;
                foreach(Minion m in minions)
                {
                    if (m != null)
                    {
                        minionQuantity++;
                    }
                }
                return minionQuantity;
            }
        }

        public bool EquipMinion(Minion minion)
        {
            for(int i = 0; i < minions.Length; ++i)
            {
                if (minions[i] == minion)
                {
                    throw new System.Exception("Minion already equipped");
                }
                //if (minions[i] != null && minions[i].Data.name == minion.Data.name)
                //{
                //    if (minion.Level > minions[i].Level)
                //    {
                //        minions[i] = minion;
                //    }
                //    minions[i].Upgrade();
                //    return true;
                //}
            }

            for(int i = 0; i < minions.Length; ++i)
            {
                if (minions[i] == null)
                {
                    minions[i] = minion;
                    minion.SetStatusManager(statusManager);
                    minion.SetEquipmentsContainer(GetEquipmentContainer());
                    return true;
                }
            }
            return false;
        }

        public bool UnequipMinion(int index)
        {
            if (index < 0 || index > MAX_MINIONS-1)
            {
                throw new System.Exception("Invalid minion index");
            }
            return UnequipMinion(minions[index]);
        }

        public bool UnequipMinion(Minion minion)
        {
            if (minion == null)
                throw new System.Exception("Can't unequip null minion");
            for (int i = 0; i < minions.Length; ++i)
            {
                if (minions[i] == minion)
                {
                    minions[i] = null;
                    AddMinionToReserve(minion);
                    return true;
                }
            }
            return false;
        }

        public void AddMinionToReserve(Minion minion)
        {
            if (!reservedMinions.Contains(minion))
                reservedMinions.Add(minion);
        }

        public int ClearMinionReserve()
        {
            int quantity = reservedMinions.Count;
            reservedMinions.Clear();
            return quantity;
        }

        public List<Minion> GetMinionReserve()
        {
            return reservedMinions;
        }
        #endregion

        #region Equipments
        public Equipment[] GetEquipments()
        {
            return equipmentsContainer.Equipments;
        }

        public EquipmentsContainer GetEquipmentContainer()
        {
            return equipmentsContainer;
        }

        public int EquippedEquipmentsQuantity
        {
            get
            {
                int equipmentQuantity = 0;
                foreach (Equipment e in GetEquipments())
                {
                    if (e != null)
                    {
                        equipmentQuantity++;
                    }
                }
                return equipmentQuantity;
            }
        }

        public bool EquipEquipment(Equipment equipment)
        {
            return equipmentsContainer.EquipEquipment(equipment);
        }

        public bool UnequipEquipment(Equipment equipment)
        {
            return equipmentsContainer.UnequipEquipment(equipment);
        }
        #endregion

        #region Consumables
        public Consumable[] GetConsumables()
        {
            return consumables;
        }

        public int ConsumablesAmount
        {
            get
            {
                int consumablesQuantity = 0;
                foreach (Consumable c in consumables)
                {
                    if (c != null)
                    {
                        consumablesQuantity++;
                    }
                }
                return consumablesQuantity;
            }
        }

        public bool AddConsumable(Consumable consumable)
        {
            if (consumable == null)
                throw new System.Exception("Can't add a null consumable");
            for (int i = 0; i < consumables.Length; ++i)
            {
                if (consumables[i] == consumable)
                {
                    return false;
                }
            }

            for (int i = 0; i < consumables.Length; ++i)
            {
                if (consumables[i] == null)
                {
                    consumables[i] = consumable;
                    return true;
                }
            }
            return false;
        }

        public bool UseConsumable(int index)
        {
            if (index < 0 || index > MAX_CONSUMABLES - 1)
            {
                throw new System.Exception("Non valid consumable index");
            }
            return UseConsumable(consumables[index]);
        }

        public bool UseConsumable(Consumable consumable)
        {
            if (consumable == null)
                throw new System.Exception("Can't use a null consumable");
            for (int i = 0; i < consumables.Length; ++i)
            {
                if (consumables[i] == consumable)
                {
                    return DisposeConsumable(consumable);
                }
            }
            return false;
        }

        public bool DisposeConsumable(int index)
        {
            if (index < 0 || index > MAX_CONSUMABLES - 1)
            {
                throw new System.Exception("Non valid consumable index");
            }
            return DisposeConsumable(consumables[index]);
        }

        public bool DisposeConsumable(Consumable consumable)
        {
            if (consumable == null)
                throw new System.Exception("Can't dispose a null consumable");
            for (int i = 0; i < consumables.Length; ++i)
            {
                if (consumables[i] == consumable)
                {
                    consumables[i] = null;
                    return true;
                }
            }
            throw new System.Exception("Can't dispose a not posessed consumable");
        }
        #endregion

        #region Abilities
        public BattleAbility[] GetAbilities()
        {
            BattleAbility[] abilities = new BattleAbility[MAX_MINIONS * 4 + MAX_CONSUMABLES + 4];
            abilities[0] = characterAbilities[0];
            abilities[1] = characterAbilities[1];
            abilities[2] = characterAbilities[2];
            abilities[3] = characterAbilities[3];
            for(int i = 0; i < MAX_MINIONS; ++i)
            {
                if (minions.Length > i && minions[i] != null)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        abilities[i * 4 + k + 4] = minions[i].Abilities[k];
                    }
                }
            }
            for(int i = 0; i < MAX_CONSUMABLES; ++i)
            {
                if (consumables.Length > i && consumables[i] != null)
                {
                    abilities[i + 4 + MAX_MINIONS * 4] = consumables[i].Ability;
                }
            }
            return abilities;
        }

        public BattleAbility[] GetAllAbilities()
        {
            // Length is 4 player abilities, 1 ultimate, 4 per minion (3 minions), 3 consumables, and the combos
            BattleAbility[] abilities = new BattleAbility[MAX_MINIONS * 4 + MAX_CONSUMABLES + 5 + combos.Length];
            // Player abilities. From 0 to 3.
            abilities[0] = characterAbilities[0];
            abilities[1] = characterAbilities[1];
            abilities[2] = characterAbilities[2];
            abilities[3] = characterAbilities[3];
            // Minion abilities. From 4 to 15
            for(int i = 0; i < MAX_MINIONS; ++i)
            {
                if (minions.Length > i && minions[i] != null)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        abilities[i * 4 + k + 4] = minions[i].Abilities[k];
                    }
                }
            }
            // Consumable abilities. From 16 to 18
            for(int i = 0; i < MAX_CONSUMABLES; ++i)
            {
                if (consumables.Length > i && consumables[i] != null)
                {
                    abilities[i + 4 + MAX_MINIONS * 4] = consumables[i].Ability;
                }
            }
            // Ultimate ability. Slot 19
            abilities[MAX_MINIONS * 4 + MAX_CONSUMABLES + 4] = ultimateAbility;
            // Combos. From 20 onward
            for (int i = 0; i < combos.Length; ++i)
            {
                abilities[MAX_MINIONS * 4 + MAX_CONSUMABLES + 5 + i] = combos[i].comboAbility;
            }
            return abilities;
        }

        public void SetMainAbilities(BattleAbility[] abilities, BattleAbility ultimate)
        {
            characterAbilities = abilities;
            ultimateAbility = ultimate;
        }
        #endregion

        #region Combos
        public void SetCombos(Combo[] combos)
        {
            this.combos = combos;
        }
        #endregion
    }
}