using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlameKnightController : MonoBehaviour
{
    public Slider healthBar;
    public static bool circleAttackActive = false;
    public bool damageReflectionActive = false;
    public Animator animator;
    public PlayerController playerController;
    public float totalTimeBetweenDamageCircleAttack;
    public GameObject poisonDamageHit;
    public GameObject circleAttackPoints;
    public GameObject areaAttackPoint;
    public GameObject portalSpawnPoint;
    public GameObject lootPointOne;
    public GameObject lootPointTwo;

    private Boss bossStats;
    private int baseHealth;
    private int baseAttackDamageOne;
    private int baseAttackDamageTwo;
    private int adjustedAttackOne;
    private int adjustedAttackTwo;
    private int maxHealth;
    private int currentHealth;
    private bool healthBarIsActive = false;
    private bool playerInCircleAttack = false;
    private bool deadBoss = false;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        // bring in all of the base stats
        bossStats = GameObject.FindGameObjectWithTag("BossSpawnPoint").GetComponent<ChooseBoss>().GetBossStats();
        baseHealth = bossStats.GetBossHealth;
        baseAttackDamageOne = bossStats.GetAttackDamageOne;
        baseAttackDamageTwo = bossStats.GetAttackDamageTwo;

        // do some math based on the current modifier
        float modifier = GameController.GetDDAModifier();
        float adjustedHealthFloat = baseHealth + (baseHealth * modifier / baseHealth);
        float adjustedAttackOneFloat = 
            baseAttackDamageOne + (baseAttackDamageOne * modifier / baseAttackDamageOne);
        float adjustedAttackTwoFloat =
            baseAttackDamageTwo + (baseAttackDamageTwo * modifier / baseAttackDamageTwo);

        // set the health and attack values based on the difficulty modifier
        maxHealth = Mathf.RoundToInt(adjustedHealthFloat);
        adjustedAttackOne = Mathf.RoundToInt(adjustedAttackOneFloat);
        adjustedAttackTwo = Mathf.RoundToInt(adjustedAttackTwoFloat);

        // instantiate the different points that the boss will need to use to 
        // move around the room
        Instantiate(circleAttackPoints, new Vector3(0, 0, 0), Quaternion.identity);
        Instantiate(areaAttackPoint, new Vector3(0, 37, 0), Quaternion.identity);

        // setting up animators, the player, and setting a timer for the circle attack
        animator = GetComponent<Animator>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        totalTimeBetweenDamageCircleAttack = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        // this conditional is used purely for my testing purposes.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentHealth = 0;
        }

        // keep the health bar current with the boss's current health
        if (healthBarIsActive)
        {
            healthBar.value = currentHealth;
        }

        // trigger the death once the boss hits the death threshold (zero health)
        if (currentHealth <= 0 && healthBarIsActive && !deadBoss)
        {
            BossIsDead();
        }

        // checks to see if the player is in the circle attack damage area
        // and if the attack is currently active
        if (playerInCircleAttack && circleAttackActive)
        {
            if (timer <= 0)
            {
                // sucks to be you, player! You're taking damage!
                if (!damageReflectionActive)
                {
                    playerController.TakeDamage(adjustedAttackOne);
                    timer = totalTimeBetweenDamageCircleAttack;
                }
                else
                {
                    TakeDamage(adjustedAttackOne);
                    timer = totalTimeBetweenDamageCircleAttack;
                }
            }
            else
            {
                timer -= Time.deltaTime;
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (circleAttackActive)
            {
                if (!damageReflectionActive)
                {
                    // player takes damage
                    playerController.TakeDamage(adjustedAttackOne);
                    timer = totalTimeBetweenDamageCircleAttack;
                    playerInCircleAttack = true;
                }
                else
                {
                    // player tells the boss, "stop hitting yourself"
                    TakeDamage(adjustedAttackOne);
                    timer = totalTimeBetweenDamageCircleAttack;
                    playerInCircleAttack = true;
                }
                
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // the boolean that will make the damage going to the player stop
            playerInCircleAttack = false;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        // pretty simple, incoming damage... remove it from the current health
        currentHealth -= damageAmount;
    }

    public void SetUpHealthBar()
    {
        // setting up the health bar... first link to it (and start playing the boss sound)
        transform.GetChild(0).GetComponent<FlameKnightGeneralSound>().StartPlayingSound();
        healthBar = GameObject.FindGameObjectWithTag("BossHealth").GetComponent<Slider>();

        // set up the values for the boss's health bar
        healthBar.minValue = 0;
        healthBar.maxValue = maxHealth;
        currentHealth = maxHealth;
        healthBar.value = currentHealth;

        // tell the script that the healthbar is active and that the script can start to update it
        healthBarIsActive = true;
    }

    public void PlayPoisonDamageAnimation()
    {
        // this instantiates an object to the boss that will apply a DOT
        GameObject poisonDamageAnimation = Instantiate(poisonDamageHit, transform.position, Quaternion.identity, transform);
        poisonDamageAnimation.transform.localScale = new Vector3(1f, 1f, 1f);
        poisonDamageAnimation.transform.position = new Vector3(0, 2f, 0);
        poisonDamageAnimation.GetComponent<Animator>().SetTrigger("PoisonHit");
        for (int i = 0; i < 1000; i++)
        {
            poisonDamageAnimation.transform.position = transform.position;
        }

        Destroy(poisonDamageAnimation, 1.0f);
    }

    // these next two are simply getter functions for outside scripts
    public int GetAreaDamage()
    {
        return adjustedAttackTwo;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    // this handles the boss death
    private void BossIsDead()
    {
        animator.SetTrigger("Death");

        // stop playing the boss sound
        transform.GetChild(0).GetComponent<FlameKnightGeneralSound>().StopPlayingSound();

        // Set the room event to false and update all of the pertinent player stats
        deadBoss = true;
        StartBossFight.bossRoomEventActive = false;
        PlayerPrefs.SetString("JustKilledBoss", "true");
        int currentStreak = PlayerPrefs.GetInt("CurrentBestStreak");
        currentStreak += 1;
        PlayerPrefs.SetInt("CurrentBestStreak", currentStreak);
        if (currentStreak == 1)
        {
            PlayerPrefs.SetInt("StreakOfOneCompleted", 1);
        }
        if (currentStreak == 5)
        {
            PlayerPrefs.SetInt("StreakOfFiveCompleted", 1);
        }
        if (currentStreak == 20)
        {
            PlayerPrefs.SetInt("StreakOfTwentyCompleted", 1);
        }
        if (currentStreak == 50)
        {
            PlayerPrefs.SetInt("StreakOfFiftyCompleted", 1);
        }

        // modifier alteration and score update
        GameController c = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        c.UpdateScore(200);
        c.playerStats.MinimumDifficultyModifier += 1;
        c.playerStats.MaximumDifficultyModifier += 2;
        c.playerStats.CurrentDifficultyModifier += 1;
        c.playerStats.SuccessfulRuns += 1;
        c.playerStats.CurrentLevel = 1;
        c.playerStats.RoomsClearedCounter = 0;
        c.playerStats.CurrentHealth = c.playerStats.TotalHealth;
        c.SavePlayerProgress();

        // portal spawn
        Invoke("StartDeathAnimations", 0.2f);
    }

    private void StartDeathAnimations()
    {
        CameraController.transitioning = true;

        PlayerController.disabled = true;

        CameraController camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        camera.SetToBoss();

        

        Invoke("MoveToPortal", 3.0f);
    }

    private void MoveToPortal()
    {
        CameraController camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        camera.SetToPortalSpawnPoint();

        Invoke("SpawnPortal", 1.0f);
    }

    private void SpawnPortal()
    {
        // spawn the portal in
        portalSpawnPoint = GameObject.Find("Portal Spawn Point");
        portalSpawnPoint.GetComponent<SpawnPortalBackToMenu>().Spawn();

        Invoke("BackToPlayer", 1.0f);
    }

    private void BackToPlayer()
    {
        CameraController camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        camera.SetBackToPlayer();

        Invoke("GiveControlBack", 0.2f);
    }

    private void GiveControlBack()
    {
        CameraController.transitioning = false;
        PlayerController.disabled = false;
    }
}
