using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Represents a single upgrade option
[System.Serializable]
public class Upgrade
{
    public string name;         // Name of the upgrade
    public string description;  // Description shown in the UI
    // Add more fields as needed (icon, effect, etc.)
}

public class Upgrades : MonoBehaviour
{
    // Enum for the different character variants
    public enum CharacterVariant { Shuriken, Sword, Ninjitsu }
    public CharacterVariant currentVariant; // The currently active character variant

    // Lists of possible upgrades for each variant, assignable in the Inspector
    public List<Upgrade> NinjitsuUpgrades;
    public List<Upgrade> ShurikenUpgrades;
    public List<Upgrade> SwordUpgrades;
    public GameObject selection;           // The UI panel for upgrade selection
    public TextMeshProUGUI[] choiceTexts;  // UI text fields for the 3 upgrade choices

    private List<Upgrade> currentChoices = new List<Upgrade>(); // The upgrades currently offered

    // Called every frame to check for level up and show upgrade choices if needed
    public void NewLevel()
    {
        ShowUpgradeChoices(); // Present upgrade choices to the player
    }

    // Returns the list of upgrades for the current character variant
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

    // Randomly selects 3 unique upgrades from the current variant's pool and displays them
    void ShowUpgradeChoices()
    {
        selection.SetActive(true);           // Show the selection UI
        currentChoices = RollUpgrades(3);    // Get 3 random upgrades

        // Update the UI text fields with the upgrade names and descriptions
        for (int i = 0; i < choiceTexts.Length; i++)
        {
            if (i < currentChoices.Count)
                choiceTexts[i].text = currentChoices[i].name + "\n" + currentChoices[i].description;
            else
                choiceTexts[i].text = "";
        }
    }

    // Returns a list of 'count' unique random upgrades from the current variant's pool
    List<Upgrade> RollUpgrades(int count)
    {
        List<Upgrade> pool = new List<Upgrade>(GetCurrentVariantUpgrades());
        List<Upgrade> result = new List<Upgrade>();
        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int idx = Random.Range(0, pool.Count);
            result.Add(pool[idx]);
            pool.RemoveAt(idx); // Remove to ensure uniqueness
        }
        return result;
    }

    // These methods are called by the UI buttons for each upgrade choice
    public void ChoiceOne() { ApplyUpgrade(0); }
    public void ChoiceTwo() { ApplyUpgrade(1); }
    public void ChoiceThree() { ApplyUpgrade(2); }

    // Applies the selected upgrade and hides the selection UI
    void ApplyUpgrade(int choiceIndex)
    {
        selection.SetActive(false); // Hide the selection UI
        if (choiceIndex < currentChoices.Count)
        {
            Upgrade chosen = currentChoices[choiceIndex];
            // Remove the chosen upgrade from the correct variant's upgrade pool
            List<Upgrade> variantUpgrades = GetCurrentVariantUpgrades();
            variantUpgrades.Remove(chosen);
            // TODO: Apply the effect of the chosen upgrade here
            Debug.Log("Chosen upgrade: " + chosen.name);

            // Example: if (chosen.name == "Kunai") kunai = true;
            // Expand this logic as needed for your upgrades
        }
    }
}
