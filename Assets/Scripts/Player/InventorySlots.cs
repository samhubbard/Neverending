using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlots : MonoBehaviour {

    public int i;

    private Inventory inventory;
    private GameController gameController;

	// Getting references to the player inventory and the game controller
	void Start () {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}
	
	// Checks to see if the inventory slot is now empty, and if it is, destroys everything applicable
	void Update () {
		if (transform.childCount <= 0)
        {
            inventory.isFull[i] = false;

            if (i == 0)
            {
                gameController.inventorySlotOneFilled = false;
                gameController.inventorySlotOne = null;
                gameController.playerStats.InventoryOneFilled = false;
                gameController.playerStats.InventoryOneItem = "none";
            }

            if (i == 1)
            {
                gameController.inventorySlotTwoFilled = false;
                gameController.inventorySlotTwo = null;
                gameController.playerStats.InventoryTwoFilled = false;
                gameController.playerStats.InventoryTwoItem = "none";
            }
        }
    }

    // drops the item onto the ground and destroys the item in the inventory
    public void DropItem()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<SpawnItem>().SpawnDroppedItem();
            GameObject.Destroy(child.gameObject);
        }
    }
}
