using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Damage : MonoBehaviour
{
    int rank = 0;
    int killCount = 0;
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
        }
    }

    public void Monster()
    {
        killCount++;
        if (killCount >= 100)
        {
            if(rank < 5)
            {
                rank++;
                maxhp++;
                hp++;
                gameObject.transform.localScale += new Vector3(0.3f, 0.3f, 0.3f); // Increase player size by 50%
                killCount -= 100;
            }
        }
    }
}
