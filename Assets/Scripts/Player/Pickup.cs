using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    private Inventory inventory;
    public GameObject itemButton;
    private GameController gameController;
    public string inventoryItemName;
    private AudioSource audioSource;
    private float timer;
    private bool canDestroy;
    private bool hasDestroyed;
    private bool hasPickedup;

	// get references to the player inventory and the game controller
	void Start () {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        audioSource = gameObject.transform.parent.GetComponent<AudioSource>();
        canDestroy = false;
        hasDestroyed = false;
        hasPickedup = false;
        timer = 0.5f;
	}

    void Update()
    {
        // checks in on the timers and eventually destroys the object off of the ground
        if (timer <= 0 && hasPickedup)
        {
            canDestroy = true;
        }
        else
        {
            timer -= Time.deltaTime;
        }

        if (canDestroy && !hasDestroyed)
        {
            hasDestroyed = true;
            Destroy(gameObject.transform.parent.gameObject, 2.0f);
            Destroy(gameObject);
        }
    }

    // this runs through the player inventory and checks to see if the player has an open slot, if the do
    // that inventory slot is filled
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasPickedup)
        {
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if (!inventory.isFull[i])
                {
                    inventory.isFull[i] = true;
                    if (i == 0)
                    {
                        gameController.inventorySlotOneFilled = true;
                        gameController.inventorySlotOne = itemButton;
                        gameController.playerStats.InventoryOneFilled = true;
                        gameController.playerStats.InventoryOneItem = GetComponent<Identifier>().identity;
                    }
                    else
                    {
                        gameController.inventorySlotTwoFilled = true;
                        gameController.inventorySlotTwo = itemButton;
                        gameController.playerStats.InventoryTwoFilled = true;
                        gameController.playerStats.InventoryTwoItem = GetComponent<Identifier>().identity;
                    }
                    Instantiate(itemButton, inventory.slots[i].transform, false);
                    audioSource.volume = 1f * GameController.sfxVolume;
                    audioSource.Play();
                    gameController.UpdatePlayerStats();
                    hasPickedup = true;

                    

                    // Flash message
                    string messageToSend = GetComponent<Identifier>().flashMessage;
                    DisplayMessage.MessageToQueue(messageToSend);

                    break;
                }
            }

            if (inventory.isFull[0] && inventory.isFull[1] && !hasPickedup)
            {
                string messageToSend = GetComponent<Identifier>().flashMessageWhenInventoryFull;
                DisplayMessage.MessageToQueue(messageToSend);
            }
        }
    }
}
