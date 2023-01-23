 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    float xRotation = 0f;

    public Transform playerBody;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        MouseMovement();
    }

    private void MouseMovement()
    {
        if (!InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen && !UIManager.Instance.isLosePanelOpen && !UIManager.Instance.isWinPanelOpen && !InventorySystem.Instance.isEscapeMenuOpen)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 80f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
