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
            new ShopOffer(1000, false, new RewardData(0, 0, new Minion(null, GetAbility(), 1), null, null, null)),
            new ShopOffer(500, false, new RewardData(0, 0, new Minion(null, GetAbility(), 1), null, null, null)),
            new ShopOffer(50, true, new RewardData(0, 0, null, null, new Equipment(0, null), null)),
            new ShopOffer(5, true, new RewardData(0, 0, null, new Consumable(null, GetAbility()), null, null))
        };


        private ShopSystem GetTestShop(int startingBlood, int startingHardCurrency)
        {
            var blood = GetCurrencyWithValue(startingBlood);
            var hardCurrency = GetCurrencyWithValue(startingHardCurrency);
            ShopSystem shop = new ShopSystem(blood, hardCurrency);
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
            RewardData reward = shop.BuyOffer(offers[0]);
            Assert.NotNull(reward);
        }

        [Test]
        public void When_BuyingOfferIncorrect()
        {
            var shop = GetTestShop(500, 5000);
            RewardData reward = shop.BuyOffer(offers[0]);
            Assert.IsNull(reward);
        }

        [Test]
        public void When_BuyingOfferCorrectAndEquipping()
        {
            var shop = GetTestShop(1000, 0);
            Player player = new Player(new Battle.BattleStatusManager(new Battle.CharacterHealth(100)));
            RewardData reward = shop.BuyOffer(offers[0]);
            Assert.NotNull(reward);
            bool equipped = reward.minion.SetInSlot(player);
            Assert.IsTrue(equipped);
        }

        [Test]
        public void When_BuyingMultipleSoftOffersCorrect()
        {
            var shop = GetTestShop(1500, 0);
            RewardData reward = shop.BuyOffer(offers[0]);
            Assert.NotNull(reward);
            reward = shop.BuyOffer(offers[1]);
            Assert.NotNull(reward);
        }

        [Test]
        public void When_BuyingMultipleSoftOffersIncorrect()
        {
            var shop = GetTestShop(1300, 0);
            RewardData reward = shop.BuyOffer(offers[0]);
            Assert.NotNull(reward);
            reward = shop.BuyOffer(offers[1]);
            Assert.IsNull(reward);
        }

        [Test]
        public void When_BuyingMultipleTimesSameOffer()
        {
            var shop = GetTestShop(1300, 0);
            RewardData reward = shop.BuyOffer(offers[0]);
            Assert.NotNull(reward);
            reward = shop.BuyOffer(offers[0]);
            Assert.IsNull(reward);
        }

        [Test]
        public void When_BuyingMultipleHardOffersCorrect()
        {
            var shop = GetTestShop(0, 100);
            RewardData reward = shop.BuyOffer(offers[2]);
            Assert.NotNull(reward);
            reward = shop.BuyOffer(offers[3]);
            Assert.NotNull(reward);
        }

        [Test]
        public void When_BuyingMultipleHardOffersIncorrect()
        {
            var shop = GetTestShop(0, 50);
            RewardData reward = shop.BuyOffer(offers[2]);
            Assert.NotNull(reward);
            reward = shop.BuyOffer(offers[3]);
            Assert.IsNull(reward);
        }

        [Test]
        public void When_BuyingMixedOffersCorrect()
        {
            var shop = GetTestShop(1000, 50);
            RewardData reward = shop.BuyOffer(offers[0]);
            Assert.NotNull(reward);
            reward = shop.BuyOffer(offers[2]);
            Assert.NotNull(reward);
        }
    }
}
