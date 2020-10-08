using Laresistance.Core;
using Laresistance.Systems;
using NUnit.Framework;
using GamedevsToolbox.ScriptableArchitecture.Values;
using System.Collections.Generic;
using Laresistance.Battle;
using Laresistance.Data;

namespace Laresistance.Tests
{
    public class Systems_Shop_Buy_Tests
    {
        private static BattleStatusManager GetStatus()
        {
            return new BattleStatusManager(new CharacterHealth(100));
        }

        private static BattleAbility GetAbility()
        {
            List<BattleEffect> effects = new List<BattleEffect>();
            effects.Add(new BattleEffectDamage(1, Data.EffectTargetType.Enemy, GetStatus()));
            return new BattleAbility(effects, 0f);
        }

        private List<ShopOffer> offers = new List<ShopOffer>() {
            new ShopOffer(1000, false, new Minion("", GetAbility(), 1)),
            new ShopOffer(500, false, new Minion("", GetAbility(), 1)),
            new ShopOffer(50, true, new Equipment(0)),
            new ShopOffer(5, true, new Consumable())
        };


        private ShopSystem GetTestShop(int startingBlood, int startingHardCurrency)
        {
            ShopSystem shop = new ShopSystem();
            var blood = GetCurrencyWithValue(startingBlood);
            var hardCurrency = GetCurrencyWithValue(startingHardCurrency);
            shop.SetCurrencyReference(blood, hardCurrency);
            foreach(var offer in offers)
            {
                shop.AddOffer(offer);
            }
            return shop;
        }

        private ScriptableIntReference GetCurrencyWithValue(int value)
        {
            var currencyRef = new ScriptableIntReference();
            ScriptableIntValue val = ScriptableIntValue.CreateInstance<ScriptableIntValue>();
            currencyRef.SetScriptableValueManually(val);
            currencyRef.SetValue(value);
            Assert.AreEqual(value, currencyRef.GetValue());
            return currencyRef;
        }

        [Test]
        public void When_BuyingOfferCorrect()
        {
            var shop = GetTestShop(1000, 0);
            ISlot slotable = shop.BuyOffer(offers[0]);
            Assert.NotNull(slotable);
        }

        [Test]
        public void When_BuyingOfferIncorrect()
        {
            var shop = GetTestShop(500, 5000);
            ISlot slotable = shop.BuyOffer(offers[0]);
            Assert.IsNull(slotable);
        }

        [Test]
        public void When_BuyingOfferCorrectAndEquipping()
        {
            var shop = GetTestShop(1000, 0);
            Player player = new Player();
            ISlot slotable = shop.BuyOffer(offers[0]);
            Assert.NotNull(slotable);
            bool equipped = slotable.SetInSlot(player);
            Assert.IsTrue(equipped);
        }

        [Test]
        public void When_BuyingMultipleSoftOffersCorrect()
        {
            var shop = GetTestShop(1500, 0);
            ISlot slotable = shop.BuyOffer(offers[0]);
            Assert.NotNull(slotable);
            slotable = shop.BuyOffer(offers[1]);
            Assert.NotNull(slotable);
        }

        [Test]
        public void When_BuyingMultipleSoftOffersIncorrect()
        {
            var shop = GetTestShop(1300, 0);
            ISlot slotable = shop.BuyOffer(offers[0]);
            Assert.NotNull(slotable);
            slotable = shop.BuyOffer(offers[1]);
            Assert.IsNull(slotable);
        }

        [Test]
        public void When_BuyingMultipleTimesSameOffer()
        {
            var shop = GetTestShop(1300, 0);
            ISlot slotable = shop.BuyOffer(offers[0]);
            Assert.NotNull(slotable);
            slotable = shop.BuyOffer(offers[0]);
            Assert.IsNull(slotable);
        }

        [Test]
        public void When_BuyingMultipleHardOffersCorrect()
        {
            var shop = GetTestShop(0, 100);
            ISlot slotable = shop.BuyOffer(offers[2]);
            Assert.NotNull(slotable);
            slotable = shop.BuyOffer(offers[3]);
            Assert.NotNull(slotable);
        }

        [Test]
        public void When_BuyingMultipleHardOffersIncorrect()
        {
            var shop = GetTestShop(0, 50);
            ISlot slotable = shop.BuyOffer(offers[2]);
            Assert.NotNull(slotable);
            slotable = shop.BuyOffer(offers[3]);
            Assert.IsNull(slotable);
        }

        [Test]
        public void When_BuyingMixedOffersCorrect()
        {
            var shop = GetTestShop(1000, 50);
            ISlot slotable = shop.BuyOffer(offers[0]);
            Assert.NotNull(slotable);
            slotable = shop.BuyOffer(offers[2]);
            Assert.NotNull(slotable);
        }
    }
}
