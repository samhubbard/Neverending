using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortalMenu : MonoBehaviour {

    public GameObject portalMenu;
    public CharacterSheet sheetInfo;
    public PlayerStatHandler getStats;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats character = getStats.GetPlayerStats();
            sheetInfo.SetCharacterSheet(character);
            portalMenu.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            portalMenu.SetActive(false);
        }
    }
}
