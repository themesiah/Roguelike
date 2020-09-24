using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.Utils
{
    [RequireComponent(typeof(Camera))]
    public class FreeCameraController : MonoBehaviour
    {
        [SerializeField]
        private float movementSpeed = 10f;

        [SerializeField]
        private float rotationSpeed = 100f;
        
        private Vector3 currentEuler = default;

        private void Start()
        {
            currentEuler = transform.rotation.eulerAngles;
        }

        private void Update()
        {
            MoveCamera();
            RotateCamera();
        }

        private void RotateCamera()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Cursor.visible = false;
            }
            if (Input.GetMouseButton(1))
            {
                Vector3 delta = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0f);
                currentEuler += delta * rotationSpeed * Time.deltaTime;
                var rot = transform.rotation;
                rot.eulerAngles = currentEuler;
                transform.rotation = rot;
            }
            if (Input.GetMouseButtonUp(1))
            {
                Cursor.visible = true;
            }
        }

        private void MoveCamera()
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            float y = Input.GetKey(KeyCode.E) ? 1f : 0f;
            y = Input.GetKey(KeyCode.Q) ? -1f : y;
            transform.Translate(new Vector3(x, y, z) * Time.deltaTime * movementSpeed);
        }
    }
}