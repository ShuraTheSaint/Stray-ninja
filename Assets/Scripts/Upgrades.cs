using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Represents a single upgrade option
[System.Serializable]
public class UpgradesBlueprint
{
    public string name;
    public string description;
    // Add more fields as needed (icon, effect, etc.)
}

public class Upgrades : MonoBehaviour
{
    public enum CharacterVariant { Shuriken, Sword, Ninjitsu }
    public CharacterVariant currentVariant;

    public List<UpgradesBlueprint> NinjitsuUpgrades;
    public List<UpgradesBlueprint> ShurikenUpgrades;
    public List<UpgradesBlueprint> SwordUpgrades;
    public GameObject[] selection;
    public GameObject SelectionCanvas;
    public MagicAttack magicAttack; // Reference to MagicAttack for cooldown management
    public TextMeshProUGUI[] choiceTexts;
    private List<UpgradesBlueprint> currentChoices = new List<UpgradesBlueprint>();

    // --- Added for queuing level-ups ---
    private int pendingLevelUps = 0;
    private bool isSelectingUpgrade = false;
    public bool levelingUp = false;
    // -----------------------------------

    public void NewLevel()
    {
        // --- Prevent level up if no upgrades are available ---
        if (GetCurrentVariantUpgrades().Count == 0)
        {
            return;
        }
        // -----------------------------------------------------
        magicAttack.afterLevelUp = true;
        levelingUp = true;

        // --- Modified to queue level-ups ---
        if (isSelectingUpgrade)
        {
            pendingLevelUps++;
        }
        else
        {
            ShowUpgradeChoices();
        }
        // -----------------------------------
    }

    List<UpgradesBlueprint> GetCurrentVariantUpgrades()
    {
        switch (currentVariant)
        {
            case CharacterVariant.Sword: return SwordUpgrades;
            case CharacterVariant.Ninjitsu: return NinjitsuUpgrades;
            case CharacterVariant.Shuriken: return ShurikenUpgrades;
            default: return new List<UpgradesBlueprint>();
        }
    }

    void ShowUpgradeChoices()
    {
        Time.timeScale = 0f; // Pause the game
        // --- Prevent level up if no upgrades are available ---
        if (GetCurrentVariantUpgrades().Count == 0)
        {
            return;
        }
        // --- Set selection state ---
        isSelectingUpgrade = true;
        // ---------------------------
        currentChoices = RollUpgrades(3);

        if (currentChoices.Count == 3)
        {
            selection[0].SetActive(true);
            selection[1].SetActive(true);
            selection[2].SetActive(true);
            SelectionCanvas.SetActive(true);
        }
        else if (currentChoices.Count == 2)
        {
            selection[0].SetActive(true);
            selection[1].SetActive(true);
            SelectionCanvas.SetActive(true);
            // Assume selection[0] and selection[1] are active
            // Set their anchored positions to be left and right of center
            RectTransform rt0 = selection[0].GetComponent<RectTransform>();
            RectTransform rt1 = selection[1].GetComponent<RectTransform>();
            float offset = 350f; // Adjust as needed for your UI

            rt0.anchoredPosition = new Vector2(-offset, 0);
            rt1.anchoredPosition = new Vector2(offset, 0);
        }
        else
        {
            selection[0].SetActive(true);
            SelectionCanvas.SetActive(true);
            RectTransform rt0 = selection[0].GetComponent<RectTransform>();
            rt0.anchoredPosition = Vector2.zero;
        }

        for (int i = 0; i < choiceTexts.Length; i++)
        {
            if (i < currentChoices.Count)
                choiceTexts[i].text = currentChoices[i].name + "\n" + currentChoices[i].description;
            else
                return; // Exit if there are fewer choices than UI elements
        }
    }

    List<UpgradesBlueprint> RollUpgrades(int count)
    {
        List<UpgradesBlueprint> pool = new List<UpgradesBlueprint>(GetCurrentVariantUpgrades());
        List<UpgradesBlueprint> result = new List<UpgradesBlueprint>();
        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int idx = Random.Range(0, pool.Count);
            result.Add(pool[idx]);
            pool.RemoveAt(idx);
        }
        return result;
    }

    public void ChoiceOne() { ApplyUpgrade(0); }
    public void ChoiceTwo() { ApplyUpgrade(1); }
    public void ChoiceThree() { ApplyUpgrade(2); }

    void ApplyUpgrade(int choiceIndex)
    {
        levelingUp = false;
        magicAttack.afterLevelUp = false;
        if (!magicAttack.onCooldown)
        {
            magicAttack.canShoot = true; // Allow shooting again after level up
        }
        Time.timeScale = 1f; // Unpause the game
        selection[0].SetActive(false);
        selection[1].SetActive(false);
        selection[2].SetActive(false);
        SelectionCanvas.SetActive(false);
        isSelectingUpgrade = false;

        if (choiceIndex < currentChoices.Count)
        {
            UpgradesBlueprint chosen = currentChoices[choiceIndex];
            List<UpgradesBlueprint> variantUpgrades = GetCurrentVariantUpgrades();
            variantUpgrades.Remove(chosen);

            if (chosen.name == "Kunai")
            {
                kunaiUpgrade();
            }
            // --- Fast Hands upgrade check ---
            if (chosen.name == "Fast hands")
            {
                FastHandsUpgrade();
            }
            if (chosen.name == "Calculated murder")
            {
                CalculatedMurderUpgrade();
            }
            if (chosen.name == "Taste of blood")
            {
                TasteofbloodUpgrade();
            }
            if (chosen.name == "Shadow core")
            {
                ShadowCoreUpgrade();
            }
            if (chosen.name == "Smooth throw")
            {
                SmoothThrowUpgrade();
            }
            if (chosen.name == "Strength and Dexterity")
            {
                StrengthAndDexterityUpgrade();
            }
            if (chosen.name == "Illusionary blade")
            {
                IllusionaryBladeUpgrade();
            }
            if (chosen.name == "Lifesteal")
            {
                LifestealUpgrade();
            }
            if (chosen.name == "Monster")
            {
                MonsterUpgrade();
            }
            if (chosen.name == "Flames of hell")
            {
                FlamesOfHellUpgrade();
            }
            if (chosen.name == "Rising sun")
            {
                MidnightSunUpgrade();
            }
            if (chosen.name == "Combustion")
            {
                CombustionUpgrade();
            }
            if (chosen.name == "Everlasting sun")
            {
                EverlastingSunUpgrade();
            }
            if(chosen.name == "Short respite")
            {
                ShortRespiteUpgrade();
            }
            // -------------------------------
        }

        // --- Handle queued level-ups ---
        if (pendingLevelUps > 0)
        {
            pendingLevelUps--;
            ShowUpgradeChoices();
        }
        // ------------------------------
    }

    [Header("Upgrade Tracking")]
    public Movement playerMovement;
    public GameObject sword;
    public bool kunai;
    public bool calculatedMurder;
    public bool tasteofblood;
    public bool smoothThrow;
    public bool shadowCore;
    public bool Lifesteal;
    public bool Monster;
    public bool hell;
    public bool Combustion;
    public bool MidnightSun;
    public int strengthDamage = 0;
    public int kunaiDamage = 0;
    public int calculatedDamage = 0;
    public int hellDamage = 0;
    public int SunDuration = 0;
    public int shortRespite = 0; // Placeholder for Short Respite upgrade

    // --- Fast Hands upgrade tracking ---
    [Header("Attack Speed Upgrade")]
    public float attackSpeed = 1f; // 1 = normal, >1 = faster
    // ----------------------------------

    void kunaiUpgrade()
    {
        kunai = true;
        kunaiDamage = 1;
    }

    // --- Fast Hands upgrade effect ---
    void FastHandsUpgrade()
    {
        attackSpeed += 1f; // Increase attack speed by 100%
    }

    void CalculatedMurderUpgrade()
    {
        calculatedMurder = true;
        attackSpeed -= 0.9f; // Decrease attack speed by 90%
        calculatedDamage += 10; // Increase damage by 10
    }
    void TasteofbloodUpgrade()
    {
        tasteofblood = true;
    }

    void ShadowCoreUpgrade()
    {
        shadowCore = true;
    }

    void SmoothThrowUpgrade()
    {
        smoothThrow = true;
    }
    
    void StrengthAndDexterityUpgrade()
    {
        strengthDamage += 5; // Increase damage by 5
        playerMovement.rotationSpeed += 100f; // Increase rotation speed by 100
    }

    void IllusionaryBladeUpgrade()
    {
        // --- Increase sword size ---
        sword.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f); // Increase sword size by 50%
    }

    void LifestealUpgrade()
    {
        Lifesteal = true;
    }

    void MonsterUpgrade()
    { 
        Monster = true;
    }
    void FlamesOfHellUpgrade()
    { 
        hell = true;
        hellDamage = 2;
    }
    void MidnightSunUpgrade()
    {
        MidnightSun = true;
    }
    void CombustionUpgrade()
    {
        Combustion = true;
    }
    void EverlastingSunUpgrade()
    {
        SunDuration = 10;
    }

    void ShortRespiteUpgrade()
    {
        shortRespite = 3; // Increase short respite duration by 3 seconds
    }
    // ---------------------------------
}
