using GamedevsToolbox.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Laresistance.Behaviours
{
    public class AbyssElevator : MonoBehaviour, IPausable
    {
        [Header("Starting animation configuration")]
        [SerializeField]
        private Animator elevatorAnimator = default;
        [SerializeField]
        private Transform movingBone = default;
        
        [SerializeField]
        private Transform startingPoint = default;
        [SerializeField]
        private Transform midPoint = default;
        [SerializeField]
        private Transform endingPoint = default;
        [SerializeField]
        private float startAnimationSpeedY = 1f;
        [SerializeField]
        private string animatorTriggerName = "";
        [SerializeField]
        private float animationDuration = 1f;
        [SerializeField]
        private Collider2D interactionCollider = default;
        [SerializeField]
        private Collider2D topCollider = default;

        [Header("Normal behaviour configuration")]
        [SerializeField]
        private Transform upPoint = default;
        [SerializeField]
        private Transform bottomPoint = default;
        [SerializeField]
        private float elevatorSpeed = 1f;


        private Coroutine elevatorNormalBehaviourCoroutine = null;

        private Rigidbody2D elevatorBody = default;
        private bool paused = false;
        private int pauseStack = 0;
        private float animSpeed = 0f;
        private bool startDone = false;
        private Vector2 currentElevatorVelocity;
        private bool currentlyUp = false;
        private bool doingStartingAnimation = false;

        private bool moving = false;
        private bool movingPhysics = true;
        private Vector3 currentTargetPosition = Vector3.zero;
        private float currentSpeed = 0f;

        private void Awake()
        {
            UnityEngine.Assertions.Assert.IsNotNull(startingPoint);
            UnityEngine.Assertions.Assert.IsNotNull(midPoint);
            UnityEngine.Assertions.Assert.IsNotNull(endingPoint);
            UnityEngine.Assertions.Assert.IsNotNull(movingBone);
            UnityEngine.Assertions.Assert.IsNotNull(elevatorAnimator);
            UnityEngine.Assertions.Assert.IsNotNull(upPoint);
            UnityEngine.Assertions.Assert.IsNotNull(bottomPoint);
            elevatorBody = GetComponent<Rigidbody2D>();
            movingBone.position = startingPoint.position;
        }

        private void FixedUpdate()
        {
            if (!paused && moving)
            {
                if (movingPhysics)
                {
                    Vector3 newPos = Vector3.MoveTowards(elevatorBody.position, currentTargetPosition, elevatorSpeed * Time.fixedDeltaTime);
                    transform.position = newPos;
                    //elevatorBody.MovePosition(newPos);
                } else
                {
                    Vector3 newPos = Vector3.MoveTowards(movingBone.position, currentTargetPosition, currentSpeed * Time.fixedDeltaTime);
                    movingBone.transform.position = newPos;
                }
            }
        }

        public void ActivateElevator()
        {
            if (!doingStartingAnimation)
            {
                if (elevatorNormalBehaviourCoroutine != null && startDone == true)
                {
                    StopCoroutine(elevatorNormalBehaviourCoroutine);
                }
                if (startDone == true)
                {
                    elevatorNormalBehaviourCoroutine = StartCoroutine(ActivateElevatorCoroutine(!currentlyUp));
                }
            }
        }

        public void ElevatorLeverActivation(bool goUp)
        {
            if (!doingStartingAnimation)
            {
                if (elevatorNormalBehaviourCoroutine != null && startDone == true)
                {
                    StopCoroutine(elevatorNormalBehaviourCoroutine);
                }
                if (!startDone)
                {
                    elevatorNormalBehaviourCoroutine = StartCoroutine(ActivateElevatorCoroutine(goUp));
                }
                else
                {
                    elevatorNormalBehaviourCoroutine = StartCoroutine(ElevatorMoveCoroutine(goUp));
                }
            }
        }

        public void Pause()
        {
            paused = true;
            animSpeed = elevatorAnimator.speed;
            elevatorAnimator.speed = 0f;
            if(pauseStack == 0)
            {
                currentElevatorVelocity = elevatorBody.velocity;
                elevatorBody.simulated = false;
                elevatorBody.velocity = Vector2.zero;
            }
            pauseStack++;
        }

        public void Resume()
        {
            paused = false;
            elevatorAnimator.speed = animSpeed;
            pauseStack--;
            pauseStack = System.Math.Max(0, pauseStack);
            if (pauseStack == 0)
            {
                elevatorBody.velocity = currentElevatorVelocity;
                elevatorBody.simulated = true;
            }
        }

        private IEnumerator ActivateElevatorCoroutine(bool goUp)
        {
            if (!startDone)
            {
                doingStartingAnimation = true;
                yield return ElevatorStartCoroutine();
                doingStartingAnimation = false;
            }
            yield return ElevatorMoveCoroutine(goUp);
        }

        private IEnumerator ElevatorStartCoroutine()
        {
            // Move elevator from starting point to mid point using speed
            while (Vector3.Distance(movingBone.position, midPoint.position) > 0.1f)
            {
                if (!paused)
                {
                    Vector3 newPos = Vector3.MoveTowards(movingBone.position, midPoint.position, startAnimationSpeedY * Time.deltaTime);
                    movingBone.transform.position = newPos;
                }
                yield return null;
            }
            // Start animation
            elevatorAnimator.SetTrigger(animatorTriggerName);
            float newSpeed = Vector3.Distance(movingBone.position, bottomPoint.position) / animationDuration;
            // Move elevator from mid point to end point using distance / animation time.
            moving = true;
            currentTargetPosition = endingPoint.position;
            currentSpeed = newSpeed;
            movingPhysics = false;
            while (Vector3.Distance(movingBone.position, endingPoint.position) > 0.1f)
            {
                yield return null;
            }
            moving = false;
            startDone = true;
            interactionCollider.enabled = true;
            topCollider.enabled = true;
        }

        private IEnumerator ElevatorMoveCoroutine(bool goUp)
        {
            Transform targetPoint = goUp ? upPoint : bottomPoint;
            currentlyUp = goUp;
            moving = true;
            currentTargetPosition = targetPoint.position;
            currentSpeed = elevatorSpeed;
            movingPhysics = true;
            while (Vector3.Distance(elevatorBody.position, targetPoint.position) > 0.1f)
            {
                yield return null;
            }
            moving = false;
            elevatorNormalBehaviourCoroutine = null;
        }
    }
}