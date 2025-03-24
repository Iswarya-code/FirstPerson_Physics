using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float mouseSensitivity = 3f;
    public Transform cameraTransform;

    private Rigidbody rb;
    private float xRotation = 0f;
    private bool jumpRequest = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor in the center
        Camera.main.nearClipPlane = 0.01f; // Prevents jerks when close to objects
    }

    void Update()
    {
        // Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Jump Input
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            jumpRequest = true;
        }
    }

    void FixedUpdate()
    {
        // Movement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);

        // Jumping
        if (jumpRequest)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequest = false;
        }
    }

    private bool IsGrounded()
    {
        return Physics.SphereCast(transform.position, 0.4f, Vector3.down, out _, 0.7f);
    }
}
