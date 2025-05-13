using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

// Represents a single upgrade option
[System.Serializable]
public class Upgrade
{
    public string name;
    public string description;
    // Add more fields as needed (icon, effect, etc.)
}

public class Upgrades : MonoBehaviour
{
    public enum CharacterVariant { Shuriken, Sword, Ninjitsu }
    public CharacterVariant currentVariant;

    public List<Upgrade> NinjitsuUpgrades;
    public List<Upgrade> ShurikenUpgrades;
    public List<Upgrade> SwordUpgrades;
    public GameObject selection;
    public TextMeshProUGUI[] choiceTexts;
    private List<Upgrade> currentChoices = new List<Upgrade>();

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

    List<Upgrade> GetCurrentVariantUpgrades()
    {
        switch (currentVariant)
        {
            case CharacterVariant.Sword: return SwordUpgrades;
            case CharacterVariant.Ninjitsu: return NinjitsuUpgrades;
            case CharacterVariant.Shuriken: return ShurikenUpgrades;
            default: return new List<Upgrade>();
        }
    }

    void ShowUpgradeChoices()
    {
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

    List<Upgrade> RollUpgrades(int count)
    {
        List<Upgrade> pool = new List<Upgrade>(GetCurrentVariantUpgrades());
        List<Upgrade> result = new List<Upgrade>();
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
        selection.SetActive(false);
        // --- Reset selection state ---
        isSelectingUpgrade = false;
        // ----------------------------

        if (choiceIndex < currentChoices.Count)
        {
            Upgrade chosen = currentChoices[choiceIndex];
            List<Upgrade> variantUpgrades = GetCurrentVariantUpgrades();
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
    public bool kunai;
    public bool calculatedMurder;
    public int kunaiDamage = 0;
    public int calculatedDamage = 0;

    // --- Fast Hands upgrade tracking ---
    [Header("Attack Speed Upgrade")]
    public float attackSpeed = 1f; // 1 = normal, >1 = faster
    // ----------------------------------

    void kunaiUpgrade()
    {
        kunai = true;
        kunaiDamage = 5;
        Debug.Log("Attack damage increased!");
    }

    // --- Fast Hands upgrade effect ---
    public void FastHandsUpgrade()
    {
        attackSpeed += 1f; // Increase attack speed by 100%
        Debug.Log("Attack speed increased!");
    }

    public void CalculatedMurderUpgrade()
    {
        calculatedMurder = true;
        attackSpeed -= 0.9f; // Decrease attack speed by 90%
        calculatedDamage += 10; // Increase damage by 5
        Debug.Log("Attack speed Decreased!");
        Debug.Log("Attack damage increased!");
    }
    // ---------------------------------
}
