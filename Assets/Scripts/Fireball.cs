using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public Rigidbody rigidb;
    MagicAttack wand;
    public ParticleSystem fireEffects; // Particle system for fire effects
    public float followSpeed; // Speed at which the object follows the target
    Joystick rotationJoystick; // Reference to the rotation joystick
    private static Upgrades up; // Static cache

    private Camera mainCamera;
    private const float movementThresholdSqr = 0.01f;

    public float aimAssistRadius = 3f;
    public float assistAngleThreshold;
    [Range(0f, 1f)]
    public float aimAssistStrength = 0.2f;

    private Transform currentAssistTarget = null; // Track the current assisted enemy
    private HashSet<Transform> hitEnemies = new HashSet<Transform>(); // Track hit enemies

    void Awake()
    {
        AudioManager.Instance.Play("Ignite");
        AudioManager.Instance.PlayDelayed("Burn", 0.5f);
        if (up == null) up = GameObject.Find("Upgrade Manager")?.GetComponent<Upgrades>();
        wand = GameObject.Find("Player").GetComponent<MagicAttack>();
        rigidb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;

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
        if (up.hell)
        {
            if (fireEffects != null)
            {
                var BlueFlames = fireEffects.main;
                BlueFlames.startColor = new ParticleSystem.MinMaxGradient(new Color(0f, 0.1f, 0.7f)); // blue
            }
        }
    }

    IEnumerator risingSun()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.transform.localScale += new Vector3(0.15f, 0.15f, 0.15f);
        StartCoroutine(risingSun());
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(5 + up.SunDuration);
        Extinguish();
    }

    void Extinguish()
    {
        wand.coolDown = true;
        AudioManager.Instance.Stop("Ignite");
        AudioManager.Instance.Stop("Burn");
        Destroy(gameObject);
    }

    void Update()
    {
        if (up.levelingUp == true)
        {
            Extinguish();
            return;
        }
        if (Time.timeSinceLevelLoad < 0.1f)
        {
            return;
        }
        if (wand.shouldExtinguish == true)
        {
            wand.shouldExtinguish = false;
            Extinguish();
        }

        Vector3 inputDirection = Vector3.zero;

        if (wand.gm.isPc)
        {
            // For PC: Use the mouse cursor as the target. No aim assist.
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, new Vector3(0, 0, 0));
            if (plane.Raycast(ray, out float distance))
            {
                Vector3 targetPoint = ray.GetPoint(distance);
                Vector3 offset = targetPoint - transform.position;
                if (offset.sqrMagnitude > movementThresholdSqr)
                {
                    inputDirection = offset.normalized;
                }
            }
            if (inputDirection != Vector3.zero)
            {
                Vector3 step = inputDirection * (followSpeed) * Time.deltaTime;
                rigidb.MovePosition(transform.position + step);
            }
        }
        else
        {
            // For Mobile: Use joystick input and improved aim assist
            float moveX = rotationJoystick.Horizontal;
            float moveZ = rotationJoystick.Vertical;
            Vector3 direction = new Vector3(moveX, 0, moveZ);
            if (direction.magnitude > 0.1f)
            {
                inputDirection = direction.normalized;
            }

            Vector3 assistDirection = GetAssistDirection();
            Vector3 finalDirection = inputDirection;

            if (assistDirection != Vector3.zero && inputDirection != Vector3.zero)
            {
                float angle = Vector3.Angle(inputDirection, assistDirection);

                if (angle < assistAngleThreshold)
                {
                    // Blend more as the directions align, less as they diverge
                    float t = Mathf.Clamp01(1f - (angle / assistAngleThreshold));
                    float blend = aimAssistStrength * t;
                    finalDirection = Vector3.Lerp(inputDirection, assistDirection, blend).normalized;
                }
                // else: don't blend, use pure input
            }
            else if (assistDirection != Vector3.zero)
            {
                finalDirection = assistDirection;
            }

            if (finalDirection != Vector3.zero)
            {
                Vector3 step = finalDirection * (followSpeed) * 0.5f * Time.deltaTime;
                rigidb.MovePosition(transform.position + step);
            }
        }
    }

    // Finds the nearest enemy within aimAssistRadius and returns a normalized direction vector toward it
    Vector3 GetAssistDirection()
    {
        // If we have a current assist target, check if it's still valid
        if (currentAssistTarget != null)
        {
            if (hitEnemies.Contains(currentAssistTarget) ||
                Vector3.Distance(transform.position, currentAssistTarget.position) > aimAssistRadius)
            {
                currentAssistTarget = null;
            }
        }

        // If no current assist target, find a new one
        if (currentAssistTarget == null)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, aimAssistRadius);
            Transform nearest = null;
            float minDist = float.MaxValue;
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Enemy") && !hitEnemies.Contains(hit.transform))
                {
                    float dist = (hit.transform.position - transform.position).sqrMagnitude;
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearest = hit.transform;
                    }
                }
            }
            currentAssistTarget = nearest;
        }

        if (currentAssistTarget != null)
        {
            Vector3 toEnemy = currentAssistTarget.position - transform.position;
            toEnemy.y = 0;
            if (toEnemy.magnitude > 0.01f)
                return toEnemy.normalized;
        }
        return Vector3.zero;
    }

    // Stop aim assist for the enemy after hit
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            hitEnemies.Add(other.transform);
            if (currentAssistTarget == other.transform)
            {
                currentAssistTarget = null;
            }
        }
    }
}