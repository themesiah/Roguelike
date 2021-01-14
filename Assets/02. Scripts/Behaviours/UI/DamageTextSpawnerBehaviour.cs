using Laresistance.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class DamageTextSpawnerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject canvasPrefab = default;
        [SerializeField]
        private GameObject textPrefab = default;
        [SerializeField]
        private Transform anchor = default;
        private GameObject canvasObject;

        private void Awake()
        {
            canvasObject = Instantiate(canvasPrefab, transform);
            //canvasObject.transform.rotation = Quaternion.identity;
            //float scale = 1f / canvasObject.transform.lossyScale.x;
            //canvasObject.transform.localScale = new Vector3(scale, Mathf.Abs(scale), Mathf.Abs(scale));
            canvasObject.transform.position = anchor.position;
        }

        private void Update()
        {
            //canvasObject.transform.position = anchor.transform.position;
        }

        public void DamageText(CharacterHealth sender, int damageTaken, int currentHealth)
        {
            DamageText(damageTaken);
        }

        public void DamageText(int damageTaken)
        {
            SpawnDamageText(damageTaken, Color.red);
        }

        public void HealText(CharacterHealth sender, int healAmount, int currentHealth)
        {
            HealText(healAmount);
        }

        public void HealText(int healAmount)
        {
            SpawnDamageText(healAmount, Color.green);
        }

        public void ShieldText(int shieldAmount)
        {
            SpawnDamageText(shieldAmount, Color.blue);
        }

        private void SpawnDamageText(int power, Color color)
        {
            GameObject textObject = Instantiate(textPrefab, canvasObject.transform);
            Text text = textObject.GetComponent<Text>();
            text.text = power.ToString();
            text.color = color;
            textObject.transform.localPosition = Vector3.zero;
        }

        public void SpawnMissText()
        {
            GameObject textObject = Instantiate(textPrefab, canvasObject.transform);
            Text text = textObject.GetComponent<Text>();
            text.text = "MISS";
            text.color = Color.white;
            textObject.transform.localPosition = Vector3.zero;
        }

        private void OnDestroy()
        {
            Destroy(canvasObject.gameObject);
        }
    }
}