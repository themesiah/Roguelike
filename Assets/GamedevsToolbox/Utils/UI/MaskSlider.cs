using UnityEngine;

namespace GamedevsToolbox.Utils
{
    public class MaskSlider : MonoBehaviour
    {
        [SerializeField] [Range(0f,1f)]
        private float sliderValue = 0f;
        [SerializeField]
        private RectTransform fill = default;
        [SerializeField]
        private RectTransform background = default;

        private void Awake()
        {
            ChangeSlider();
        }

        public float Value
        {
            get { return sliderValue; }
            set
            {
                sliderValue = Mathf.Clamp01(value);
                ChangeSlider();
            }
        }

        private void ChangeSlider()
        {
            var size = fill.sizeDelta;
            size.x = sliderValue * background.rect.width;
            fill.sizeDelta = size;
        }

        public void UpdateSlider()
        {
            ChangeSlider();
        }
    }
}