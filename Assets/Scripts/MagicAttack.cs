using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MagicAttack : MonoBehaviour
{
    private GameManager gameManager; // Reference to the GameManager
    public bool canShoot = true;
    public bool Extinguish = false;
    public bool coolDown = false;
    public int fireballCd = 5;
    private string cdUI;
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
        // Check if the game is on
        if (gm.GameOn)
        {
            // Get the input from the rotation joystick
            float aimX = rotationJoystick.Horizontal;
            float aimZ = rotationJoystick.Vertical;

            // Create a direction vector based on the joystick input
            Vector3 aimDirection = new Vector3(aimX, 0, aimZ).normalized;

            // Check if there's significant input for aiming and if the player can shoot
            if (canShoot && aimDirection.magnitude > 0.1f)
            {
                Extinguish = false; // Ensure we are not extinguishing right after shooting
                // Instantiate the bullet
                Instantiate(bullet, gun.position + transform.forward, player.rotation);
                canShoot = false;
                // Start cooldown after shooting
            }
            else if (Extinguish == false && aimDirection.magnitude == 0)
            {
                Extinguish = true;
            }
        }

        // Handle the cooldown process
        if (coolDown)
        {
            coolDown = false;
            cdtext.SetActive(true);
            StartCoroutine(CoolDown());
        }
    }

    IEnumerator CoolDown()
    {
        for (int x = 1; x <= fireballCd; x++)
        {
            cdUI = tempCD.ToString();
            CD.text = cdUI;
            yield return new WaitForSeconds(1);
            tempCD -= 1;
        }
        cdtext.SetActive(false);
        tempCD = fireballCd;
        canShoot = true;
    }
}
