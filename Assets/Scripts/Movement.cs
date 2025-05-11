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
    private Vector3 pcMovementInput; // Stores PC input direction
    private Vector3 joystickMovementInput; // Stores joystick input direction
    private Camera mainCamera; // Cached main camera for mouse rotation

    void Start()
    {
        // Cache the Rigidbody component
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing from the player object.");
        }
        rb.freezeRotation = true; // Prevent rotation by physics forces

        // Cache the main camera to avoid repeated calls 
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!gm.GameOn)
            return;

        if (gm.isPc)
        {
            pcMovementInput = GetPCInput();
            RotateWithMouse();
        }
        else
        {
            joystickMovementInput = GetJoystickInput();
            RotateWithJoystick();
        }
    }

    void FixedUpdate()
    {
        if (!gm.GameOn)
            return;

        if (gm.isPc)
        {
            if (pcMovementInput != Vector3.zero)
            {
                // Using AddForce in FixedUpdate for more consistent physics behavior
                rb.AddForce(pcMovementInput.normalized * speed * 110);
            }
        }
        else
        {
            // Directly set velocity for the joystick movement
            rb.linearVelocity = joystickMovementInput * speed;
        }
    }

    Vector3 GetPCInput()
    {
        // Build a movement direction vector from the individual keys
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) direction += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) direction += Vector3.back;
        if (Input.GetKey(KeyCode.A)) direction += Vector3.left;
        if (Input.GetKey(KeyCode.D)) direction += Vector3.right;
        return direction;
    }

    Vector3 GetJoystickInput()
    {
        // Get the movement input from the joystick and normalize it
        float moveX = movementJoystick.Horizontal;
        float moveZ = movementJoystick.Vertical;
        return new Vector3(moveX, 0, moveZ).normalized;
    }

    void RotateWithJoystick()
    {
        // Get the rotation input from the second joystick
        float rotateX = rotationJoystick.Horizontal;
        float rotateZ = rotationJoystick.Vertical;
        Vector3 rotationDirection = new Vector3(rotateX, 0, rotateZ);

        if (rotationDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rotationDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void RotateWithMouse()
    {
        // Get the mouse position in the world using the cached main camera
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position.y);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 target = ray.GetPoint(distance);
            Vector3 direction = target - transform.position;
            direction.y = 0;
            if (direction.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
