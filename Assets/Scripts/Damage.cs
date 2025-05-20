using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Damage : MonoBehaviour
{
    int count = 0;
    public int hp;
    int maxhp = 5;
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
        if(HealNumb==1&&hp<maxhp)
        {
            hp++;
            Debug.Log("healed");
        }
    }

    public void Monster()
    {
        count++;
        if (count>=100)
        {
            maxhp++;
            hp++;
            gameObject.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f); // Increase player size by 50%
            count -= 100;
            Debug.Log("Monster!");
        }
    }
}
