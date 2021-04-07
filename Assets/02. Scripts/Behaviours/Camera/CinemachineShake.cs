using UnityEngine;
using Cinemachine;

namespace Laresistance.Behaviours
{
    public class CinemachineShake : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera vcam = default;

		[Header("Trauma properties")]
		[SerializeField]
		private float traumaDecreaseRate = 0.3f;
		[SerializeField]
		private float traumaForce = 2f;

		private float trauma = 0f;
		private CinemachineBasicMultiChannelPerlin camNoise;

        private void Awake()
        {
			camNoise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
			if (camNoise != null)
			{
				camNoise.m_AmplitudeGain = 0f;
			}
		}

        void LateUpdate()
		{
			if (trauma > 0f && traumaDecreaseRate > 0f)
			{
				float t = Mathf.Max(0f, trauma - traumaDecreaseRate * Time.deltaTime);
				ChangeTrauma(t);
			}
		}

		public void AddTrauma(float t)
		{
			t = Mathf.Min(trauma + t, 1f);
			ChangeTrauma(t);
		}

		private void ChangeTrauma(float t)
        {
			trauma = t;
			if (camNoise != null)
			{
				camNoise.m_AmplitudeGain = t * traumaForce;
			}
		}
	}
}