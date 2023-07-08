using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forceMagnitude;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float rotationSpeed;
    private Rigidbody rb;
    private Camera mainCamera;
    private Vector3 movementDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        ProcessInput();
        KeepPlayerOnScreen();
        RotateToFaceVelocity();
    }

    void FixedUpdate()
    {
        if (movementDirection == Vector3.zero) { return; }

        rb.AddForce(movementDirection * forceMagnitude * Time.deltaTime, ForceMode.Force);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }


    private void ProcessInput()
    {
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            // Debug.Log($"touchPosition: {touchPosition}");
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
            movementDirection = transform.position - worldPosition;
            // Debug.Log($"movementDirection:{movementDirection} = {transform.position} - {worldPosition}");
            movementDirection.z = 0f;
            // Debug.Log($"movementDirection.z:{movementDirection.z}");
            movementDirection.Normalize();
        }
        else
        {
            movementDirection = Vector3.zero;
        }
    }

    private void KeepPlayerOnScreen()
    {
        // Debug.Log($"KeepPlayerOnScreen");

        Vector3 newPosition = transform.position;
        // Debug.Log($"newPosition: {newPosition}");
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        // Debug.Log($"viewportPosition: {viewportPosition}");

        if (viewportPosition.x > 1)
        {
            // Debug.Log("x>1");
            newPosition.x = -newPosition.x + 0.1f;
        }
        else if (viewportPosition.x < 0)
        {
            // Debug.Log("x<0");
            newPosition.x = -newPosition.x + 0.1f;
        }


        if (viewportPosition.y > 1)
        {
            // Debug.Log("y>1");
            newPosition.y = -newPosition.y + 0.1f;
        }
        else if (viewportPosition.y < 0)
        {
            // Debug.Log("y<0");
            newPosition.y = -newPosition.y + 0.1f;
        }

        transform.position = newPosition;
        // Debug.Log($"transform.position: {transform.position}");

    }

    private void RotateToFaceVelocity()
    {
        if (rb.velocity == Vector3.zero) { return; }

        Quaternion targetRotation = Quaternion.LookRotation(rb.velocity, Vector3.back);
        // Debug.Log($"targetRotation: {targetRotation}");
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
