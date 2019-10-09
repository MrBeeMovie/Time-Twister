using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private string mousexInputString = "Mouse X", mouseyInputString = "Mouse Y";
    [SerializeField] private float sensitivity = 150f;
    [SerializeField] private Transform playerBody = null;
    private float xAxisClamp;

    private void Awake()
    {
        xAxisClamp = 0.0f;
        LockCursor();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        float mouseX = Input.GetAxisRaw(mousexInputString) * sensitivity * Time.unscaledDeltaTime;
        float mouseY = Input.GetAxisRaw(mouseyInputString) * sensitivity * Time.unscaledDeltaTime;

        xAxisClamp += mouseY;

        if(xAxisClamp > 90.0f)
        {
            xAxisClamp = 90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(270.0f);
        }
        else if(xAxisClamp < -90.0f)
        {
            xAxisClamp = -90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(90.0f);
        }

        transform.Rotate(-Vector3.right * mouseY);
        playerBody.transform.Rotate(Vector3.up * mouseX);
    }

    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }
}
