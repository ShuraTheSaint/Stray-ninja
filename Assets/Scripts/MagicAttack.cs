using System.Collections;
using UnityEngine;
using TMPro;

public class MagicAttack : MonoBehaviour
{
    public bool canShoot = true;
    public bool shouldExtinguish = false;
    public bool coolDown = false;
    public int fireballCd = 5;
    private int tempCD;
    public GameObject bullet;
    public Transform gun;
    public Transform player;
    public GameManager gm;
    public TextMeshProUGUI CD;
    public GameObject cdtext;
    public Joystick rotationJoystick; // Reference to the rotation joystick

    void Start()
    {
        tempCD = fireballCd;
    }

    void Update()
    {
        // Process input only when the game is active.
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

        // Start the cooldown process if triggered.
        if (coolDown)
        {
            coolDown = false;
            cdtext.SetActive(true);
            StartCoroutine(CoolDown());
        }
    }

    private void HandleMouseMagicInput()
    {
        // For PC: Fire once per click.
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
            // No input – set flag to allow future shots if needed.
            if (!shouldExtinguish)
            {
                shouldExtinguish = true;
            }
        }
    }

    private void HandleJoystickMagicInput()
    {
        // For Mobile: Read the joystick input.
        float aimX = rotationJoystick.Horizontal;
        float aimZ = rotationJoystick.Vertical;
        Vector3 aimDirection = new Vector3(aimX, 0, aimZ).normalized;

        if (canShoot && aimDirection.magnitude > 0.1f)
        {
            shouldExtinguish = false;
            Instantiate(bullet, gun.position + transform.forward, player.rotation);
            canShoot = false;
        }
        else if (!shouldExtinguish && aimDirection.magnitude == 0)
        {
            shouldExtinguish = true;
        }
    }

    IEnumerator CoolDown()
    {
        while (tempCD > 0)
        {
            CD.text = tempCD.ToString();
            yield return new WaitForSeconds(1);
            tempCD--;
        }
        cdtext.SetActive(false);
        tempCD = fireballCd;
        canShoot = true;
    }
}
