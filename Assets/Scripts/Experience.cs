using UnityEngine;
using TMPro;

public class Experience : MonoBehaviour
{
    public Upgrades upgrades;
    public int level = 1;
    public float exp = 0;
    public float expReq = 10;
    private TextMeshProUGUI EXPtext;
    private TextMeshProUGUI LVtext;

    private void Start()
    {
        LVtext = GameObject.Find("LVN").GetComponent<TextMeshProUGUI>();
        EXPtext = GameObject.Find("EXPN").GetComponent<TextMeshProUGUI>();
        UpdateUI();
    }

    // Call this method whenever you want to add experience
    public void AddExperience(float amount)
    {
        exp += amount;
        bool leveledUp = false;
        while (exp >= expReq)
        {
            exp -= expReq;
            level++;
            expReq = Mathf.Round(expReq * 1.1f + 10);
            leveledUp = true;
            upgrades.NewLevel();
        }
        UpdateUI();
        if (leveledUp)
        {
            ChangeText(level.ToString());
        }
    }

    private void UpdateUI()
    {
        if (EXPtext != null)
            EXPtext.text = exp.ToString("F0");
        if (LVtext != null)
            LVtext.text = level.ToString();
    }

    public void ChangeText(string newText)
    {
        if (LVtext != null)
            LVtext.text = newText;
    }
}
