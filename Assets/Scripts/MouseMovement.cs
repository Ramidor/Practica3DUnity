using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensibility;

    public float mouseSensibilityDefault = 200f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    public float topClamp = 90f;
    public float bottomClamp = -90f;

    public Slider sliderMouseSensibility;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensibility * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensibility * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, bottomClamp, topClamp);
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    public void SetMouseSensibility()
    {
        mouseSensibility = mouseSensibilityDefault * sliderMouseSensibility.value;
    }
}
