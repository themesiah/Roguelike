using NUnit.Framework;
using Laresistance.Core;
using UnityEngine;
using System.Collections.Generic;

namespace Laresistance.Tests
{
    public class Player_Equipment_Events_Test
    {
        [Test]
        public void When_RegisteringEquipmentEvent()
        {
            Equipment e = new Equipment(0, null, null);
            EquipmentEvents events = new EquipmentEvents();
            e.SetPowerModifier(events, 1f);
            e.EquipEquipment();
        }

        [Test]
        public void When_GettingModifiedPower()
        {
            TestEquipmentWithEvent(10, 1f, 10f);
            TestEquipmentWithEvent(10, 2f, 20f);
            TestEquipmentWithEvent(10, 0.5f, 5f);
        }

        private void TestEquipmentWithEvent(int initialPower, float modifier, float expected)
        {
            Equipment e = new Equipment(0, null, null);
            EquipmentEvents events = new EquipmentEvents();

            int power = initialPower;
            e.SetPowerModifier(events, modifier);
            e.EquipEquipment();
            events.OnGetPower(ref power);
            Assert.AreEqual(expected, power);
            e.UnequipEquipment();
        }

        [Test]
        public void When_GettingModifiedPowerMultipleEquipments()
        {
            EquipmentEvents events = new EquipmentEvents();
            TestMultipleEquipmentWithEvent(events, 10, new float[] { 1f, 1f, 1f }, 10f);
            TestMultipleEquipmentWithEvent(events, 10, new float[] { 2f, 2f }, 40f);
            TestMultipleEquipmentWithEvent(events, 10, new float[] { 0.5f, 0.5f }, 3f);
        }

        private void TestMultipleEquipmentWithEvent(EquipmentEvents events, int initialPower, float[] modifiers, float expected)
        {
            int power = initialPower;
            List<Equipment> equips = new List<Equipment>();
            foreach (float modifier in modifiers)
            {
                Equipment e = new Equipment(0, null, null);
                e.SetPowerModifier(events, modifier);
                e.EquipEquipment();
                equips.Add(e);
            }
            events.OnGetPower(ref power);
            Assert.AreEqual(expected, power);
            equips.ForEach((e) => { e.UnequipEquipment(); });
        }
    }
}