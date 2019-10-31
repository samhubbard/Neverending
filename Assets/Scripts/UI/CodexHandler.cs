using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodexHandler : MonoBehaviour
{
    public GameObject codexWindow;

    // shows the codex menu
    private void OnTriggerEnter2D(Collider2D other)
    {
    	if (other.CompareTag("Player"))
    	{
    		codexWindow.SetActive(true);
    	}
    }

    // hides the codex menu
    private void OnTriggerExit2D(Collider2D other)
    {
    	if (other.CompareTag("Player"))
    	{
    		codexWindow.SetActive(false);
    	}
    }
}
