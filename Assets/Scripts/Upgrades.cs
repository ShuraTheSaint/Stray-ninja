using TMPro;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    int option;
    public int LevelUp;
    public int PastLV = 1;
    public GameObject selection;
    public TextMeshProUGUI text;

    public bool kunai;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PastLV<LevelUp)
        {
            selection.SetActive(true);
            PastLV = LevelUp;
            random();
        }
    }

    void random()
    {
        option = Random.Range(1, 1);
        if(option == 1)
        {
            text.text = "Kunai";
        }
    }

    public void ChoiceOne()
    {
        selection.SetActive(false);
        if (option == 1)
        {
            kunai = true;
        }
    }
    
    public void ChoiceTwo()
    {
        selection.SetActive(false);
    }

    public void ChoiceThree()
    {
        selection.SetActive(false);
    }


}
