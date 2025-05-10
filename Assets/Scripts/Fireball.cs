using System.Collections;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public Rigidbody rigidb;
    MagicAttack wand;
    public float fixedYa = 0f; // Fixed Y position for the object
    public float followSpeed = 2000f; // Speed at which the object follows the joystick input
    public Joystick rotationJoystick; // Reference to the rotation joystick

    void Awake()
    {
        wand = GameObject.Find("Player").GetComponent<MagicAttack>();
        rigidb = GetComponent<Rigidbody>();
        rotationJoystick = GameObject.Find("Aim").GetComponent<FloatingJoystick>();
        StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(5); // Wait for 5 seconds before extinguishing the fireball
        Extinguish();
    }

    void Extinguish()
    {
        wand.coolDown = true; // Signal the cooldown to the MagicAttack script
        Destroy(gameObject); // Destroy the fireball
    }

    void Update()
    {
        // Prevent immediate extinguishing after creation
        if (Time.timeSinceLevelLoad < 0.1f)
        {
            return; // Skip the initial frame to avoid immediate extinguish
        }

        // Check if the MagicAttack script wants to extinguish the fireball
        if (wand.Extinguish == true)
        {
            wand.Extinguish = false;
            Extinguish();
        }

        // Get the input from the rotation joystick
        float moveX = rotationJoystick.Horizontal;
        float moveZ = rotationJoystick.Vertical;

        // Create a direction vector based on the joystick input
        Vector3 direction = new Vector3(moveX, 0, moveZ).normalized;

        // Move the fireball only if there's significant joystick input
        if (direction.magnitude > 0.1f)
        {
            Vector3 step = direction * followSpeed * Time.deltaTime;
            rigidb.MovePosition(transform.position + step);
        }
    }
}
