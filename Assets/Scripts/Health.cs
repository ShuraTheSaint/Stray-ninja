using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    Upgrades up;
    public string bulletTag = "Bullet";
    public string playerTag = "Player";
    public string swordTag = "Sword";
    public string spellTag = "Fireball";
    public GameObject fire;
    public int[] damage; // [0]=bullet, [1]=sword, [2]=burn
    public int hpoints = 10;
    public Xp xp; // Assign in Inspector
    int burnDuration = 5;

    private bool isPlayerInRange = false;
    private bool isCoroutineRunning = false;
    private bool hasDied = false;
    private bool damaged = false;
    private bool burning = false;

    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    private Color[] originalColors;

    // Cached player and Damage reference
    private GameObject player;
    private Damage playerDamage;
    private static SpawnZombies spawnZombies;
    Movement move;
    Experience expS;

    private void Awake()
    {
        expS = GameObject.Find("Player").GetComponent<Experience>();
        move = GameObject.Find("Player")?.GetComponent<Movement>();
        up = GameObject.Find("Upgrade Manager")?.GetComponent<Upgrades>();
        if (spawnZombies == null) spawnZombies = GameObject.Find("Spawner")?.GetComponent<SpawnZombies>();

        if (fire != null)
        fire.SetActive(false);

        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        if (skinnedMeshRenderers != null && skinnedMeshRenderers.Length > 0)
        {
            originalColors = new Color[skinnedMeshRenderers.Length];
            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
                originalColors[i] = skinnedMeshRenderers[i].material.color;
        }
        else
        {
            Debug.LogWarning("No SkinnedMeshRenderer components found on " + gameObject.name);
        }

        // Cache player and Damage reference
        player = GameObject.Find("Player");
        if (player != null)
            playerDamage = player.GetComponent<Damage>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(bulletTag))
        {
            if (damage.Length > 0)
                ApplyDamage(damage[0]+up.kunaiDamage+up.calculatedDamage);
            
            // Only destroy the bullet if Kunai upgrade is NOT active
            if (up == null || !up.kunai)
            {
                Destroy(other.gameObject);
            }
            // else: let the bullet pass through (do not destroy)
        }

        if (other.gameObject.CompareTag(playerTag))
        {
            isPlayerInRange = true;
        }

        if (other.gameObject.CompareTag(swordTag))
        {
            if (damage.Length > 1)
                ApplyDamage(damage[1]+up.strengthDamage);
        }

        if (other.gameObject.CompareTag(spellTag))
        {
            if (!burning)
            {
                StartCoroutine(Burn());
                if (up.Combustion)
                {
                    ApplyDamage(6);
                }
                burning = true;
            }
        }
    }

    IEnumerator Burn()
    {
        if (up.hell)
        {
            burnDuration = 10;
        }
        if (fire != null)
            fire.SetActive(true);
        for (int x = 1; x <= burnDuration; x++)
        {
            if (damage.Length > 2)
                ApplyDamage(damage[2]+up.hellDamage);
            yield return new WaitForSeconds(1);
        }
        burning = false;
        if (fire != null)
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
        if (hasDied) return;

        hpoints -= amount;

        if (skinnedMeshRenderers != null && skinnedMeshRenderers.Length > 0 && !damaged)
        {
            StartCoroutine(ColorChange());
            damaged = true;
        }

        if (hpoints <= 0)
        {
            if(up.tasteofblood)
            {
                move.Tasting();
            }
            if(up.Lifesteal)
            {
                playerDamage.Heal();
            }
            if(up.Monster)
            {
                playerDamage.Monster();
            }
            hasDied = true;
            if (xp != null)
            {
                if (gameObject.name == "FastEnemy(Clone)")
                {
                    if(!up.shadowCore) xp.dropxpp(2);
                    else expS.AddExperience(4);
                }
                else if (gameObject.name == "StrongEnemy(Clone)")
                {
                    if (!up.shadowCore) xp.dropxpp(4);
                    else expS.AddExperience(6);
                }
                else if (!up.shadowCore)
                {
                    xp.dropxpp(1);
                }
                else
                {
                    expS.AddExperience(2);
                }
            }
            spawnZombies.zombieCount--;
            Destroy(gameObject);
        }
    }

    private IEnumerator ColorChange()
    {
        if (skinnedMeshRenderers != null)
        {
            foreach (var renderer in skinnedMeshRenderers)
            {
                if (renderer != null)
                    renderer.material.color = Color.red;
            }

            yield return new WaitForSeconds(0.05f);

            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                if (skinnedMeshRenderers[i] != null)
                    skinnedMeshRenderers[i].material.color = originalColors[i];
            }
        }
        damaged = false;
    }

    private IEnumerator ApplyPeriodicDamageToPlayer()
    {
        isCoroutineRunning = true;

        if (playerDamage != null && playerDamage.hp > 0)
        {
            playerDamage.hp--;
        }

        yield return new WaitForSeconds(1);
        isCoroutineRunning = false;
    }
}
