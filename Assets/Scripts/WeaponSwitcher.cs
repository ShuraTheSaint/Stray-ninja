using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public GameObject Selection;
    public GameManager gm;
    public int currentWeapon = 0;
    public void SwitchWeapon(int playerIndex)
    {
        playerPrefabs[playerIndex].SetActive(true);
        Selection.SetActive(false);
        gm.UI[2].SetActive(true);
        gm.GameOn = true;
        Time.timeScale = 1;
    }
}

