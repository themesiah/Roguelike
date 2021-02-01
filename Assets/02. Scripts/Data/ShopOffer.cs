using Laresistance.Core;

namespace Laresistance.Data
{
    public class ShopOffer
    {
        public int Cost { get; private set; }
        public bool UseHardCurrency { get; private set; }
        public RewardData Reward { get; private set; }

        public ShopOffer(int cost, bool useHardCurrency, RewardData reward)
        {
            Cost = cost;
            UseHardCurrency = useHardCurrency;
            Reward = reward;
        }

        public void SetNewCost(EquipmentsContainer equipments)
        {
            Cost = equipments.ModifyValue(Equipments.EquipmentSituation.ShopPrice, Cost);
        }
    }
}