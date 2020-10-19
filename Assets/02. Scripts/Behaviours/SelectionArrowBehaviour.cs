using DigitalRuby.Tween;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class SelectionArrowBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float deltaMovement = 3f;
        [SerializeField]
        private float movementDuration = 1f;

        private void OnEnable()
        {
            ExecuteTween();
        }

        private void OnDisable()
        {
            TweenFactory.RemoveTweenKey("SelectionArrow", TweenStopBehavior.DoNotModify);
        }

        private void UpdateArrowPosition(ITween<float> t)
        {
            var pos = transform.localPosition;
            pos.y = t.CurrentValue;
            transform.localPosition = pos;
        }

        private void MoveCompleted(ITween<float> t)
        {
            ExecuteTween();
        }

        private void ExecuteTween()
        {
            TweenFactory.Tween("SelectionArrow", 0f, -deltaMovement, movementDuration, TweenScaleFunctions.Linear, UpdateArrowPosition, ExecuteReverseTween);
        }

        private void ExecuteReverseTween(ITween<float> t)
        {
            TweenFactory.Tween("SelectionArrow", -deltaMovement, 0f, movementDuration, TweenScaleFunctions.Linear, UpdateArrowPosition, MoveCompleted);
        }
    }
}