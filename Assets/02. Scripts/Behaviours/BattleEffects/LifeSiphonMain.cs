using UnityEngine;
using System.Collections;
using GamedevsToolbox.ScriptableArchitecture.Sets;

namespace Laresistance.Behaviours
{
    public class LifeSiphonMain : MonoBehaviour
    {
        [SerializeField]
        private RuntimeSingleTransform targetRef = default;
        [SerializeField]
        private float moveDelay = default;
        [SerializeField]
        private float timeToArrive = default;

        private bool playing = false;

        public void Play()
        {
            playing = true;
            if (targetRef.Get() != null)
            {
                DoEffect(targetRef.Get());
            }
        }

        private void OnEnable()
        {
            targetRef.RegisterOnSetEvent(DoEffect);
        }

        private void OnDisable()
        {
            targetRef.UnregisterOnSetEvent(DoEffect);
        }

        private void DoEffect(Transform t)
        {
            if (playing)
            {
                StartCoroutine(MoveCoroutine(t));
            }
        }

        private IEnumerator MoveCoroutine(Transform t)
        {
            yield return new WaitForSeconds(moveDelay);
            float distance = Vector3.Distance(transform.position, t.position);
            float speed = distance / timeToArrive;
            float timer = 0f;
            while (timer < timeToArrive)
            {
                var pos = Vector3.MoveTowards(transform.position, t.position, speed * Time.deltaTime);
                transform.position = pos;
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}