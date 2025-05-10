using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    Upgrades up;
    public string bulletTag = "Bullet";
    public string playerTag = "Player";
    public string swordTag = "Sword";
    public string spellTag = "Fireball";
    public GameObject fire;
    public int[] damage;
    public int hpoints = 10;
    public Xp xp; // Ensure this is assigned in the Inspector or elsewhere in the code
    private bool isPlayerInRange = false;
    private bool isCoroutineRunning = false;
    private bool hasDied = false;
    private bool damaged = false;
    bool burning = false;
    public int burnDuration = 5;

    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    private Color[] originalColors; // Array to store original colors of each SkinnedMeshRenderer

    private void Awake()
    {
        up = GameObject.Find("Upgrade Manager").GetComponent<Upgrades>();
        fire.SetActive(false);
        // Get all SkinnedMeshRenderer components attached to this GameObject or its children
        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        if (skinnedMeshRenderers != null && skinnedMeshRenderers.Length > 0)
        {
            // Initialize the array to store original colors
            originalColors = new Color[skinnedMeshRenderers.Length];

            // Store the original color of each SkinnedMeshRenderer
            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                originalColors[i] = skinnedMeshRenderers[i].material.color;
            }
        }
        else
        {
            Debug.LogWarning("No SkinnedMeshRenderer components found on " + gameObject.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(bulletTag))
        {
            ApplyDamage(damage[0]);
            if (up.kunai == false)
            {
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.CompareTag(playerTag))
        {
            isPlayerInRange = true;
        }

        if (other.gameObject.CompareTag(swordTag))
        {
            ApplyDamage(damage[1]);  // Apply sword damage immediately
        }

        if (other.gameObject.CompareTag(spellTag))
        {
            if (!burning)
            {
                StartCoroutine(burn());
                burning = true;
            }
        }
    }

    IEnumerator burn()
    {
        fire.SetActive(true);
        for (int x=1; x<=burnDuration; x++)
        {
            ApplyDamage(damage[2]);
            yield return new WaitForSeconds(1);
        }
        burning = false;
        fire.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            isPlayerInRange = false;
        }
    }

    private void Update()
    {
        if (isPlayerInRange && !isCoroutineRunning)
        {
            StartCoroutine(ApplyPeriodicDamageToPlayer());
        }
    }

    private void ApplyDamage(int amount)
    {
        if (!hasDied)
        {
            hpoints -= amount;

            if (skinnedMeshRenderers != null && skinnedMeshRenderers.Length > 0 && !damaged)
            {
                StartCoroutine(ColorChange());
                damaged = true;
            }

            if (hpoints <= 0)
            {
                hasDied = true;
                xp.dropxpp();
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator ColorChange()
    {
        if (skinnedMeshRenderers != null)
        {
            // Change each SkinnedMeshRenderer's color to red
            foreach (var renderer in skinnedMeshRenderers)
            {
                if (renderer != null)
                {
                    renderer.material.color = Color.red;
                }
            }

            yield return new WaitForSeconds(0.05f);

            // Revert each SkinnedMeshRenderer's color to its original color
            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                if (skinnedMeshRenderers[i] != null)
                {
                    skinnedMeshRenderers[i].material.color = originalColors[i];
                }
            }
        }

        damaged = false;
    }

    private IEnumerator ApplyPeriodicDamageToPlayer()
    {
        isCoroutineRunning = true;

        GameObject player = GameObject.Find("Player");
        Damage playerDamage = player?.GetComponent<Damage>();

        if (playerDamage != null && playerDamage.hp > 0)
        {
            playerDamage.hp--;
        }

        yield return new WaitForSeconds(1);
        isCoroutineRunning = false;
    }
}
