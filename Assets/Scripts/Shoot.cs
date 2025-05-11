using System.Collections;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public bool canShoot = true; // Indicates if the player can shoot
    public Transform player; // Reference to the player transform
    public Transform gun; // Reference to the main gun transform
    public GameObject bullet; // Prefab for the main bullet (shuriken)
    public float fireRate = 50f; // Bullets per second
    public Animator anim;
    public GameManager gm;
    public Joystick rotationJoystick; // Reference to the rotation joystick

    private WaitForSeconds fireWait; // Cached WaitForSeconds for fire interval

    void Start()
    {
        // Cache the fire interval so we don't allocate every shot
        fireWait = new WaitForSeconds(1f / fireRate);
    }

    void Update()
    {
        if (!gm.GameOn)
        {
            anim.SetBool("Shooting", false);
            return;
        }

        // Determine if we should shoot based on input
        bool shootInput = gm.isPc ? Input.GetMouseButton(0) : GetControllerShootInput();

        if (shootInput)
        {
            anim.SetBool("Shooting", true);
            if (canShoot)
            {
                StartCoroutine(ShootBullet());
            }
        }
        else
        {
            anim.SetBool("Shooting", false);
        }
    }

    private bool GetControllerShootInput()
    {
        // Get the rotation input from the joystick
        float aimX = rotationJoystick.Horizontal;
        float aimZ = rotationJoystick.Vertical;
        Vector3 aimDirection = new Vector3(aimX, 0, aimZ);
        return aimDirection.magnitude > 0.1f;
    }

    private IEnumerator ShootBullet()
    {
        canShoot = false;

        // Instantiate the bullet at the gun's position with the player's rotation; 
        // using gun.forward so the bullet travels in the gun's facing direction.
        Instantiate(bullet, gun.position + gun.forward, player.rotation);

        yield return fireWait;
        canShoot = true;
    }
}
