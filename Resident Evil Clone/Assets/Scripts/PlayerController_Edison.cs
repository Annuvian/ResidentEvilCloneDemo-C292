using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController_Edison : MonoBehaviour
{
    [SerializeField] private Transform dropPoint;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float mouseSensitivity = 60f;

    [SerializeField] private float verticalLookLimit;
    [SerializeField] private Transform fpsCamera;
    [SerializeField] private Weapon_Edison currentWeapon;

    [SerializeField] private int maxHealth;
    private int currentHealth;
    private bool isKnockedBack = false;

    private bool isGrounded;
    private float xRotation;
    private Rigidbody rb;
    private Magazine_Edison currentMag;

    public bool IsKnockedBack { get => isKnockedBack; set => isKnockedBack = value; }
    public Magazine_Edison CurrentMag { get => currentMag; set => currentMag = value; }
    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }

    private bool debug = false;

    //One way to create an event
    public delegate void onPlayerDeath();
    public static event onPlayerDeath OnPlayerDeath;
    public static onPlayerDeath OnPlayerDeath2;

    //Another way to create an event
    public static event Action<int,int> OnDamaged;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentHealth = maxHealth;
        OnDamaged?.Invoke(currentHealth,maxHealth);
    }

    private void Update()
    {
        if (!isKnockedBack) {
            Move();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        LookAround();

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
        if (isKnockedBack && !debug)
        {
            Debug.Log("Knocked Back");
            debug = true;

        }else if (isKnockedBack == false && debug)
        {
            Debug.Log("Knock Backed Stopped");
            debug = false;
        }

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

    /// <summary>
    /// Damages the player
    /// </summary>
    /// <param name="damageAmount"> The amount of damage being dealt to the player</param>
    /// <param name="direction"> The direction of knockback, usually the normal to the zombie</param>
    public void Damage(int damageAmount, Vector3 direction, float knockbackPower)
    {
        if(currentHealth - damageAmount <= 0)
        {
            Debug.Log("Player is dead");

            currentHealth = 0;
            OnPlayerDeath?.Invoke();
            return;
        }

        currentHealth -= damageAmount;

        rb.velocity = direction * knockbackPower;
        StartCoroutine(Knockback(direction, knockbackPower, 1f));

        Debug.Log(rb.velocity);

        OnDamaged?.Invoke(currentHealth,maxHealth);

        Debug.Log("Player took damage");
    }

    private IEnumerator Knockback(Vector3 direction, float knockbackPower, float time)
    {
        isKnockedBack = true;
        rb.AddForce(direction * knockbackPower, ForceMode.Impulse);
        Debug.Log(rb.velocity);

        yield return new WaitForSeconds(time);
        isKnockedBack = false;
    }
}
