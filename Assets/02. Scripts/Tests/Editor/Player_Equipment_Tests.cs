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
            Equipment equipment = new Equipment(0, null, null);
            bool equipped = player.EquipEquipment(equipment);
            Assert.AreEqual(true, equipped);
            Assert.AreEqual(1, player.EquippedEquipmentsQuantity);
        }

        [Test]
        public void When_EquippingTwoEquipmentsOfSameSlot()
        {
            Player player = new Player(new Battle.BattleStatusManager(new Battle.CharacterHealth(100)));
            Equipment equipment = new Equipment(0, null, null);
            bool equipped = player.EquipEquipment(equipment);
            Assert.AreEqual(true, equipped);
            Equipment equipment2 = new Equipment(0, null, null);
            Assert.Catch(()=>player.EquipEquipment(equipment2));
            Assert.AreEqual(1, player.EquippedEquipmentsQuantity);
        }

        [Test]
        public void When_EquippingTwoEquipmentsOfDifferentSlot()
        {
            Player player = new Player(new Battle.BattleStatusManager(new Battle.CharacterHealth(100)));
            Equipment equipment = new Equipment(0, null, null);
            bool equipped = player.EquipEquipment(equipment);
            Assert.AreEqual(true, equipped);
            Equipment equipment2 = new Equipment(1, null, null);
            equipped = player.EquipEquipment(equipment2);
            Assert.AreEqual(true, equipped);
            Assert.AreEqual(2, player.EquippedEquipmentsQuantity);
        }

        [Test]
        public void When_EquippingEquipmentAndUnequip()
        {
            Player player = new Player(new Battle.BattleStatusManager(new Battle.CharacterHealth(100)));
            Equipment equipment = new Equipment(0, null, null);
            bool equipped = player.EquipEquipment(equipment);
            Assert.AreEqual(true, equipped);
            bool unequiped = player.UnequipEquipment(equipment);
            Assert.AreEqual(true, unequiped);
            Assert.AreEqual(0, player.EquippedEquipmentsQuantity);
        }

        [Test]
        public void When_EquippingEquipmentAndTryToUnequipNonEquippedEquipmentOfSameSlot()
        {
            Player player = new Player(new Battle.BattleStatusManager(new Battle.CharacterHealth(100)));
            Equipment equipment = new Equipment(0, null, null);
            bool equipped = player.EquipEquipment(equipment);
            Assert.AreEqual(true, equipped);
            Equipment equipment2 = new Equipment(0, null, null);
            Assert.Catch(()=> { player.UnequipEquipment(equipment2); });
            Assert.AreEqual(1, player.EquippedEquipmentsQuantity);
        }

        [Test]
        public void When_EquippingEquipmentAndTryToUnequipNonEquippedEquipmentOfDifferentSlot()
        {
            Player player = new Player(new Battle.BattleStatusManager(new Battle.CharacterHealth(100)));
            Equipment equipment = new Equipment(0, null, null);
            bool equipped = player.EquipEquipment(equipment);
            Assert.AreEqual(true, equipped);
            Equipment equipment2 = new Equipment(1, null, null);
            Assert.Catch(()=> { player.UnequipEquipment(equipment2); });
            Assert.AreEqual(1, player.EquippedEquipmentsQuantity);
        }

        [Test]
        public void When_EquippingMaxEquipments()
        {
            Player player = new Player(new Battle.BattleStatusManager(new Battle.CharacterHealth(100)));
            Equipment equipment = new Equipment(0, null, null);
            bool equipped = player.EquipEquipment(equipment);
            Assert.AreEqual(true, equipped);
            Equipment equipment2 = new Equipment(1, null, null);
            equipped = player.EquipEquipment(equipment2);
            Assert.AreEqual(true, equipped);
            Equipment equipment3 = new Equipment(2, null, null);
            equipped = player.EquipEquipment(equipment3);
            Assert.AreEqual(true, equipped);
            Equipment equipment4 = new Equipment(3, null, null);
            equipped = player.EquipEquipment(equipment4);
            Assert.AreEqual(true, equipped);
            Assert.AreEqual(4, player.EquippedEquipmentsQuantity);
        }

        [Test]
        public void When_TryingToEquipEquipmentWithIncorrectSlot()
        {
            Player player = new Player(new Battle.BattleStatusManager(new Battle.CharacterHealth(100)));
            Equipment equipment = new Equipment(-1, null, null);
            Assert.Catch(()=>player.EquipEquipment(equipment));
            Equipment equipment2 = new Equipment(4, null, null);
            Assert.Catch(()=>player.EquipEquipment(equipment));
            Assert.AreEqual(0, player.EquippedEquipmentsQuantity);
        }
    }
}