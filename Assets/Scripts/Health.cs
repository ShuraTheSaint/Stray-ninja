using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    Upgrades up;
    public string bulletTag = "Bullet";
    public string playerTag = "Player";
    public string swordTag = "Sword";
    public string spellTag = "Fireball";
    public int OriginalSpeed; // Speed of the enemy
    public GameObject fire;
    public ParticleSystem fireFX;
    public GameObject Explosion;
    public int[] damage; // [0]=bullet, [1]=sword, [2]=burn
    public int hpoints = 10;
    public Enemycontroller enemyController; // Assign in Inspector
    public Xp xp; // Assign in Inspector
    int burnDuration = 5;
    float burnTick = 1;
    public static int burnSound = 0;

    private bool isPlayerInRange = false;
    private bool isCoroutineRunning = false;
    private bool hasDied = false;
    private bool damaged = false;
    private bool burning = false;
    private bool UpgradeAplied = false; // Flag to check if upgrade is applied

    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    private Color[] originalColors;

    // Cached player and Damage reference
    private GameObject player;
    private Damage playerDamage;
    private static SpawnZombies spawnZombies;
    Movement move;
    Experience expS;


    public void Awake()
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
            AudioManager.Instance.PlayOneShot("Hit");
            enemyController.speed = 0; // Stop enemy movement when hit by a bullet
            ApplyDamage(damage[0] + up.kunaiDamage + up.calculatedDamage);

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
            ApplyDamage(damage[1] + up.strengthDamage);
            AudioManager.Instance.PlayOneShot("Slash");
        }

        if (other.gameObject.CompareTag(spellTag))
        {
            if (!burning)
            {
                burning = true; // Set burning first to avoid race conditions
                burnSound++;
                if (burnSound == 1)
                {
                    AudioManager.Instance.Play("Fire");
                }
                StartCoroutine(Burn());
                if (up.Combustion)
                {
                    ApplyDamage(6);
                    Explosion.SetActive(true);
                }
            }
        }
    }

    IEnumerator Burn()
    {
        if (up.hell && !UpgradeAplied)
        {
            burnTick = 0.3f;
            if (fireFX != null)
            {
                var hellFlames = fireFX.main;
                hellFlames.startColor = new ParticleSystem.MinMaxGradient(new Color(0f, 0.1f, 0.7f)); // blue
            }
            else
            {
                Debug.LogWarning("fireFX is not assigned on " + gameObject.name);
            }
            UpgradeAplied = true;
        }
        fire.SetActive(true);
        yield return new WaitForSeconds(burnTick);
        for (int x = 1; x <= burnDuration; x++)
        {
            ApplyDamage(damage[2] + up.hellDamage);
            yield return new WaitForSeconds(burnTick);
        }
        burning = false;
        fire.SetActive(false);

        burnSound--;
        if (burnSound < 0) burnSound = 0;
        if (burnSound == 0)
        {
            AudioManager.Instance.Stop("Fire");
        }
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
            if (burning)
            {
                burning = false;
                burnSound--;
                if (burnSound < 0) burnSound = 0;
                Debug.Log("Burn sound count: " + burnSound);
                if (burnSound == 0)
                {
                    AudioManager.Instance.Stop("Fire");
                }
            }
            if (up.tasteofblood)
            {
                move.Tasting();
            }
            if (up.Lifesteal)
            {
                playerDamage.Heal();
            }
            if (up.Monster)
            {
                playerDamage.Monster();
            }
            hasDied = true;
            if (xp != null)
            {
                if (gameObject.name == "FastEnemy(Clone)")
                {
                    if (!up.shadowCore) xp.dropxpp(2);
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
        enemyController.speed = OriginalSpeed; // Resume enemy movement
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