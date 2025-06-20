using System.Collections;
using UnityEngine;
using TMPro;

public class MagicAttack : MonoBehaviour
{
    public Upgrades up; // Reference to Upgrades for cooldown management
    // Can the player shoot a fireball right now?
    public bool canShoot = true;

    // Used to prevent holding down the button for continuous fire
    public bool shouldExtinguish = false;

    // Triggers the cooldown process
    public bool coolDown = false;

    // Is currently on cooldown
    public bool onCooldown = false;

    // Fireball cooldown duration in seconds
    public int fireballCd = 5;

    // Tracks remaining cooldown time
    public int tempCD;

    // If true, skip cooldown after leveling up
    public bool afterLevelUp = false;

    // Fireball prefab to spawn
    public GameObject bullet;

    // Where the fireball spawns from
    public Transform gun;

    // Reference to player (for rotation)
    public Transform player;

    // Reference to GameManager for game state
    public GameManager gm;

    // UI text for cooldown timer
    public TextMeshProUGUI CD;

    // UI GameObject for cooldown display
    public GameObject cdtext;

    // Reference to the rotation joystick (for mobile)
    public Joystick rotationJoystick;

    void Start()
    {
        up = GameObject.Find("Upgrade Manager")?.GetComponent<Upgrades>();
        // Set cooldown timer to default at start
        tempCD = fireballCd;
    }

    void Update()
    {
        // Only allow input if the game is running
        if (gm.GameOn)
        {
            if (gm.isPc)
            {
                HandleMouseMagicInput();
            }
            else
            {
                HandleJoystickMagicInput();
            }
        }

        // If cooldown is triggered, start cooldown process
        if (coolDown)
        {
            if (!afterLevelUp)
            {
                coolDown = false;
                cdtext.SetActive(true);
                StartCoroutine(CoolDown());
            }
            else
            {
                // If just leveled up, skip cooldown and allow shooting
                coolDown = false;
            }
        }
    }

    // Handles magic attack input for PC (mouse)
    private void HandleMouseMagicInput()
    {
        // Fire once per mouse click
        if (Input.GetMouseButton(0))
        {
            if (canShoot)
            {
                Instantiate(bullet, gun.position + transform.forward, player.rotation);
                canShoot = false;
                shouldExtinguish = false;
            }
        }
        else
        {
            // Allow shooting again after mouse is released
            if (!shouldExtinguish)
            {
                shouldExtinguish = true;
            }
        }
    }

    // Handles magic attack input for mobile (joystick)
    private void HandleJoystickMagicInput()
    {
        // Get joystick direction
        float aimX = rotationJoystick.Horizontal;
        float aimZ = rotationJoystick.Vertical;
        Vector3 aimDirection = new Vector3(aimX, 0, aimZ).normalized;

        // Fire if joystick is pushed far enough
        if (canShoot && aimDirection.magnitude > 0.1f)
        {
            shouldExtinguish = false;
            Instantiate(bullet, gun.position + transform.forward, player.rotation);
            canShoot = false;
        }
        // Allow shooting again after joystick is released
        else if (!shouldExtinguish && aimDirection.magnitude == 0)
        {
            shouldExtinguish = true;
        }
    }

    // Handles the cooldown timer and UI
    IEnumerator CoolDown()
    {
        onCooldown = true;
        tempCD = fireballCd - up.shortRespite;
        while (tempCD > 0)
        {
            CD.text = tempCD.ToString();
            yield return new WaitForSeconds(1);
            tempCD--;
        }
        cdtext.SetActive(false);
        tempCD = fireballCd - up.shortRespite;
        canShoot = true;
        onCooldown = false;
    }
}
