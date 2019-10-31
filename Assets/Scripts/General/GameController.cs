using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public bool gameEnded = false;
    public static float maxHealth = 250;
    public float currentHealth;
    public float calcHealth;
    private float calcHealing;
    public float healthBeforeHealing;
    public Image healthBar;
    public Image healingBar;
    public GameObject endGamePanel;
    public Text classArea;
    public PlayerController player;
    public Text healthText;
    public string healthTextRaw;
    public static Transform currentRoom;
    public static Node currentLootDropPoint;
    public PlayerStats playerStats;
    public static float difficultyModifier;
    public int score;
    public int currentLevel;
    public bool inventorySlotOneFilled;
    public bool inventorySlotTwoFilled;
    public GameObject inventorySlotOne;
    public GameObject inventorySlotTwo;
    public ClassObject chosenClass;

    // loot based variables (for storage)
    public float playerRateOfFire = 0.5f;
    public float maxHealthIncreased = 0; 
    public float attackDamageIncreased = 0; 
    public float projectileSpeedIncrease = 0; 
    public float runSpeedIncreaseAmount = 0; 
    public Text dungeonLevel;
    public static float sfxVolume;

    private DBAccess database;
    private GameObject endGame;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        // get chosen class information
        string chosenClassName = PlayerPrefs.GetString("ChosenClass");
        database = GetComponent<DBAccess>();
        chosenClass = database.GetClassInfo(chosenClassName);

        maxHealth = chosenClass.GetPlayerHealth;
        
        // this is where I will pull the information for the player stats
        playerStats = GetComponent<PlayerStatHandler>().GetPlayerStats();
        maxHealth = playerStats.TotalHealth;
        player.SetUpPlayer(playerStats);

        // link in the health bar and set it up
        healthBar = GameObject.FindWithTag("HealthBar").GetComponent<Image>();
        healingBar = GameObject.FindWithTag("HealingBar").GetComponent<Image>();
        healthText = GameObject.FindWithTag("HealthText").GetComponent<Text>();
        currentHealth = playerStats.CurrentHealth;
        healthBar.fillAmount = currentHealth / maxHealth;
        healingBar.fillAmount = currentHealth / maxHealth;
        UpdateHealthTextString();

        // set the current level to 1 (since it will always be that when the game controller starts)
        currentLevel = playerStats.CurrentLevel;
        
        // check to see if the calculated difficulty modifier is below the minimum that it can be
        // and if it is, set it to the minimum
        if (playerStats.CurrentDifficultyModifier < playerStats.MinimumDifficultyModifier)
        {
            difficultyModifier = playerStats.MinimumDifficultyModifier;
            playerStats.CurrentDifficultyModifier = playerStats.MinimumDifficultyModifier;
        }
        else if (playerStats.CurrentDifficultyModifier > playerStats.MaximumDifficultyModifier)
        {
            difficultyModifier = playerStats.MaximumDifficultyModifier;
            playerStats.CurrentDifficultyModifier = playerStats.MaximumDifficultyModifier;
        }
        else
        {
            difficultyModifier = playerStats.CurrentDifficultyModifier;
        }

        // setting the initial rate of fire for the player
        playerRateOfFire = 0.5f;

        // check to see if the player already has inventory items, if so... update the inventory
        if (playerStats.InventoryOneFilled)
        {
            UpdateInventory(0);
        }

        if (playerStats.InventoryTwoFilled)
        {
            UpdateInventory(1);
        }

        // display the chosen class
        classArea.text = playerStats.Class;

        if (playerStats.CurrentLevel < 4)
        {
            dungeonLevel.text = "Floor " + playerStats.CurrentLevel;
        }

        UpdateScore(0);

        if (!PlayerPrefs.HasKey("sfxVolume"))
        {
            sfxVolume = 1.0f;
            PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        }
        else
        {
            sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
        }
    }
    // Update is called once per frame
    void Update()
    {
        // check to see if the current health has gone above the max health
        // if it has, set it to max and that's it
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }

        // quick check to see if the modifier is correct
        if (difficultyModifier != playerStats.CurrentDifficultyModifier)
        {
            UpdateDifficultyModifier();
        }

        // rounding out the current health to an int
        playerStats.CurrentHealth = Mathf.RoundToInt(currentHealth);

    }

    public void EndGame(bool win)
    {

        if (!gameEnded)
        {
            gameEnded = true;
            if (win)
            {
                // this was going to have something in it, however, it is now handled elsewhere
            }
            else
            {
                // show the end game panel giving the player a choice
                endGame = Instantiate(endGamePanel, transform.position, transform.rotation) as GameObject;
                endGame.transform.SetParent(GameObject.FindWithTag("EndGamePanel").transform, false);
                
                // pause the game
                Time.timeScale = 0;
            }
        }
    }

    public void UpdatePlayerStats()
    {
        // update the player stats....
        PlayerStatHandler handler = GetComponent<PlayerStatHandler>();
        handler.SavePlayerStats(playerStats);
    }

    private void UpdateDifficultyModifier()
    {
        // update the difficulty modifier
        if (difficultyModifier < playerStats.MinimumDifficultyModifier)
        {
            difficultyModifier = playerStats.MinimumDifficultyModifier;
        }
        else if (difficultyModifier > playerStats.MaximumDifficultyModifier)
        {
            difficultyModifier = playerStats.MaximumDifficultyModifier;
        }
        playerStats.CurrentDifficultyModifier = difficultyModifier;
    }

    public void SetUpHealthBar()
    {
        // link in the health bar components
        healthBar = GameObject.FindWithTag("HealthBar").GetComponent<Image>();
        healingBar = GameObject.FindWithTag("HealingBar").GetComponent<Image>();

        // calculation and then set the health
        calcHealth = currentHealth / maxHealth;
        healthBar.fillAmount = calcHealth;
        healingBar.fillAmount = calcHealth;
        
    }

    public void UpdateHealthBar()
    {
        // update the health bar (red)
        calcHealth = currentHealth / maxHealth;
        healthBar.fillAmount = calcHealth;
        healingBar.fillAmount = calcHealth;
    }

    public void UpdateHealthBarPostDamage()
    {
        // update the health (red) and then the effect health (pink) through UpdateHealingBarPostDamage()
        calcHealth = currentHealth / maxHealth;
        healthBar.fillAmount = calcHealth;
        calcHealing = calcHealth;

        Invoke("UpdateHealingBarPostDamage", 0.3f);
    }

    public void UpdateHealingBarPostDamage()
    {
        healingBar.fillAmount = calcHealing;
        UpdateHealthTextString();
    }

    public void UpdateHealingBar()
    {
        // player takes a healing item, update the effect health (pink)
        // by the way, the "effect health" isn't actually the health, it's just the effect on the health bar
        // to enhance the feedback to the player
        calcHealth = currentHealth / maxHealth;
        healingBar.fillAmount = calcHealth;
        calcHealing = calcHealth;

        Invoke("UpdateHealthBarPostHealing", 1.0f);
    }

    public void UpdateHealthBarPostHealing()
    {
        // update the actual health bar (red) after a heal
        healthBar.fillAmount = calcHealing;
        UpdateHealthTextString();
    }

    public void UpdateScore(int _score)
    {
        // update the score to the player stats and display it on the UI
        playerStats.TotalScore += _score;
        Text scoreText = GameObject.FindWithTag("ScoreArea").GetComponent<Text>();
        scoreText.text = "Score: " + playerStats.TotalScore;
        
    }

    public void UpdateInventory(int i)
    {
        // link in to the player's inventory
        Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        inventory.isFull[i] = true; // set the index of the inventory that has an item

        if (i == 0)
        {
            // Inventory Item one
            inventorySlotOne = GetComponent<InventoryLinkingForSave>().GetInventoryItem(playerStats.InventoryOneItem);
            Instantiate(inventorySlotOne, inventory.slots[0].transform, false);
        }
        else
        {
            // Inventory Item two
            inventorySlotTwo = GetComponent<InventoryLinkingForSave>().GetInventoryItem(playerStats.InventoryTwoItem);
            Instantiate(inventorySlotTwo, inventory.slots[1].transform, false);
        }
    }

    // the Dynamic Damage Algorithm
    public static void DDA(float _timeToClear, int _hitpointsLost, float _hitPercentage, float _hitpointsAtStart) 
    {
        float interimModifier;

        interimModifier = difficultyModifier;

        // Time to clear room
        if (_timeToClear < 15f) {
            interimModifier = interimModifier + (interimModifier * .1f);
        } else if (_timeToClear >= 25) {
            interimModifier = interimModifier - (interimModifier * .1f);
        }


        if (_hitpointsLost == 0) {
            // drastically increase the modifier
            interimModifier = interimModifier + (interimModifier * .2f);
        } else if (_hitpointsLost <= 20 && _hitpointsLost > 1) {
            // moderately increase the modifier
            interimModifier = interimModifier + (interimModifier * .15f);
        } else if (_hitpointsLost <= 40 && _hitpointsLost > 21) {
            // slightyly increase the modifier
            interimModifier = interimModifier + (interimModifier * .1f);
        } else if (_hitpointsLost <= 80 && _hitpointsLost > 61) {
            // slightly decrease the modifier
            interimModifier = interimModifier - (interimModifier * .1f);
        } else if (_hitpointsLost > 100) {
            // drastically decrease the modifier
            interimModifier = interimModifier - (interimModifier * .15f);
        }

        // hits / total shots taken
        if (_hitPercentage == 1.0f) {
            // increase modifier
            interimModifier = interimModifier + (interimModifier * .2f);
        } else if (_hitPercentage > .5f && _hitPercentage < 1.0f) {
            // increase modifier
            interimModifier = interimModifier + (interimModifier * .1f);
        } else if (_hitPercentage > 0f && _hitPercentage < .5f) {
            // decrease modifier
            interimModifier = interimModifier - (interimModifier * .001f);
        }
        difficultyModifier = interimModifier;
    }

    // a getter for the difficulty modifer for outside scripts
    public static float GetDDAModifier() {
        return difficultyModifier;
    }

    // revive the player if the player watched the video
    public void RevivePlayer()
    {
        Destroy(endGame);
        currentHealth = maxHealth;
        UpdateHealthBar();
        Time.timeScale = 1;
        gameEnded = false;
    }

    // if the player skipped the add, kill him... GIVE ME MONIES!
    public void PlayerSkippedAd()
    {
        playerStats.FlaggedForDeletion = true;
        GetComponent<PlayerStatHandler>().SavePlayerStats(playerStats);
        Destroy(GameObject.Find("GameController"));

        SceneManager.LoadScene(0);
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    public void SavePlayerProgress()
    {
        GetComponent<PlayerStatHandler>().SavePlayerStats(playerStats);
    }

    public void UpdateHealthTextString()
    {
        healthTextRaw = currentHealth + "/" + maxHealth;
        healthText.text = healthTextRaw;
    }
}