using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashScreen : MonoBehaviour
{
	public GameObject roomClearFlash;
    private GameObject temp;
    private bool active;
    private bool inactive;
    private bool complete;

    // Flash the room once
    void Start()
    {
        FlashRoom();
    }

    // Run through the different booleans to flash the screen and eventually destroy the objects
    void Update()
    {
        if (active && !complete)
        {
        	 temp.GetComponent<Image>().CrossFadeAlpha(1.0f, 0.5f, false);
        }

        if (complete)
        {
        	 temp.GetComponent<Image>().CrossFadeAlpha(0.0f, 0.5f, false);
        	 Destroy(temp, 1.0f);
        	 Destroy(gameObject, 1.0f);
        }
    }

    // instantiate the flash screen in
    // get the rect information and set the parent to the element in the UI
    // set the image alpha to zero
    private void FlashRoom()
    {
        temp = Instantiate(roomClearFlash);
        RectTransform tempRect = temp.GetComponent<RectTransform>();
        temp.transform.SetParent(GameObject.FindWithTag("whiteScreen").transform, false);
        tempRect.transform.localPosition = roomClearFlash.transform.localPosition;
        tempRect.transform.localScale = roomClearFlash.transform.localScale;
        tempRect.transform.localRotation = roomClearFlash.transform.localRotation;
        temp.GetComponent<Image>().canvasRenderer.SetAlpha(0.0f);
       
        active = true;

        Invoke("FlashComplete", 1.0f);
    }

    // set the booleans to reflect that the flash is complete and it's time to delete the object from the scene
    private void FlashComplete()
    {
    	complete = true;
    	active = false;
    }
}
