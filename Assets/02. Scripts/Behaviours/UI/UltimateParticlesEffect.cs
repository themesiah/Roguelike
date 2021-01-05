using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Values;
using GamedevsToolbox.ScriptableArchitecture.Pools;

namespace Laresistance.Behaviours
{
    public class UltimateParticlesEffect : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem particles = default;
        [SerializeField]
        private ScriptableFloatReference speedRef = default;


        public void GoToTarget(Vector3 position, ScriptablePool pool)
        {
            StartCoroutine(GoToTargetCoroutine(position, pool));
        }

        private IEnumerator GoToTargetCoroutine(Vector3 position, ScriptablePool pool)
        {
            particles.Play();
            while (transform.position != position)
            {
                Vector3 newPos = Vector3.MoveTowards(transform.position, position, speedRef.GetValue() * Time.deltaTime);
                transform.position = newPos;
                yield return null;
            }

            particles.Stop();
            yield return new WaitForSeconds(1f);
            pool.FreeInstance(gameObject);
        }
    }
}