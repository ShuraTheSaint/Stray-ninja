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
    public GameObject selection;
    public TextMeshProUGUI[] choiceTexts;
    private List<UpgradesBlueprint> currentChoices = new List<UpgradesBlueprint>();

    // --- Added for queuing level-ups ---
    private int pendingLevelUps = 0;
    private bool isSelectingUpgrade = false;
    // -----------------------------------

    public void NewLevel()
    {
        // --- Prevent level up if no upgrades are available ---
        if (GetCurrentVariantUpgrades().Count == 0)
        {
            Debug.Log("No more upgrades available. Cannot level up.");
            return;
        }
        // -----------------------------------------------------

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
            Debug.Log("No more upgrades available. Cannot level up.");
            return;
        }
        // --- Set selection state ---
        isSelectingUpgrade = true;
        // ---------------------------
        selection.SetActive(true);
        currentChoices = RollUpgrades(3);

        for (int i = 0; i < choiceTexts.Length; i++)
        {
            if (i < currentChoices.Count)
                choiceTexts[i].text = currentChoices[i].name + "\n" + currentChoices[i].description;
            else
                choiceTexts[i].text = "<<<";
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
        Time.timeScale = 1f; // Unpause the game
        selection.SetActive(false);
        // --- Reset selection state ---
        isSelectingUpgrade = false;
        // ----------------------------

        if (choiceIndex < currentChoices.Count)
        {
            UpgradesBlueprint chosen = currentChoices[choiceIndex];
            List<UpgradesBlueprint> variantUpgrades = GetCurrentVariantUpgrades();
            variantUpgrades.Remove(chosen);

            Debug.Log("Chosen upgrade: " + chosen.name);
            if (chosen.name == "Kunai")
            {
                Debug.Log("Kunai upgrade applied!");
                kunaiUpgrade();
            }
            // --- Fast Hands upgrade check ---
            if (chosen.name == "Fast hands")
            {
                Debug.Log("Fast Hands upgrade applied!");
                FastHandsUpgrade();
            }
            if (chosen.name == "Calculated murder")
            {
                Debug.Log("Calculated murder upgrade applied!");
                CalculatedMurderUpgrade();
            }
            if (chosen.name == "Taste of blood")
            {
                Debug.Log("Taste of blood upgrade applied!");
                TasteofbloodUpgrade();
            }
            if (chosen.name == "Shadow core")
            {
                Debug.Log("Shadow core upgrade applied!");
                ShadowCoreUpgrade();
            }
            if (chosen.name == "Smooth throw")
            {
                Debug.Log("Smooth throw upgrade applied!");
                SmoothThrowUpgrade();
            }
            if (chosen.name == "Strength and Dexterity")
            {
                Debug.Log("Strength and Dexterity");
                StrengthAndDexterityUpgrade();
            }
            if (chosen.name == "Illusionary blade")
            {
                Debug.Log("Illusionary blade upgrade applied!");
                IllusionaryBladeUpgrade();
            }
            if (chosen.name == "Lifesteal")
            {
                Debug.Log("Lifesteal upgrade applied!");
                LifestealUpgrade();
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
    public int strengthDamage = 0;
    public int kunaiDamage = 0;
    public int calculatedDamage = 0;

    // --- Fast Hands upgrade tracking ---
    [Header("Attack Speed Upgrade")]
    public float attackSpeed = 1f; // 1 = normal, >1 = faster
    // ----------------------------------

    void kunaiUpgrade()
    {
        kunai = true;
        kunaiDamage = 1;
        Debug.Log("Attack damage increased!");
    }

    // --- Fast Hands upgrade effect ---
    void FastHandsUpgrade()
    {
        attackSpeed += 1f; // Increase attack speed by 100%
        Debug.Log("Attack speed increased!");
    }

    void CalculatedMurderUpgrade()
    {
        calculatedMurder = true;
        attackSpeed -= 0.9f; // Decrease attack speed by 90%
        calculatedDamage += 10; // Increase damage by 10
        Debug.Log("Attack speed Decreased!");
        Debug.Log("Attack damage increased!");
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
    // ---------------------------------
}
