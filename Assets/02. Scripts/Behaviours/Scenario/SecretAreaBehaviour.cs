using UnityEngine;

namespace Laresistance.Behaviours
{
    public class SecretAreaBehaviour : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer[] secretBlockingGraphics = default;
        [SerializeField]
        private float transitionTime = default;

        private float timer = 0f;
        private float targetTimer = 0f;

        private void Awake()
        {
            HideSecretArea();
        }

        private void ChangeGraphicsAlpha(float alpha)
        {
            foreach(var graphic in secretBlockingGraphics)
            {
                Color c = graphic.color;
                c.a = alpha;
                graphic.color = c;
            }
        }

        private void Update()
        {
            if (timer != targetTimer)
            {
                if (timer > targetTimer)
                {
                    timer -= Time.deltaTime;
                    if (timer < targetTimer)
                        timer = targetTimer;
                } else
                {
                    timer += Time.deltaTime;
                    if (timer > targetTimer)
                        timer = targetTimer;
                }
                float alpha = (float)timer / (float)transitionTime;
                ChangeGraphicsAlpha(alpha);
            }
        }

        public void ShowSecretArea()
        {
            targetTimer = 0f;
        }

        public void HideSecretArea()
        {
            targetTimer = transitionTime;
        }
    }
}