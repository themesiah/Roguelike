using DigitalRuby.Tween;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class SelectionArrowBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float movementDuration = 1f;
        [SerializeField]
        private Vector3 targetPosition = default;
        [SerializeField]
        private Vector3 initialPosition = Vector3.zero;

        private void OnEnable()
        {
            ExecuteTween();
        }

        private void OnDisable()
        {
            TweenFactory.RemoveTweenKey("SelectionArrow", TweenStopBehavior.DoNotModify);
        }

        private void UpdateArrowPosition(ITween<Vector3> t)
        {
            transform.localPosition = t.CurrentValue;
        }

        private void MoveCompleted(ITween<Vector3> t)
        {
            ExecuteTween();
        }

        private void ExecuteTween()
        {
            TweenFactory.Tween("SelectionArrow", initialPosition, targetPosition, movementDuration, TweenScaleFunctions.Linear, UpdateArrowPosition, ExecuteReverseTween);
        }

        private void ExecuteReverseTween(ITween<Vector3> t)
        {
            TweenFactory.Tween("SelectionArrow", targetPosition, initialPosition, movementDuration, TweenScaleFunctions.Linear, UpdateArrowPosition, MoveCompleted);
        }
    }
}