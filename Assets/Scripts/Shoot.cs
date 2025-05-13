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
    Upgrades up;

    private WaitForSeconds fireWait; // Cached WaitForSeconds for fire interval
    private float lastAttackSpeed = 1f; // Track last attack speed to update fireWait if needed

    void Start()
    {
        up = GameObject.Find("Upgrade Manager")?.GetComponent<Upgrades>();
        // Cache the fire interval so we don't allocate every shot
        fireWait = new WaitForSeconds(1f / fireRate);
        lastAttackSpeed = up != null ? up.attackSpeed : 1f;
    }

    void Update()
    {
        // --- Update fireWait if attackSpeed changes ---
        if (up != null && up.attackSpeed != lastAttackSpeed)
        {
            lastAttackSpeed = up.attackSpeed;
            fireWait = new WaitForSeconds(1f / (fireRate * lastAttackSpeed));
        }
        // ---------------------------------------------

        if (!gm.GameOn)
        {
            anim.SetBool("Shooting", false);
            return;
        }

        bool shootInput;
        // Determine if we should shoot based on input
        if (up.calculatedMurder)
        {
            shootInput = gm.isPc ? Input.GetMouseButtonDown(0) : GetControllerShootInput();
        }else shootInput = gm.isPc ? Input.GetMouseButton(0) : GetControllerShootInput();

        if (shootInput)
        {
            if (canShoot)
            {
                anim.SetBool("Shooting", true);
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
