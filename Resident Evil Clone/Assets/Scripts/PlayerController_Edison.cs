using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Edison : MonoBehaviour
{
    [SerializeField] Transform dropPoint;
    [SerializeField] Weapon weapon;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float mouseSensity = 60f;

    [SerializeField] float verticalLookLimit;
    [SerializeField] Transform fpsCamera;
    [SerializeField] Transform firePoint;

    private bool isGrounded;
    private float xRotation;
    private Rigidbody rb;
    private Magazine_E currentMag;
    public Magazine_E CurrentMag { get => currentMag; set => currentMag = value; }
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
            if(Physics.Raycast(fpsCamera.position,fpsCamera.forward, out RaycastHit hit, distance))
            {
                if(hit.transform.TryGetComponent(out Magazine_E magazine)){
                    Debug.Log("Magazine");
                    magazine.OnPickup(this);
                    Debug.Log(currentMag);
                    weapon.CurrentMag = currentMag;
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Q)){
            currentMag.OnDrop(dropPoint);
        }
    }
    private void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensity * Time.deltaTime;

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

    //private void Shoot(float damage)
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, 100))
    //    {
    //        Debug.DrawRay(firePoint.position, firePoint.forward * hit.distance, Color.red, 2f);
    //        if (hit.transform.CompareTag("Zombie"))
    //        {
    //            hit.transform.GetComponent<Zombie_Edison>().TakeDamage(damage);
    //        }
    //    }
    //}
}
