using System.Collections;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public bool canShoot = true; // Indicates if the player can shoot
    public Transform player; // Reference to the player transform
    public Transform gun; // Reference to the main gun transform
    public GameObject bullet; // Prefab for the main bullet
    public float fireRate = 50f; // Bullets per second
    public Animator anim;
    public GameManager gm;
    public Joystick rotationJoystick; // Reference to the rotation joystick

    private float fireInterval; // Time interval between shots

    void Start()
    {
        // Calculate the interval between shots based on the fire rate
        fireInterval = 1f / fireRate;
    }

    void Update()
    {
        // Check if the game is on and the player is aiming or shooting
        if (gm.GameOn)
        {
            if (gm.isPc)
            {
                HandleMouseInput();
            }
            else
            {
                HandleControllerInput();
            }
        }
        else
        {
            // Stop shooting animation if the game is not on
            anim.SetBool("Shooting", false);
        }
    }

    private void HandleMouseInput()
    {
        // Check if the player is pressing the left mouse button
        if (Input.GetMouseButton(0))
        {
            // Set shooting animation
            anim.SetBool("Shooting", true);

            // Start shooting if not already in the middle of a shot
            if (canShoot)
            {
                StartCoroutine(ShootBullet());
            }
        }
        else
        {
            // Stop shooting animation
            anim.SetBool("Shooting", false);
        }
    }

    private void HandleControllerInput()
    {
        // Get the rotation input from the second joystick
        float aimX = rotationJoystick.Horizontal;
        float aimZ = rotationJoystick.Vertical;

        // Create a direction vector based on the rotation input
        Vector3 aimDirection = new Vector3(aimX, 0, aimZ);

        // Check if there's significant input for aiming
        if (aimDirection.magnitude > 0.1f)
        {
            // Set shooting animation
            anim.SetBool("Shooting", true);

            // Start shooting if not already in the middle of a shot
            if (canShoot)
            {
                StartCoroutine(ShootBullet());
            }
        }
        else
        {
            // Stop shooting animation
            anim.SetBool("Shooting", false);
        }
    }

    private IEnumerator ShootBullet()
    {
        canShoot = false;

        // Instantiate the bullet at the gun's position with the player's rotation
        Instantiate(bullet, gun.position + this.transform.forward, player.rotation);

        // Wait for the fire interval before allowing another shot
        yield return new WaitForSeconds(fireInterval);

        canShoot = true;
    }
}
