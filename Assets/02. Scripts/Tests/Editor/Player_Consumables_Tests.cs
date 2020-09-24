using NUnit.Framework;
using Laresistance.Core;

namespace Laresistance.Tests
{
    public class Player_Consumables_Tests
    {
        [Test]
        public void When_AddingConsumable()
        {
            Player player = new Player();
            Consumable consumable = new Consumable();
            bool added = player.AddConsumable(consumable);
            Assert.AreEqual(true, added);
            Assert.AreEqual(1, player.ConsumablesAmount);
        }

        [Test]
        public void When_AddingSameConsumableTwoTimes()
        {
            Player player = new Player();
            Consumable consumable = new Consumable();
            bool added = player.AddConsumable(consumable);
            Assert.AreEqual(true, added);
            added = player.AddConsumable(consumable);
            Assert.AreEqual(false, added);
            Assert.AreEqual(1, player.ConsumablesAmount);
        }

        [Test]
        public void When_AddingMaxConsumables()
        {
            Player player = new Player();
            Consumable consumable1 = new Consumable();
            bool added = player.AddConsumable(consumable1);
            Assert.AreEqual(true, added);
            Consumable consumable2 = new Consumable();
            added = player.AddConsumable(consumable2);
            Assert.AreEqual(true, added);
            Consumable consumable3 = new Consumable();
            added = player.AddConsumable(consumable3);
            Assert.AreEqual(true, added);
            Assert.AreEqual(3, player.ConsumablesAmount);
        }

        [Test]
        public void When_AddingMoreThanMaxConsumables()
        {
            Player player = new Player();
            Consumable consumable1 = new Consumable();
            bool added = player.AddConsumable(consumable1);
            Assert.AreEqual(true, added);
            Consumable consumable2 = new Consumable();
            added = player.AddConsumable(consumable2);
            Assert.AreEqual(true, added);
            Consumable consumable3 = new Consumable();
            added = player.AddConsumable(consumable3);
            Assert.AreEqual(true, added);
            Consumable consumable4 = new Consumable();
            added = player.AddConsumable(consumable4);
            Assert.AreEqual(false, added);
            Assert.AreEqual(3, player.ConsumablesAmount);
        }
        
        [Test]
        public void When_AddingConsumableAndDisposingFromReference()
        {
            Player player = new Player();
            Consumable consumable = new Consumable();
            bool added = player.AddConsumable(consumable);
            Assert.AreEqual(true, added);
            bool disposed = player.DisposeConsumable(consumable);
            Assert.AreEqual(true, disposed);
            Assert.AreEqual(0, player.ConsumablesAmount);
        }
        
        [Test]
        public void When_AddingConsumableAndDisposingFromIndex()
        {
            Player player = new Player();
            Consumable consumable = new Consumable();
            bool added = player.AddConsumable(consumable);
            Assert.AreEqual(true, added);
            bool disposed = player.DisposeConsumable(0);
            Assert.AreEqual(true, disposed);
            Assert.AreEqual(0, player.ConsumablesAmount);
        }
        
        [Test]
        public void When_AddingConsumableAndDisposingFromInvalidIndexOrReference()
        {
            Player player = new Player();
            Consumable consumable = new Consumable();
            bool added = player.AddConsumable(consumable);
            Assert.AreEqual(true, added);
            Assert.Catch(() => { player.DisposeConsumable(2); });
            Assert.Catch(()=> { player.DisposeConsumable(-1); });
            Assert.Catch(() => { player.DisposeConsumable(null); });
            Consumable consumable2 = new Consumable();
            Assert.Catch(() => { player.DisposeConsumable(consumable2); });
            Assert.AreEqual(1, player.ConsumablesAmount);
        }
        
        [Test]
        public void When_AddingConsumableAndUsing()
        {
            Player player = new Player();
            Consumable consumable = new Consumable();
            bool added = player.AddConsumable(consumable);
            Assert.AreEqual(true, added);
            bool used = player.UseConsumable(consumable);
            Assert.AreEqual(true, used);
            Assert.AreEqual(0, player.ConsumablesAmount);
        }
    }
}