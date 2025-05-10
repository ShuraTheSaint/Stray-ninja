using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed; // Maximum speed the player can reach
    public float rotationSpeed = 720f; // Speed at which the player rotates (degrees per second)
    public Joystick movementJoystick; // Joystick for movement
    public Joystick rotationJoystick; // Joystick for rotation
    public GameManager gm; // Reference to the GameManager

    private Rigidbody rb; // Reference to the player's Rigidbody
    private Vector3 inputDirection; // Stores the current input direction

    void Start()
    {
        // Cache the Rigidbody component
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing from the player object.");
        }

        // Ensure Rigidbody is not affected by physics forces in terms of rotation
        rb.freezeRotation = true; // Prevent rotation by physics forces
    }

    void Update()
    {
        // Set the max speed based on game state
        float maxSpeed = gm.GameOn ? speed : 0;

        if (gm.isPc)
        {
            HandlePCInput();
        }
        else
        {
            HandleJoystickInput();
        }

        // Apply movement
        MovePlayer(maxSpeed);

        if (gm.GameOn)
        {
            if (gm.isPc)
            {
                RotateWithMouse();
            }
            else
            {
                RotateWithJoystick();
            }
        }
    }

    void HandlePCInput()
    {
        // Get the movement input from keyboard
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Create a direction vector based on input
        inputDirection = new Vector3(moveX, 0, moveZ).normalized;
    }

    void HandleJoystickInput()
    {
        // Get the movement input from the first joystick 
        float moveX = movementJoystick.Horizontal;
        float moveZ = movementJoystick.Vertical;

        // Create a direction vector based on input
        inputDirection = new Vector3(moveX, 0, moveZ).normalized;
    }

    void MovePlayer(float maxSpeed)
    {
        // Move the player based on the input direction and max speed
        Vector3 targetVelocity = inputDirection * maxSpeed;
        rb.linearVelocity = targetVelocity;
    }

    void RotateWithJoystick()
    {
        // Get the rotation input from the second joystick
        float rotateX = rotationJoystick.Horizontal;
        float rotateZ = rotationJoystick.Vertical;

        // Create a direction vector based on the rotation input
        Vector3 rotationDirection = new Vector3(rotateX, 0, rotateZ);

        // Check if there's significant input for rotation
        if (rotationDirection.magnitude > 0.1f)
        {
            // Calculate the target rotation to face the input direction
            Quaternion targetRotation = Quaternion.LookRotation(rotationDirection, Vector3.up);

            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void RotateWithMouse()
    {
        // Get the mouse position in the world
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position.y);
        float distance;

        if (plane.Raycast(ray, out distance))
        {
            Vector3 target = ray.GetPoint(distance);
            Vector3 direction = target - transform.position;
            direction.y = 0; // Ensure the player only rotates around the y-axis

            if (direction.magnitude > 0.1f)
            {
                // Calculate the target rotation to face the mouse position
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
