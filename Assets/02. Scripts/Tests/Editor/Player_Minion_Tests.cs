using NUnit.Framework;
using Laresistance.Core;

namespace Laresistance.Tests
{
    public class Player_Minion_Tests
    {
        [Test]
        public void When_EquippingMinionWithNoMinions()
        {
            Player player = new Player();
            Minion minion = new Minion("", null, 1);
            bool equipped = player.EquipMinion(minion);
            Assert.AreEqual(true, equipped);
            Assert.AreEqual(1, player.EquippedMinionsQuantity);
        }

        [Test]
        public void When_Equipping3MinionsWithNoMinions()
        {
            Player player = new Player();
            Minion minion1 = new Minion("", null, 1);
            bool equipped = player.EquipMinion(minion1);
            Assert.AreEqual(true, equipped);
            Minion minion2 = new Minion("", null, 1);
            equipped = player.EquipMinion(minion2);
            Assert.AreEqual(true, equipped);
            Minion minion3 = new Minion("", null, 1);
            equipped = player.EquipMinion(minion3);
            Assert.AreEqual(true, equipped);
            Assert.AreEqual(3, player.EquippedMinionsQuantity);
        }

        [Test]
        public void When_EquippingSameMinionTwoTimes()
        {
            Player player = new Player();
            Minion minion = new Minion("", null, 1);
            bool equipped = player.EquipMinion(minion);
            Assert.AreEqual(true, equipped);
            Assert.Catch(()=>player.EquipMinion(minion));
        }

        [Test]
        public void When_EquippingMoreMinionsThanMaximum()
        {
            Player player = new Player();
            Minion minion1 = new Minion("", null, 1);
            bool equipped = player.EquipMinion(minion1);
            Assert.AreEqual(true, equipped);
            Minion minion2 = new Minion("", null, 1);
            equipped = player.EquipMinion(minion2);
            Assert.AreEqual(true, equipped);
            Minion minion3 = new Minion("", null, 1);
            equipped = player.EquipMinion(minion3);
            Assert.AreEqual(true, equipped);
            Minion minion4 = new Minion("", null, 1);
            equipped = player.EquipMinion(minion4);
            Assert.AreEqual(false, equipped);
            Assert.AreEqual(3, player.EquippedMinionsQuantity);
        }

        [Test]
        public void When_EquippingAndUnequippingMinionWithReference()
        {
            Player player = new Player();
            Minion minion1 = new Minion("", null, 1);
            bool equipped = player.EquipMinion(minion1);
            Assert.AreEqual(true, equipped);
            bool unequipped = player.UnequipMinion(minion1);
            Assert.AreEqual(true, unequipped);
            Assert.AreEqual(0, player.EquippedMinionsQuantity);
        }

        [Test]
        public void When_EquippingAndUnequippingMinionWithIndex()
        {
            Player player = new Player();
            Minion minion1 = new Minion("", null, 1);
            bool equipped = player.EquipMinion(minion1);
            Assert.AreEqual(true, equipped);
            bool unequipped = player.UnequipMinion(0);
            Assert.AreEqual(true, unequipped);
            Assert.AreEqual(0, player.EquippedMinionsQuantity);
        }

        [Test]
        public void When_EquippingMinionAndTryingToUnequipInvalidReferenceOrIndex()
        {
            Player player = new Player();
            Minion minion1 = new Minion("", null, 1);
            bool equipped = player.EquipMinion(minion1);
            Assert.AreEqual(true, equipped);
            Assert.Catch(()=>player.UnequipMinion(1));
            Assert.Catch(()=>player.UnequipMinion(-1));
            Assert.Catch(()=>player.UnequipMinion(null));
            Assert.AreEqual(1, player.EquippedMinionsQuantity);
        }

        [Test]
        public void When_EquippingMinionAndTryingToUnequipInvalidReferenceOrIndexAndEquippingAgain()
        {
            Player player = new Player();
            Minion minion1 = new Minion("", null, 1);
            bool equipped = player.EquipMinion(minion1);
            Assert.AreEqual(true, equipped);
            Assert.Catch(()=>player.UnequipMinion(1));
            Assert.Catch(()=>player.UnequipMinion(null));
            Assert.Catch(()=>player.EquipMinion(minion1));
            Assert.AreEqual(1, player.EquippedMinionsQuantity);
        }

        [Test]
        public void When_EquippingAllMaximumMinionsAndUnequippingOneToEquipAnotherNewOne()
        {
            Player player = new Player();
            Minion minion1 = new Minion("", null, 1);
            bool equipped = player.EquipMinion(minion1);
            Assert.AreEqual(true, equipped);
            Minion minion2 = new Minion("", null, 1);
            equipped = player.EquipMinion(minion2);
            Assert.AreEqual(true, equipped);
            Minion minion3 = new Minion("", null, 1);
            equipped = player.EquipMinion(minion3);
            Assert.AreEqual(true, equipped);
            player.UnequipMinion(1);
            Minion minion4 = new Minion("", null, 1);
            equipped = player.EquipMinion(minion4);
            Assert.AreEqual(true, equipped);
            Assert.AreEqual(3, player.EquippedMinionsQuantity);
        }
    }
}
