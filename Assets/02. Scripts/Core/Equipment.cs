using UnityEngine.Events;
using UnityEngine;
using Laresistance.Data;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Battle;

namespace Laresistance.Core
{
    public class Equipment : ISlot
    {
        #region Events
        private UnityAction onEquip;
        private UnityAction onUnequip;
        #endregion

        private int slot = -1;
        private bool equiped = false;
        private BattleStatusManager statusManager;
        private List<string> descriptionReferences;
        private List<string> descriptionVariables;
        public EquipmentData Data { get; private set; }
        public string Name => Texts.GetText(Data.EquipmentNameReference);
        public string SlotName => Texts.GetText("EQUIPMENT_SLOT_" + Slot.ToString());
        public int Slot
        {
            get
            {
                return slot;
            }
        }

        public Equipment(int slot, EquipmentData equipmentData, BattleStatusManager statusManager)
        {
            this.slot = slot;
            this.Data = equipmentData;
            this.statusManager = statusManager;
            descriptionReferences = new List<string>();
            descriptionVariables = new List<string>();
        }

        public bool SetInSlot(Player player)
        {
            return player.EquipEquipment(this);
        }

        public void EquipEquipment()
        {
            UnityEngine.Assertions.Assert.IsFalse(equiped);
            equiped = true;
            onEquip?.Invoke();
        }

        public void UnequipEquipment()
        {
            UnityEngine.Assertions.Assert.IsTrue(equiped);
            equiped = false;
            onUnequip?.Invoke();
        }

        public string GetEquipmentEffectDescription()
        {
            StringBuilder builder = new StringBuilder();
            int index = 0;
            foreach(string effectDescriptionFormat in descriptionReferences)
            {
                string format = Texts.GetText(effectDescriptionFormat);
                if (format.Contains("{x}"))
                {
                    format = format.Replace("{x}", "{"+index.ToString()+"}");
                    index++;
                }
                builder.Append(format);
                builder.Append(" ");
            }
            return string.Format(builder.ToString(), (string[])descriptionVariables.ToArray());
        }

        #region EventRegister
        public void SetPowerModifierFlat(EquipmentEvents equipmentEvents, float modifier)
        {
            if (equipmentEvents == null)
                return;
            EquipmentEvents.OnGetPowerHandler handler = ((ref int currentPower) => { currentPower = Mathf.CeilToInt(currentPower + modifier); });
            onEquip += () =>
            {
                equipmentEvents.OnGetPowerFlat += handler;
            };

            onUnequip += () =>
            {
                equipmentEvents.OnGetPowerFlat -= handler;
            };

            if (modifier > 0f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0101-A");
                descriptionVariables.Add(modifier.ToString());
            }
            else if (modifier < 0f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0101-B");
                descriptionVariables.Add((-modifier).ToString());
            }
        }

        public void SetPowerModifier(EquipmentEvents equipmentEvents, float modifier)
        {
            if (equipmentEvents == null)
                return;
            EquipmentEvents.OnGetPowerHandler handler = ((ref int currentPower) => { currentPower = Mathf.CeilToInt(currentPower * modifier); });
            onEquip += () =>
            {
                equipmentEvents.OnGetPower += handler;
            };

            onUnequip += () =>
            {
                equipmentEvents.OnGetPower -= handler;
            };

            if (modifier > 1f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0102-A");
                descriptionVariables.Add(((modifier - 1f) * 100f).ToString());
            } else  if (modifier < 1f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0102-B");
                descriptionVariables.Add(((1f - modifier) * 100f).ToString());
            }
        }

        public void SetAttackPowerModifierFlat(EquipmentEvents equipmentEvents, float modifier)
        {
            if (equipmentEvents == null)
                return;
            EquipmentEvents.OnGetPowerHandler handler = ((ref int currentPower) => { currentPower = Mathf.CeilToInt(currentPower + modifier); });
            onEquip += () =>
            {
                equipmentEvents.OnGetAttackPowerFlat += handler;
            };

            onUnequip += () =>
            {
                equipmentEvents.OnGetAttackPowerFlat -= handler;
            };

            if (modifier > 0f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0103-A");
                descriptionVariables.Add(modifier.ToString());
            }
            else if (modifier < 0f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0103-B");
                descriptionVariables.Add((-modifier).ToString());
            }
        }

        public void SetAttackPowerModifier(EquipmentEvents equipmentEvents, float modifier)
        {
            if (equipmentEvents == null)
                return;
            EquipmentEvents.OnGetPowerHandler handler = ((ref int currentPower) => { currentPower = Mathf.CeilToInt(currentPower * modifier); });
            onEquip += () =>
            {
                equipmentEvents.OnGetAttackPower += handler;
            };

            onUnequip += () =>
            {
                equipmentEvents.OnGetAttackPower -= handler;
            };

            if (modifier > 1f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0104-A");
                descriptionVariables.Add(((modifier - 1f) * 100f).ToString());
            } else  if (modifier < 1f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0104-B");
                descriptionVariables.Add(((1f - modifier) * 100f).ToString());
            }
        }

        public void SetHealPowerModifierFlat(EquipmentEvents equipmentEvents, float modifier)
        {
            if (equipmentEvents == null)
                return;
            EquipmentEvents.OnGetPowerHandler handler = ((ref int currentPower) => { currentPower = Mathf.CeilToInt(currentPower + modifier); });
            onEquip += () =>
            {
                equipmentEvents.OnGetHealPowerFlat += handler;
            };

            onUnequip += () =>
            {
                equipmentEvents.OnGetHealPowerFlat -= handler;
            };

            if (modifier > 0f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0105-A");
                descriptionVariables.Add(modifier.ToString());
            }
            else if (modifier < 0f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0105-B");
                descriptionVariables.Add((-modifier).ToString());
            }
        }

        public void SetHealPowerModifier(EquipmentEvents equipmentEvents, float modifier)
        {
            if (equipmentEvents == null)
                return;
            EquipmentEvents.OnGetPowerHandler handler = ((ref int currentPower) => { currentPower = Mathf.CeilToInt(currentPower * modifier); });
            onEquip += () =>
            {
                equipmentEvents.OnGetHealPower += handler;
            };

            onUnequip += () =>
            {
                equipmentEvents.OnGetHealPower -= handler;
            };

            if (modifier > 1f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0106-A");
                descriptionVariables.Add(((modifier - 1f) * 100f).ToString());
            } else  if (modifier < 1f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0106-B");
                descriptionVariables.Add(((1f - modifier) * 100f).ToString());
            }
        }

        public void SetShieldPowerModifierFlat(EquipmentEvents equipmentEvents, float modifier)
        {
            if (equipmentEvents == null)
                return;
            EquipmentEvents.OnGetPowerHandler handler = ((ref int currentPower) => { currentPower = Mathf.CeilToInt(currentPower + modifier); });
            onEquip += () =>
            {
                equipmentEvents.OnGetShieldPowerFlat += handler;
            };

            onUnequip += () =>
            {
                equipmentEvents.OnGetShieldPowerFlat -= handler;
            };

            if (modifier > 0f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0107-A");
                descriptionVariables.Add(modifier.ToString());
            }
            else if (modifier < 0f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0107-B");
                descriptionVariables.Add((-modifier).ToString());
            }
        }

        public void SetShieldPowerModifier(EquipmentEvents equipmentEvents, float modifier)
        {
            if (equipmentEvents == null)
                return;
            EquipmentEvents.OnGetPowerHandler handler = ((ref int currentPower) => { currentPower = Mathf.CeilToInt(currentPower * modifier); });
            onEquip += () =>
            {
                equipmentEvents.OnGetShieldPower += handler;
            };

            onUnequip += () =>
            {
                equipmentEvents.OnGetShieldPower -= handler;
            };

            if (modifier > 1f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0108-A");
                descriptionVariables.Add(((modifier - 1f) * 100f).ToString());
            } else  if (modifier < 1f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0108-B");
                descriptionVariables.Add(((1f - modifier) * 100f).ToString());
            }
        }

        public void SetEffectPowerModifierFlat(EquipmentEvents equipmentEvents, float modifier)
        {
            if (equipmentEvents == null)
                return;
            EquipmentEvents.OnGetPowerHandler handler = ((ref int currentPower) => { currentPower = Mathf.CeilToInt(currentPower + modifier); });
            onEquip += () =>
            {
                equipmentEvents.OnGetEffectPowerFlat += handler;
            };

            onUnequip += () =>
            {
                equipmentEvents.OnGetEffectPowerFlat -= handler;
            };

            if (modifier > 0f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0109-A");
                descriptionVariables.Add(modifier.ToString());
            }
            else if (modifier < 0f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0109-B");
                descriptionVariables.Add((-modifier).ToString());
            }
        }

        public void SetEffectPowerModifier(EquipmentEvents equipmentEvents, float modifier)
        {
            if (equipmentEvents == null)
                return;
            EquipmentEvents.OnGetPowerHandler handler = ((ref int currentPower) => { currentPower = Mathf.CeilToInt(currentPower * modifier); });
            onEquip += () =>
            {
                equipmentEvents.OnGetEffectPower += handler;
            };

            onUnequip += () =>
            {
                equipmentEvents.OnGetEffectPower -= handler;
            };

            if (modifier > 1f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0110-A");
                descriptionVariables.Add(((modifier - 1f) * 100f).ToString());
            } else  if (modifier < 1f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0110-B");
                descriptionVariables.Add(((1f - modifier) * 100f).ToString());
            }
        }

        public void SetCooldownModifier(EquipmentEvents equipmentEvents, float modifier)
        {
            if (equipmentEvents == null)
                return;
            EquipmentEvents.OnGetCooldownHandler handler = ((ref float currentCooldown) => { currentCooldown = currentCooldown * modifier; });
            onEquip += () =>
            {
                equipmentEvents.OnGetCooldown += handler;
            };

            onUnequip += () =>
            {
                equipmentEvents.OnGetCooldown -= handler;
            };

            if (modifier > 1f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0201-B");
                descriptionVariables.Add(((modifier - 1f) * 100f).ToString());
            }
            else if (modifier < 1f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0201-A");
                descriptionVariables.Add(((1f - modifier) * 100f).ToString());
            }
        }

        public void SetStartingCooldown(EquipmentEvents equipmentEvents, float modifier)
        {
            if (equipmentEvents == null)
                return;
            EquipmentEvents.OnGetCooldownHandler handler = ((ref float currentCooldown) => { currentCooldown = modifier; });
            onEquip += () =>
            {
                equipmentEvents.OnGetStartingCooldowns += handler;
            };
            
            onUnequip += () =>
            {
                equipmentEvents.OnGetStartingCooldowns -= handler;
            };

            descriptionReferences.Add("EQUIPMENT_EFFECT_0202-A");
            descriptionVariables.Add((modifier * 100f).ToString());
        }

        public void SetAttackAbilityBloodCost(EquipmentEvents equipmentEvents, float modifier)
        {
            if (equipmentEvents == null)
                return;
            EquipmentEvents.OnGetAbilityBloodCostHandler handler = ((ScriptableIntReference bloodReference) => { if (bloodReference == null) return; bloodReference.SetValue((int)(bloodReference.GetValue() * (1f-modifier))); });
            onEquip += () =>
            {
                equipmentEvents.OnGetAttackAbilityBloodCost += handler;
            };
            
            onUnequip += () =>
            {
                equipmentEvents.OnGetAttackAbilityBloodCost -= handler;
            };

            descriptionReferences.Add("EQUIPMENT_EFFECT_0301-A");
            descriptionVariables.Add((modifier * 100f).ToString());
        }

        public void SetMaxHealth(EquipmentEvents equipmentEvents, float modifier)
        {
            if (equipmentEvents == null)
                return;
            EquipmentEvents.OnGetMaxHealthHandler handler = ((ref int health) => { health += (int)(health * modifier); });
            onEquip += () =>
            {
                equipmentEvents.OnGetMaxHealth += handler;
                statusManager.health.RecalculateMaxHealth(equipmentEvents);
            };
            
            onUnequip += () =>
            {
                equipmentEvents.OnGetMaxHealth -= handler;
                statusManager.health.RecalculateMaxHealth(equipmentEvents);
            };

            if (modifier > 0f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0401-A");
            } else
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0401-B");
            }
            descriptionVariables.Add(Mathf.Abs(modifier * 100f).ToString());
        }

        public void SetExtraBlood(EquipmentEvents equipmentEvents, float modifier)
        {
            if (equipmentEvents == null)
                return;
            EquipmentEvents.OnGetExtraBloodHandler handler = ((ref int blood) => { blood += (int)(blood * modifier); });
            onEquip += () =>
            {
                equipmentEvents.OnGetExtraBlood += handler;
            };
            
            onUnequip += () =>
            {
                equipmentEvents.OnGetExtraBlood -= handler;
            };

            if (modifier > 0f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0302-A");
            } else
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0302-B");
            }
            descriptionVariables.Add(Mathf.Abs(modifier * 100f).ToString());
        }

        public void SetBloodLoss(EquipmentEvents equipmentEvents, float modifier)
        {
            if (equipmentEvents == null)
                return;
            EquipmentEvents.OnBloodLossPerSecondHandler handler = ((ref int bloodLost) => { bloodLost += (int)modifier; });
            onEquip += () =>
            {
                equipmentEvents.OnBloodLossPerSecond += handler;
            };
            
            onUnequip += () =>
            {
                equipmentEvents.OnBloodLossPerSecond -= handler;
            };

            descriptionReferences.Add("EQUIPMENT_EFFECT_0303-A");
            descriptionVariables.Add(modifier.ToString());
        }

        public void SetUpgradePriceModifier(EquipmentEvents equipmentEvents, float modifier)
        {
            if (equipmentEvents == null)
                return;
            EquipmentEvents.OnShopPriceHandler handler = ((ref int price) => { price = (int)(price * modifier); });
            onEquip += () =>
            {
                equipmentEvents.OnUpgradePrice += handler;
            };
            
            onUnequip += () =>
            {
                equipmentEvents.OnUpgradePrice -= handler;
            };

            if (modifier < 1f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0501-A");
            } else
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0501-B");
            }
            descriptionVariables.Add((Mathf.Abs(1f - modifier) * 100f).ToString());
        }

        public void SetShopPriceModifier(EquipmentEvents equipmentEvents, float modifier)
        {
            if (equipmentEvents == null)
                return;
            EquipmentEvents.OnShopPriceHandler handler = ((ref int price) => { price = (int)(price * modifier); });
            onEquip += () =>
            {
                equipmentEvents.OnShopPrice += handler;
            };
            
            onUnequip += () =>
            {
                equipmentEvents.OnShopPrice -= handler;
            };

            if (modifier < 1f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0502-A");
            } else
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0502-B");
            }
            descriptionVariables.Add((Mathf.Abs(1f - modifier) * 100f).ToString());
        }

        public void SetRetaliationFlatModifier(EquipmentEvents equipmentEvents, float modifier)
        {
            if (equipmentEvents == null)
                return;
            EquipmentEvents.OnRetaliationDamageHandler handler = ((ref int received, ref int caused) => { caused = (int)modifier; });
            onEquip += () =>
            {
                equipmentEvents.OnRetaliationDamageFlat += handler;
            };
            
            onUnequip += () =>
            {
                equipmentEvents.OnRetaliationDamageFlat -= handler;
            };

            descriptionReferences.Add("EQUIPMENT_EFFECT_0601-A");
            descriptionVariables.Add(((int)modifier).ToString());
        }

        public void SetDamageReceivedFlatModifier(EquipmentEvents equipmentEvents, float modifier)
        {
            if (equipmentEvents == null)
                return;
            EquipmentEvents.OnDamageReceivedModifierHandler handler = ((ref int received) => { received = (received + (int)modifier); });
            onEquip += () =>
            {
                equipmentEvents.OnDamageReceivedModifierFlat += handler;
            };
            
            onUnequip += () =>
            {
                equipmentEvents.OnDamageReceivedModifierFlat -= handler;
            };

            if (modifier < 1f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0603-A");
            } else
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0603-B");
            }
            descriptionVariables.Add(modifier.ToString());
        }

        public void SetDamageReceivedModifier(EquipmentEvents equipmentEvents, float modifier)
        {
            if (equipmentEvents == null)
                return;
            EquipmentEvents.OnDamageReceivedModifierHandler handler = ((ref int received) => { received = modifier > 1f ? Mathf.CeilToInt(received * modifier) : Mathf.FloorToInt(received * modifier); });
            onEquip += () =>
            {
                equipmentEvents.OnDamageReceivedModifier += handler;
            };
            
            onUnequip += () =>
            {
                equipmentEvents.OnDamageReceivedModifier -= handler;
            };

            if (modifier < 1f)
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0602-A");
            } else
            {
                descriptionReferences.Add("EQUIPMENT_EFFECT_0602-B");
            }
            descriptionVariables.Add(Mathf.Abs((1f-modifier)*100f).ToString());
        }
        #endregion
    } 
}