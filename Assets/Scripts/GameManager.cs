using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] UI;
    public bool GameOn = false;
    public bool isPc;
    public GameObject SettingsTab;
    int wincondition = 5; // Number of keys to win
    public TextMeshProUGUI keyText; // Reference to the UI text element to display keys collected

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

    public void KeyCollected()
    {
        wincondition--;
        keyText.text=(5-wincondition).ToString();
        if (wincondition == 0) Debug.Log("Won");
    }

    public void playerDead()
    {
        if (UI.Length > 0) UI[0].SetActive(true);
        if (UI.Length > 4) UI[3].SetActive(false);
        if (UI.Length > 5) UI[4].SetActive(false);
        if (UI.Length > 6) UI[5].SetActive(false);

        AudioManager.Instance.StopAllSounds();
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Health.burnSound = 0; // Reset burn sound counter when a new scene is loaded
        AudioManager.SoundCD = false; // Reset sound cooldown flag
    }

    public void Settings()
    {
        SettingsTab.SetActive(true);
    }

    public void SettingsOff()
    {
        SettingsTab.SetActive(false);
    }

    public void VolumeSetting(float volume)
    {
        AudioManager.Instance.SetVolume(volume);
    }
}
