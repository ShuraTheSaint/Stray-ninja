using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Damage : MonoBehaviour
{
    public float hp;
    public GameManager gm;
    TextMeshProUGUI HealthUI;


    private void Start()
    {
        
        HealthUI = GameObject.Find("HPN").GetComponent<TextMeshProUGUI>();
    }
    public void Awake()
    {
        hp = 5;
        name = "Player";
    }

    void Update()
    {

        if (hp==0)
        {
            gm.playerDead();
        }
        HealthUI.text = hp.ToString();
    }

    public void Heal()
    {
        int HealNumb = Random.Range(0, 20);
        if(HealNumb==1&&hp<5)
        {
            hp++;
            Debug.Log("healed");
        }
    }
}
