using System.Collections;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public interface IRangedAttackSpawner
    {
        IEnumerator SpawnRangedAttack(Transform targetPosition);
    }
}