using NUnit.Framework;
using Laresistance.Core;

namespace Laresistance.Tests
{
    public class Player_Equipment_Tests
    {
        [Test]
        public void When_EquippingEquipment()
        {
            Player player = new Player(new Battle.BattleStatusManager(new Battle.CharacterHealth(100)));
            Equipment equipment = new Equipment(null, null);
            bool equipped = player.EquipEquipment(equipment, 0);
            Assert.AreEqual(true, equipped);
            Assert.AreEqual(1, player.EquippedEquipmentsQuantity);
        }

        [Test]
        public void When_EquippingTwoEquipmentsOfSameSlot()
        {
            Player player = new Player(new Battle.BattleStatusManager(new Battle.CharacterHealth(100)));
            Equipment equipment = new Equipment(null, null);
            bool equipped = player.EquipEquipment(equipment, 0);
            Assert.AreEqual(true, equipped);
            Equipment equipment2 = new Equipment(null, null);
            Assert.Catch(()=>player.EquipEquipment(equipment2, 0));
            Assert.AreEqual(1, player.EquippedEquipmentsQuantity);
        }

        [Test]
        public void When_EquippingTwoEquipmentsOfDifferentSlot()
        {
            Player player = new Player(new Battle.BattleStatusManager(new Battle.CharacterHealth(100)));
            Equipment equipment = new Equipment(null, null);
            bool equipped = player.EquipEquipment(equipment, 0);
            Assert.AreEqual(true, equipped);
            Equipment equipment2 = new Equipment(null, null);
            equipped = player.EquipEquipment(equipment2, 1);
            Assert.AreEqual(true, equipped);
            Assert.AreEqual(2, player.EquippedEquipmentsQuantity);
        }

        [Test]
        public void When_EquippingEquipmentAndUnequip()
        {
            Player player = new Player(new Battle.BattleStatusManager(new Battle.CharacterHealth(100)));
            Equipment equipment = new Equipment(null, null);
            bool equipped = player.EquipEquipment(equipment, 0);
            Assert.AreEqual(true, equipped);
            bool unequiped = player.UnequipEquipment(0);
            Assert.AreEqual(true, unequiped);
            Assert.AreEqual(0, player.EquippedEquipmentsQuantity);
        }

        [Test]
        public void When_EquippingEquipmentAndTryToUnequipNonEquippedEquipmentOfSameSlot()
        {
            Player player = new Player(new Battle.BattleStatusManager(new Battle.CharacterHealth(100)));
            Equipment equipment = new Equipment(null, null);
            bool equipped = player.EquipEquipment(equipment, 0);
            Assert.AreEqual(true, equipped);
            Equipment equipment2 = new Equipment(null, null);
            Assert.Catch(()=> { player.UnequipEquipment(0); });
            Assert.AreEqual(1, player.EquippedEquipmentsQuantity);
        }

        [Test]
        public void When_EquippingEquipmentAndTryToUnequipNonEquippedEquipmentOfDifferentSlot()
        {
            Player player = new Player(new Battle.BattleStatusManager(new Battle.CharacterHealth(100)));
            Equipment equipment = new Equipment(null, null);
            bool equipped = player.EquipEquipment(equipment, 0);
            Assert.AreEqual(true, equipped);
            Equipment equipment2 = new Equipment(null, null);
            Assert.Catch(()=> { player.UnequipEquipment(1); });
            Assert.AreEqual(1, player.EquippedEquipmentsQuantity);
        }

        [Test]
        public void When_EquippingMaxEquipments()
        {
            Player player = new Player(new Battle.BattleStatusManager(new Battle.CharacterHealth(100)));
            Equipment equipment = new Equipment(null, null);
            bool equipped = player.EquipEquipment(equipment, 0);
            Assert.AreEqual(true, equipped);
            Equipment equipment2 = new Equipment(null, null);
            equipped = player.EquipEquipment(equipment2, 1);
            Assert.AreEqual(true, equipped);
            Equipment equipment3 = new Equipment(null, null);
            equipped = player.EquipEquipment(equipment3, 2);
            Assert.AreEqual(true, equipped);
            Equipment equipment4 = new Equipment(null, null);
            equipped = player.EquipEquipment(equipment4, 3);
            Assert.AreEqual(true, equipped);
            Assert.AreEqual(4, player.EquippedEquipmentsQuantity);
        }

        [Test]
        public void When_TryingToEquipEquipmentWithIncorrectSlot()
        {
            Player player = new Player(new Battle.BattleStatusManager(new Battle.CharacterHealth(100)));
            Equipment equipment = new Equipment(null, null);
            Assert.Catch(()=>player.EquipEquipment(equipment, -1));
            Equipment equipment2 = new Equipment(null, null);
            Assert.Catch(()=>player.EquipEquipment(equipment, 4));
            Assert.AreEqual(0, player.EquippedEquipmentsQuantity);
        }
    }
}