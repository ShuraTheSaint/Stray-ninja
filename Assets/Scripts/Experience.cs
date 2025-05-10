using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Experience : MonoBehaviour
{
    public Upgrades upgrades;
    public int level = 1;
    public float exp = 0;
    public float expReq = 10;
    public string Tag = "Exp";
    TextMeshProUGUI EXPtext;
    TextMeshProUGUI LVtext;

    private void Start()
    {
        LVtext = GameObject.Find("LVN").GetComponent<TextMeshProUGUI>();
        EXPtext = GameObject.Find("EXPN").GetComponent<TextMeshProUGUI>();
    }

    public void Update()
    {
        upgrades.LevelUp = level;
        EXPtext.text = exp.ToString();
        if (exp >= expReq)
        {
            level = level + 1;
            exp = exp - expReq;
            expReq = Mathf.Round(expReq * 1.1f + 10);
            ChangeText(level.ToString());
        }
    }
    public void ChangeText(string newText)
    {
        LVtext.text = newText;
    }
}