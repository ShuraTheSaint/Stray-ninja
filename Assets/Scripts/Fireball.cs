using System.Collections;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public Rigidbody rigidb;
    MagicAttack wand;
    public float fixedYa = 0f; // Fixed Y position for the object
    public float followSpeed; // Speed at which the object follows the target
    Joystick rotationJoystick; // Reference to the rotation joystick
    private static Upgrades up; // Static cache

    private Camera mainCamera;
    // Movement threshold: if the squared distance is below this value, we do not move.
    private const float movementThresholdSqr = 0.01f; // (0.1f^2)

    void Awake()
    {
        if (up == null) up = GameObject.Find("Upgrade Manager")?.GetComponent<Upgrades>();
        // Get a reference to the MagicAttack script on the Player.
        wand = GameObject.Find("Player").GetComponent<MagicAttack>();
        rigidb = GetComponent<Rigidbody>();

        // Cache the main camera for PC.
        mainCamera = Camera.main;

        // Assign joystick only for mobile input.
        if (wand.gm.isPc)
        {
            rotationJoystick = null;
        }
        else
        {
            rotationJoystick = GameObject.Find("Aim").GetComponent<Joystick>();
        }
        StartCoroutine(Fire());
        if (up.MidnightSun)
        {
            StartCoroutine(risingSun());
        }
    }

    IEnumerator risingSun()
    {
        Debug.Log("NewHour");
        yield return new WaitForSeconds(0.5f);
        gameObject.transform.localScale += new Vector3(0.15f, 0.15f, 0.15f); // Increase size by 50%
        StartCoroutine(risingSun());
    }

    IEnumerator Fire()
    {
        // Wait 5 seconds before extinguishing the fireball.
        yield return new WaitForSeconds(5+up.SunDuration);
        Extinguish();
    }

    void Extinguish()
    {
        // Signal the cooldown in the MagicAttack script then destroy this fireball.
        wand.coolDown = true;
        Destroy(gameObject);
    }

    void Update()
    {
        // Skip processing during the first frame.
        if (Time.timeSinceLevelLoad < 0.1f)
        {
            return;
        }
        // Check if MagicAttack indicated the fireball should be extinguished.
        if (wand.shouldExtinguish == true)
        {
            wand.shouldExtinguish = false;
            Extinguish();
        }

        // Process input based on platform.
        if (wand.gm.isPc)
        {
            // For PC: Use the mouse cursor as the target.
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, new Vector3(0, fixedYa, 0));
            if (plane.Raycast(ray, out float distance))
            {
                Vector3 targetPoint = ray.GetPoint(distance);
                Vector3 offset = targetPoint - transform.position;

                // Move only if the offset is above the threshold.
                if (offset.sqrMagnitude > movementThresholdSqr)
                {
                    Vector3 direction = offset.normalized;
                    Vector3 step = direction * (followSpeed) * Time.deltaTime;
                    rigidb.MovePosition(transform.position + step);
                }
            }
        }
        else
        {
            // For Mobile: Use joystick input.
            float moveX = rotationJoystick.Horizontal;
            float moveZ = rotationJoystick.Vertical;
            Vector3 direction = new Vector3(moveX, 0, moveZ);
            if (direction.magnitude > 0.1f)
            {
                direction = direction.normalized;
                Vector3 step = direction * (followSpeed) * Time.deltaTime;
                rigidb.MovePosition(transform.position + step);
            }
        }
    }
}
