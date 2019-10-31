using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassSelection : MonoBehaviour
{
    public GameObject classSelectionMenu;

    private static bool classSelected; // this will be used in the future to ensure that a player is going in after selecting a class

    // show the class selection menu
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !classSelected)
        {
            classSelectionMenu.SetActive(true);
        }
    }

    // hides the class selection menu
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            classSelectionMenu.SetActive(false);
        }
    }
}
