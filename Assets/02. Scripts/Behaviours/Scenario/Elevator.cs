using UnityEngine;
using System.Collections;
using GamedevsToolbox.ScriptableArchitecture.Values;
using GamedevsToolbox.Utils;

namespace Laresistance.Behaviours
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Elevator : MonoBehaviour, IPausable
    {
        [SerializeField]
        private Transform startingPosition = default;
        [SerializeField]
        private Transform endingPosition = default;
        [SerializeField]
        private ScriptableFloatReference elevatorSpeedReference = default;

        private Rigidbody2D body;
        private bool goingFromStartToEnd = true;
        private int pauseStack = 0;
        private Vector2 lastVelocity = Vector2.zero;

        public void Awake()
        {
            UnityEngine.Assertions.Assert.IsNotNull(startingPosition);
            UnityEngine.Assertions.Assert.IsNotNull(endingPosition);
            body = GetComponent<Rigidbody2D>();
        }
        public void Activate()
        {
            float targetY = 0f;
            if (goingFromStartToEnd)
            {
                Debug.Log("Going from start to end");
                targetY = endingPosition.position.y;
            }
            else
            {
                Debug.Log("Going from end to start");
                targetY = startingPosition.position.y;
            }
            Vector3 targetPos = transform.position;
            targetPos.y = targetY;
            StopAllCoroutines();
            StartCoroutine(MoveElevator(targetPos));
            goingFromStartToEnd = !goingFromStartToEnd;
        }

        private IEnumerator MoveElevator(Vector3 targetPos)
        {
            body.velocity = new Vector2(0f, elevatorSpeedReference.GetValue());
            if (transform.position.y > targetPos.y)
            {
                body.velocity = -body.velocity;
                while (transform.position.y > targetPos.y)
                {
                    yield return null;
                }
            } else
            {
                while (transform.position.y < targetPos.y)
                {
                    yield return null;
                }
            }
            body.velocity = Vector2.zero;
        }

        public void Pause()
        {
            if (pauseStack == 0)
            {
                lastVelocity = body.velocity;
                body.simulated = false;
                body.velocity = Vector2.zero;
            }
            pauseStack++;
        }

        public void Resume()
        {
            pauseStack--;
            pauseStack = System.Math.Max(0, pauseStack);
            if (pauseStack == 0)
            {
                body.velocity = lastVelocity;
                body.simulated = true;
            }
        }
    }
}