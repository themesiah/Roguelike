using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDistanceForBattleSetup : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask = default;
    public void GetDistance(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        bool enoughSpace = Laresistance.Battle.BattlePosition.CheckSpace(transform.position, layerMask);
        if (enoughSpace == true)
            Debug.Log("There is space");
        else
            Debug.LogWarning("Not enough space");
    }
}
