using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // This is the Header attribute. It will make a title appear in the Inspector for this script.
    [Header("Player Stats")]
    // This is a Tooltip attribute. It will make the description appear in the Inspector when
    // hovering over this field.
    [Tooltip("How many hitpoints the player has.")]
    [SerializeField] float health;
    [Tooltip("The movement speed of the player in meters per second.")]
    [SerializeField] float moveSpeed;
    [Tooltip("The jump force for the player in Newtons.")]
    [SerializeField] float jumpForce;
    [Tooltip("The sensitivity of the mouse looking/aiming.")]
    [SerializeField] float mouseSensitivity;
    [Tooltip("The limit in degrees the player can look up and down.")]
    [SerializeField] float verticalLookLimit;
    // NOTE: You cannot use the Tooltip attribute for things that are not public, or [SerializeField].
    // Keeps track of if the player is on the ground or not for determining if they can or cannot jump.
    private bool isGrounded = true;
    // We'll use this to store the rotation component of movement to apply later.
    private float xRotation;
    
    [Header("Object References")]
    [Tooltip("A reference to the camera attached to the player for their FPS view.")]
    [SerializeField] Transform fpsCamera;
    [Tooltip("The location that our bullets/raycasts will be spawned at for shooting.")]
    [SerializeField] Transform firePoint;
    [Tooltip("A reference to the current weapon the player has equipped.")]
    [SerializeField] Weapon currentWeapon;

    // Reference to the Rigidbody component on the player.
    private Rigidbody rb;

    // We'll use this list of IPickupable to store player inventory items.
    // Remember, one cool thing about Interfaces is that it doesn't matter what the object is (class) that the item is,
    // if it implements IPickupable, we can hold it in this list. Pretty cool!
    private List<IPickupable> inventory = new List<IPickupable>();
    [Tooltip("Reference to the UI object that shows how many rounds we have remaining in our weapon.")]
    [SerializeField] TextMeshProUGUI ammoText;


    // Start is called before the first frame update
    void Start()
    {
        // Initialize the rb field with the value of the Rigidbody component on the player.
        rb = GetComponent<Rigidbody>();

        // This locks the mouse cursor to the center of the screen.
        Cursor.lockState = CursorLockMode.Locked;
        // This hides the mouse cursor. NOTE: We could add a custom mouse cursor (like a crosshair) and unhide this.
        // Alternatively, we can use a UI object for a crosshair or red dot or whatever and keep the cursor hidden.
        Cursor.visible = false;

        // Let's first check to see if the player actually has a weapon equipped.
        if (currentWeapon != null)
        {
            // If they do, let's check how much ammo the current weapon has and insert that into our ammo UI object.
            ammoText.text = "Ammo: " + currentWeapon.CheckAmmo();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Call the LookAround() method so that our look input is captured and applied each frame.
        LookAround();
        // Call MovePlayer() so our movement input is captured and applied each frame.
        MovePlayer();
        // Check to see if the "Jump" button (spacebar by default) is pressed AND make sure the player is on the ground.
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
        // Check to see if the left mouse click button is pressed...
        if (Input.GetMouseButtonDown(0))
        {
            //Shoot();
            currentWeapon.Fire();
        }
        // Check to see if the 'R' key is pressed to check for an attempted reload by the player.
        if (Input.GetKeyDown(KeyCode.R))
        {
            AttemptReload();
        }
    }

    // This method will handle all our mouse movement for looking up and down, as well as rotating the player left and right.
    void LookAround()
    {
        // First, let's capture the mouse movement for the x and y axes.
        // Notice we're multiplying the result by the mouseSensitivity to apply our custom sensitivity
        // and also by Time.deltaTime to make it be in a unit of degrees per second.
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // This rotates the player around the world Y axis due to the input and result of mouseX.
        // NOTE: The naming can be confusing, mouseX captures left to right movement of the mouse.
        transform.Rotate(Vector3.up * mouseX);

        // NOTE: Again, confusing naming maybe, but here, xRotation is referring to rotation around the X axis
        // for the camera (which tilts it up and down.)
        // This will adjust the current xRotation value by the mouse up and down movement.
        xRotation -= mouseY;
        // This will then clamp (lock or limit) the value to be between the negative and positive look limit (inclusive.)
        // Mathf is just a C# thing, and Clamp() is a method in the Mathf class.
        // Clamp() takes three arguments: 1: The value to clamp, 2: The lowest limit, 3: The highest limit.
        xRotation = Mathf.Clamp(xRotation, -verticalLookLimit, verticalLookLimit);
        // Now we rotate the camera finally.
        // We will adjust its LOCAL rotation and set it to be equal to our newly calculated xRotation value.
        // Don't worry about what a Quaternion is, just know they're not very easy to work with, so we are using the
        // Euler() method of the Quaternion class to convert the Quaternion into degrees which is much easier to understand.
        // So we're just setting the local rotation of the camera to be xRotation on the X axis, 0 on the Y, and 0 on the Z axis.
        fpsCamera.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    // This method will move the player.
    void MovePlayer()
    {
        // Capture the input from the Horizontal (left and right) axis and Vertical (forward and back) axis.
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // We'll first create a new vector combining the two movement axes into a single Vector3.
        // If this looks confusing it's basically this:
        // (moveX, 0, 0) + (0, 0, moveZ) = new Vector3(moveX, 0, moveZ).
        // So you can see, adding the two vectors just creates a new one with both values in it.
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        // Now it's important to normalize the vector which sets the magnitude to 1.
        // This is because we want the speed to be calculated by moveSpeed only.
        // For example if the player was moving forward and strafing right at the same time,
        // their move vector would be (1, 0, 1), which has a magnitude of 1.414 (from our old friend Pythagoras)
        // So we use the Normalize() method to change this vector to (0.707, 0, 0.707) which has a magnitude of 1.
        move.Normalize();
        // Now, we'll create a new vector that is the result of our normalized movement direction, multipled by the moveSpeed.
        // Now we have a single vector with our movement direction and magnitude (speed) information.
        Vector3 moveVelocity = move * moveSpeed;

        // This is very important to do this step.
        // If we didn't do this, our Y velocity would ALWAYS be 0, cause our moveVelocity variable
        // was only looking at the X and Z axis. This means our player would never be able to fall due
        // to gravity. Here we are setting the velocity of the Y axis to be just whatever it already was.
        // Recall that velocity is a vector and includes both the direction and the magnitude.
        moveVelocity.y = rb.velocity.y;

        // Finally we can actually apply the movement forces by directly setting the velocity of the player.
        rb.velocity = moveVelocity;
    }

    // Handles Jumping.
    void Jump()
    {
        // Simply add an instant force applied in the world up direction to the player.
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        // Vector3.up = (0, 1, 0)

        // Make sure we flip this boolean to false since we know the player just jumped off the ground.
        isGrounded = false;
    }

    // Event for collision entering.
    private void OnCollisionEnter(Collision collision)
    {
        // If the thing the player's collider entered is the ground, flip the boolean to true so they can jump again..
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
        // If it wasn't the ground, let's check to see if it's anything that has a class on it that implements IPickupable.
        else if (collision.gameObject.GetComponent<IPickupable>() != null)
        {
            // So if it is something that implements IPickupable, we are going to insert that item into our player's inventory.
            // RECALL: We're not actually creating a new instance of this object, we're just referencing the one that exists in the world,
            // and storing that reference in our inventory.
            inventory.Add(collision.gameObject.GetComponent<IPickupable>());
            // After it's added to our inventory, we're going to call the Pickup() method of the thing we touched.
            // Remember, we KNOW it has a Pickup() method, because every single class that implements IPickupable HAS to have a Pickup() method defined.
            collision.gameObject.GetComponent<IPickupable>().Pickup(this);
        }
    }

    // Event for collision exit.
    private void OnCollisionExit(Collision collision)
    {
        // If the player WAS colliding with something and suddenly stops, if it's the ground, flip isGrounded to false so they can't jump.
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    // Handles our player taking damage.
    public void TakeDamage(int damage)
    {
        // The damage amount passed in from wherever the method is called will be subtracted from the player's hitpoints.
        health -= damage;
        // We will also apply a force that pushes the player backward
        // NOTE: transform.forward is referring to the LOCAL forward axis (Z axis, the direction the player is facing).
        // Remember, transform.forward will have a magnitude of 1 cause it's shorthand for (0, 0, 1).
        // So we multiply it by 10 to give it a magnitude of 10 Newtons, (0, 0, 10).
        // Notice we're actually multiplying it by -10, so the resulting vector is (0, 0, -10).
        // So, the force is 10 Newtons in the opposite direction of local forward, which is backwards.
        rb.AddForce(transform.forward * -10);
    }

    // Handles shooting our weapon using a Raycast.
    // A Raycast can simply be thought of a laser beam that shoots straight out from a single point.
    private void Shoot()
    {
        // First, we'll create a RaycastHit variable, which is just a data container that holds lots data from a Raycast collision.
        RaycastHit hit;
        // This is combining the actual firing of the raycast with an if statement to see if it actually hit anything.
        // If the raycast doesn't hit anything, this will be false, and none of the logic inside the conditional statement will be called.
        // So we're calling the Raycast() method from the Physics class. There are several overloads for this method that take a different amount/type of arguments.
        // This particular one we're using takes 4 arguments.
        // (Vector3 location of where to start the raycast from, Vector3 direction of where to shoot the raycast to, Where to store the hit data, How far the raycast should travel).
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, 100))
        {
            // So, if our raycast hit anything, let's first draw a line that can be seen in the Scene (but not Game) view.
            // This is optional but it allows us to actually see where the laser is.
            // DrawRay() is a method in the Debug class. It takes 4 arguments.
            // (Where to start the ray, the direction to fire it in (notice we multiply it by a distance so it's limited to a certain length), the color, the duration in seconds it will be displayed.
            Debug.DrawRay(firePoint.position, firePoint.forward * hit.distance, Color.red, 2f);
            // Check to see if the object the ray hit is a Zombie.
            // NOTE: CompareTag("Zombie") == tag == "Zombie"
            // Remember, the hit variable is storing the thing we hit. So we are accessing the transform of what was hit, and checking the tag.
            if (hit.transform.CompareTag("Zombie"))
            {
                // Grab the Enemy script on the Enemy we hit, and call its TakeDamage() method, passing in the damage to deal (1 in this case).
                hit.transform.GetComponent<Enemy>().TakeDamage(1);
            }
        }
    }

    // Method to call when the player tries to reload.
    private void AttemptReload()
    {
        // First to avoid any null reference errors, let's check to be sure the player actually has a weapon equipped.
        if (currentWeapon != null)
        {
            // We'll create a new local variable of a MagazineType from the Enums static class (recall that means we can call anything in it from anywhere globally in our codebase).
            // We'll set the value of this new local variable to match the magazineType of the currently equipped weapon.
            Enums.MagazineType gunMagType = currentWeapon.magazineType;
            // Let's use a foreach loop to search through every single thing in our inventory. One by one.
            foreach (IPickupable item in inventory)
            {
                // For each item checked, we'll check to see if the current IPickupable is actually a Magazine.
                // RECALL: Because Magazine implements IPickupable, that's how we can do this.
                // In other words since Magazine implements IPickupable, every Magazine is also an IPickupable.
                // NOTE: We have to give the Magazine we find a name (I chose "mag"). This is so we can then do stuff with that Magazine later.
                if (item is Magazine mag)
                {
                    // So obviously if we make it here, the IPickupable we were looking at from the inventory IS a Magazine.
                    // So we need to check to make sure that whatever type that magazine is, matches the type of the equipped weapon.
                    if (mag.GetMagType() == gunMagType)
                    {
                        // Now that we know the magazine in the inventory is of the same type as the currentWeapon, we can reload it.
                        // Notice we will call the Reload() method on the Weapon itself, and pass in the variable we created, which passes in
                        // a reference to this specific magazine in the player's inventory, so now the gun knows which magazine is inserted into it.
                        currentWeapon.Reload(mag);
                        // We then remove the magazine from our inventory. NOTE: We are not destroying the magazine.
                        // We are simply removing it from the inventory list so that we don't keep using the same magazine over and over for subsequent reloads.
                        // It needs to still exist in the world because remember, it's "in" the currentWeapon. If we Destroy it, then the gun has no magazine in it.
                        // This could lead to null reference errors, or depending on how we wrote the code in the Weapon class, it might just not shoot and do nothing.
                        inventory.Remove(item);
                        // We'll now update the current ammo UI object to display how many rounds are loaded into the currentWeapon now that it's been reloaded.
                        ammoText.text = "Ammo: " + currentWeapon.CheckAmmo();
                    }
                }
            }
        }
    }

    // Alternative way to shoot bullets using physical projectiles other than raycasts.
    // There are pros and cons to both methods. Which you choose will be a combination of how realistic you want your game to be, as well as performance considerations.
    // Raycasts are instant. So no bullet drop, and no bullet travel time. They are however computationally very very cheap to do.
    // NOTE: You can "fake" travel time and drop by doing some math to shoot the ray at a different angle than straight forward based on distance to target and bullet velocity.
    // Physical projectiles can have bullet drop and travel time taken care of easily by the physics engine. They are however more complex computations and take more performance resources.
    // They can also cause skipped collisions if the bullets are traveling too fast and/or the thing they're hitting has a small collision box.
    /*
    private void ShootBullet()
    {
       // Spawn the bullet prefab at the location of the firePoint, in the local forward direction of the firePoint.
       // NOTE: When you use Instantiate after GameObject <variableName> =
       // It will also save a reference to the thing you just spawned so you can call its methods or whatever else you wanna do with it.
       GameObject bullet = Instantiate(Projectile, firePoint.position, firePoint.forward);
       // Add an instant force of 10 newtons to the newly spawned bullet in its local forward direction.
       bullet.GetComponent<Rigidbody>().AddForce(firePoint.forward * 10, ForceMode.Impulse);
    }
    */
}