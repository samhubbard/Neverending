using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadNewSceneHelper : MonoBehaviour {

    public Transform player;
    PlayerController playerController;
    GameController gameController;
    Text scoreText;
    GameObject endGame;
    private Inventory inventory;
    public Text classArea;

	// Use this for initialization
	void Start () {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        NewSceneTransfer();

    }

    void NewSceneTransfer() {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();

        playerController.movementStick = GameObject.FindWithTag("MovementStick").GetComponent<Joystick>();
        playerController.firingStick = GameObject.FindWithTag("ShootingStick").GetComponent<Joystick>();

        // health bar
        gameController.healthBar = GameObject.FindWithTag("HealthBar").GetComponent<Image>();
        gameController.healingBar = GameObject.FindWithTag("HealingBar").GetComponent<Image>();
        gameController.healthText = GameObject.FindWithTag("HealthText").GetComponent<Text>();
        gameController.SetUpHealthBar();
        gameController.UpdateHealthTextString();

        // setup the player and display what class the player is
        PlayerStats character = gameController.playerStats;
        playerController.SetUpPlayer(character);
        classArea.text = character.Class;
        playerController.firingSpeed = gameController.playerStats.TotalRateOfFire;
        playerController.moveSpeed = gameController.playerStats.TotalRunSpeed;

        // get the current score and display it
        scoreText = GameObject.FindWithTag("ScoreArea").GetComponent<Text>();
        scoreText.text = "Score: " + gameController.score;

        // inventory swap over
        if (gameController.playerStats.InventoryOneFilled)
        {
            // set it to the proper inventory slot
            inventory.isFull[0] = true;
            Instantiate(gameController.inventorySlotOne, inventory.slots[0].transform, false);
        }

        if (gameController.playerStats.InventoryTwoFilled)
        {
            // set it to the proper inventory slot
            inventory.isFull[1] = true;
            Instantiate(gameController.inventorySlotTwo, inventory.slots[1].transform, false);
        }
    }
}

// comments are the devil