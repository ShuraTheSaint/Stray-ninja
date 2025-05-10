using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool mainGame = true;
    public GameObject[] UI;
    public WeaponSwitcher WS;
    public bool GameOn = true;
    public bool isPc = true;

    public void Start()
    {
        Application.targetFrameRate = 120;
        if (mainGame)
        {
            UI[0].SetActive(false);
            UI[1].SetActive(false);
            UI[2].SetActive(false);
            UI[3].SetActive(true);
            if (isPc)
            {
                UI[5].SetActive(false);
                UI[6].SetActive(false);
            }
        }
    }

    public void playerDead()
    {
        if(mainGame)
        {
            UI[0].SetActive(true);
            UI[1].SetActive(true);
            UI[4].SetActive(false);
            UI[5].SetActive(false);
            UI[6].SetActive(false);
            GameOn = false;
            Time.timeScale = 0;
        }
    }
    public void play()
    {
        SceneManager.LoadScene(1);
    }

    public void quit()
    {
        Application.Quit();
    }


}
