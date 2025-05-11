using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] UI;
    public bool GameOn = true;
    public bool isPc = true;

    public void Start()
    {
        Application.targetFrameRate = 120;

        // Defensive: Only set UI elements if they exist
        if (UI.Length > 0) UI[0].SetActive(false);
        if (UI.Length > 1) UI[1].SetActive(false);
        if (UI.Length > 2) UI[2].SetActive(false);
        if (UI.Length > 3) UI[3].SetActive(true);

        if (isPc)
        {
            if (UI.Length > 5) UI[5].SetActive(false);
            if (UI.Length > 6) UI[6].SetActive(false);
        }
    }

    public void playerDead()
    {
        if (UI.Length > 0) UI[0].SetActive(true);
        if (UI.Length > 1) UI[1].SetActive(true);
        if (UI.Length > 4) UI[4].SetActive(false);
        if (UI.Length > 5) UI[5].SetActive(false);
        if (UI.Length > 6) UI[6].SetActive(false);

        GameOn = false;
        Time.timeScale = 0;
    }

    public void play()
    {
        SceneManager.LoadScene("TheGame"); // Consider using scene name for clarity
    }

    public void quit()
    {
        Application.Quit();
    }
}
