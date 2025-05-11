using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public GameObject Selection;
    public GameManager gm;
    public Upgrades upgrades;
    public int currentWeapon = 0;
    public void SwitchWeapon(int playerIndex)
    {
        if (playerIndex == 0)
        {
            upgrades.currentVariant = Upgrades.CharacterVariant.Sword;
        }
        if (playerIndex == 1)
        {
            upgrades.currentVariant = Upgrades.CharacterVariant.Shuriken;
        }
        if (playerIndex == 2)
        {
            upgrades.currentVariant = Upgrades.CharacterVariant.Ninjitsu;
        }
        playerPrefabs[playerIndex].SetActive(true);
        Selection.SetActive(false);
        gm.UI[2].SetActive(true);
        gm.GameOn = true;
        Time.timeScale = 1;

    }
}

