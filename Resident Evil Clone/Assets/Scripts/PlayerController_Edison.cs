using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Edison : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float mouseSensitivity = 60f;

    [SerializeField] float verticalLookLimit;
    [SerializeField] Transform fpsCamera;

    [SerializeField] Weapon_Edison currentWeapon;

    private bool isGrounded;
    private float xRotation;
    private Rigidbody rb;
    private Magazine_Edison currentMag;

    [SerializeField] Transform dropPoint;
    public Magazine_Edison CurrentMag { get => currentMag; set => currentMag = value; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Move();
        LookAround();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            float distance = 100f;
            Debug.DrawRay(fpsCamera.position, fpsCamera.forward * distance, Color.green, 2f);
            if (Physics.Raycast(fpsCamera.position, fpsCamera.forward, out RaycastHit hit, distance))
            {
                if (hit.transform.TryGetComponent(out Magazine_Edison magazine))
                {
                    Debug.Log("Magazine");
                    magazine.OnPickup(this);
                    Debug.Log(currentMag);

                    currentWeapon.Magazine = currentMag;
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("Drop");
                if (currentMag != null)
                {
                    Debug.Log("Dropping");
                    currentMag.OnDrop(dropPoint);
                    currentMag = null;
                }
            }
        }
    }
    private void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalLookLimit, verticalLookLimit);

        fpsCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        Vector3 moveVelocity = move * moveSpeed;

        moveVelocity.y = rb.velocity.y;

        rb.velocity = moveVelocity;
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
