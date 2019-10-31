using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyController : MonoBehaviour {

    // variable declaration
    public Transform parent;
    public Unit parentUnit;
    public Image healthBar;
    DBAccess dbAccess;
    public Enemy enemyInfo;
    public static int meleeEnemyCount;
    public int baseHealthValue;
    private float currentHealth;
    private float maxHealthFloat;
    private float healthBarValue;
    public int baseAttackValue;
    public float speedModifier;
    public GameObject meleeHit;
    public GameObject poisonDamageHit;
    public AudioClip meleeAttackSound;
    public int adjustedHealthValue;
    public int adjustedAttackValue;
    GameController gameController;
    float difficultyModifier;
    public bool damageRelflection = false;

    // I need the player controller so that the player can react to attacks
    PlayerController playerController;
    Transform player;
    private float timeBetweenAttacks;
    public float startTimeBetweenAttacks = 1.0f;

    // Use this for initialization
    void Start()
    {
        // link in all of the pertinent stuff
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        meleeEnemyCount += 1;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        dbAccess = GetComponent<DBAccess>();
        enemyInfo = dbAccess.GetEnemyInfo("skeleton");

        // get the current difficulty modifer
        difficultyModifier = GameController.GetDDAModifier();

        // get the base stats for the enemy
        baseHealthValue = enemyInfo.getEnemyHealth;
        baseAttackValue = (int)enemyInfo.getBaseAttack;
        speedModifier = (float)enemyInfo.getSpeedModifier;

        // MATHS IT UP! Get adjusted values.
        float adjustedHealthFloat = baseHealthValue + (baseHealthValue * difficultyModifier / baseHealthValue);
        float adjustedAttackFloat = baseAttackValue + (baseAttackValue * difficultyModifier / baseAttackValue);

        // set the health for the health bars
        currentHealth = adjustedHealthFloat;
        maxHealthFloat = adjustedHealthFloat;
        healthBarValue = currentHealth / maxHealthFloat;
        healthBar.fillAmount = healthBarValue;

        // round all of the values to ints. This is what will be used
        adjustedHealthValue = Mathf.RoundToInt(adjustedHealthFloat);
        adjustedAttackValue = Mathf.RoundToInt(adjustedAttackFloat);

    }

    // Update is called once per frame
    void Update()
    {
        // keep this sprite glued to the parent (this is because of the 3D Astar)
        transform.position = parent.position;

        // ensure that enemy is within attack distance
        if (timeBetweenAttacks <= 0 && Vector3.Distance(transform.position, player.position) <= parentUnit.attackRange) {
            // attack the player and reset the time between attack timer
            AttackPlayer();
            timeBetweenAttacks = startTimeBetweenAttacks;
            GetComponent<Animator>().speed = 0;
        } else {
            timeBetweenAttacks -= Time.deltaTime;
        }

        // if the enemy is outside of the attack range, start animating
        // because the AStar is already working on moving the enemy toward the player
        if (Vector3.Distance(transform.position, player.position) > parentUnit.attackRange)
        {
            GetComponent<Animator>().speed = 1;
        }
    }

    void AttackPlayer() {
        // play the attack sound
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = meleeAttackSound;
        audioSource.volume = .30f * GameController.sfxVolume;
        audioSource.Play();

        // Animate a melee hitS
        // I'm leaving the above comment in the code... what the hell was I thinking there?
        if (!damageRelflection)
        {
            // as long as the damage reflection buff isn't active, attack the player

            // set the animation and play it
            GameObject hitAnimation = Instantiate(meleeHit, player.position, Quaternion.identity, player);
            hitAnimation.transform.localScale = new Vector3(0.25f, 0.25f, 1);
            hitAnimation.GetComponent<Animator>().SetTrigger("MeleeHit");
            
            // this for loop is to ensure that the animation actually happens on the player
            // even if the player is moving
            for (int i = 0; i < 1000; i++)
            {
                hitAnimation.transform.position = player.position;
            }

            // Call the TakeDamage function
            playerController.TakeDamage(adjustedAttackValue);
            Destroy(hitAnimation, 1.0f);
        }
        else
        {
            // the player yells to the stupid skeleton, "Stop hitting yourself!"
            GameObject hitAnimation = Instantiate(meleeHit, transform.position, Quaternion.identity, transform);
            hitAnimation.transform.localScale = new Vector3(0.25f, 0.25f, 1);
            hitAnimation.GetComponent<Animator>().SetTrigger("MeleeHit");
            for (int i = 0; i < 1000; i++)
            {
                hitAnimation.transform.position = transform.position;
            }
            // Call the TakeDamage function
            TakeDamage(adjustedAttackValue);
            Destroy(hitAnimation, 1.0f);
        }

        
    }

    public void TakeDamage(int damageAmount) {
        // remove the health from the current health and
        // adjust the health bar
        adjustedHealthValue -= damageAmount;
        currentHealth -= damageAmount;
        UpdateHealthBar();

        // check to see if the enemy is dead
        if (adjustedHealthValue <= 0)
        {
            // remove the enemy from the count and destroy the player
            meleeEnemyCount -= 1;
            Destroy(transform.parent.gameObject);
            Destroy(gameObject);

            // update the player score
            gameController.UpdateScore(5);
        }
    }

    private void UpdateHealthBar()
    {
        // keep the health bar completely up to date
        healthBarValue = currentHealth / maxHealthFloat;
        healthBar.fillAmount = healthBarValue;
    }

    public void PlayPoisonDamageAnimation()
    {
        // instantiate the DOT object onto the enemy and run it
        GameObject poisonDamageAnimation = Instantiate(poisonDamageHit, transform.position, Quaternion.identity, transform);
        poisonDamageAnimation.transform.localScale = new Vector3(0.125f, 0.125f, 1f);
        poisonDamageAnimation.GetComponent<Animator>().SetTrigger("PoisonHit");
        for (int i = 0; i < 1000; i++)
        {
            poisonDamageAnimation.transform.position = transform.position;
        }

        Destroy(poisonDamageAnimation, 1.0f);
    }
}
