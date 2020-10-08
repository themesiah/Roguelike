using Laresistance.Battle;

namespace Laresistance.Core
{
    public class Player
    {
        private static int MAX_MINIONS = 3;
        private static int MAX_EQUIPS = 4;
        private static int MAX_CONSUMABLES = 3;

        private Minion[] minions;
        private Equipment[] equipments;
        private Consumable[] consumables;
        private EquipmentEvents equipmentEvents = null;
        public BattleAbility characterAbility { get; private set; }

        public Player()
        {
            minions = new Minion[MAX_MINIONS];
            equipments = new Equipment[MAX_EQUIPS];
            consumables = new Consumable[MAX_CONSUMABLES];
            InitEquipmentEvents();
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
            }

            for(int i = 0; i < minions.Length; ++i)
            {
                if (minions[i] == null)
                {
                    minions[i] = minion;
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
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Equipments
        public EquipmentEvents GetEquipmentEvents()
        {
            return equipmentEvents;
        }

        public int EquippedEquipmentsQuantity
        {
            get
            {
                int equipmentQuantity = 0;
                foreach (Equipment e in equipments)
                {
                    if (e != null)
                    {
                        equipmentQuantity++;
                    }
                }
                return equipmentQuantity;
            }
        }

        private void InitEquipmentEvents()
        {
            if (equipmentEvents == null)
            {
                equipmentEvents = new EquipmentEvents();
            }
        }

        public bool EquipEquipment(Equipment equipment)
        {
            if (equipment == null || equipment.Slot == -1 || equipments[equipment.Slot] != null)
                throw new System.Exception("Can't equip. Equipment does not exist, or invalid slot");
            equipments[equipment.Slot] = equipment;
            equipment.EquipEquipment();
            return true;
        }

        public bool UnequipEquipment(Equipment equipment)
        {
            if (equipment == null || equipment.Slot == -1 || equipments[equipment.Slot] != equipment)
                throw new System.Exception("Can't unequip. Equipment does not exist, or invalid slot");
            equipments[equipment.Slot] = null;
            equipment.UnequipEquipment();
            return true;
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
                    consumables[i].Use();
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
            BattleAbility[] abilities = new BattleAbility[MAX_MINIONS + MAX_CONSUMABLES + 1];
            abilities[0] = characterAbility;
            for(int i = 0; i < MAX_MINIONS; ++i)
            {
                if (minions.Length > i && minions[i] != null)
                {
                    abilities[i + 1] = minions[i].Abilities[0];
                }
            }
            for(int i = 0; i < MAX_CONSUMABLES; ++i)
            {
                if (consumables.Length > i && consumables[i] != null)
                {
                    abilities[i + 1 + MAX_MINIONS] = consumables[i].Ability;
                }
            }
            return abilities;
        }

        public void ResetAbilities()
        {
            foreach(var ability in GetAbilities())
            {
                ability?.ResetTimer();
            }
        }
        #endregion
    }
}