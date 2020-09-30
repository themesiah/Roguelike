using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingImage : MonoBehaviour
{
    [SerializeField]
    private float fillRate = 1f;
    private Image image;

    private float returning = 1f;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        image.fillAmount = image.fillAmount + fillRate * Time.deltaTime * returning;
        if (image.fillAmount >= 1f || image.fillAmount <= 0f)
        {
            image.fillClockwise = !image.fillClockwise;
            returning *= -1f;
        }
    }
}
