using Laresistance.Core;
using Laresistance.Data;
using System.Collections;

namespace Laresistance.Behaviours
{
    public interface IPanelBehaviour
    {
        IEnumerator StartPanel(RewardData rewardData, Player player);
    }
}