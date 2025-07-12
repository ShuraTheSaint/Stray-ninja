using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] UI;
    public bool GameOn = false;
    public bool isPc = true;

    public void Start()
    {
        Time.timeScale = 1;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            isPc = false;
        }else isPc = true;

        Application.targetFrameRate = 120;
        if (UI.Length > 0) UI[1].SetActive(false);
        if (UI.Length > 1) UI[2].SetActive(true);

        if (isPc)
        {
            if (UI.Length > 3) UI[4].SetActive(false);
            if (UI.Length > 4) UI[5].SetActive(false);
        }
    }

    public void playerDead()
    {
        if (UI.Length > 0) UI[0].SetActive(true);
        if (UI.Length > 4) UI[3].SetActive(false);
        if (UI.Length > 5) UI[4].SetActive(false);
        if (UI.Length > 6) UI[5].SetActive(false);

        GameOn = false;
        Time.timeScale = 0;
    }

    public void play()
    {
        SceneManager.LoadScene("TheGame");
    }

    public void quit()
    {
        Application.Quit();
    }
}
