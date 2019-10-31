using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangedEnemy : MonoBehaviour {
    // variables
    Transform player;
    private float timeBetweenAttacks;
    public float startTimeBetweenAttacks = 1.0f;
    public float bulletSpeed;
    public GameObject poisonDamageHit;
    public Image healthBar;
    DBAccess dbAccess;
    public Enemy enemyInfo;
    public static int rangedEnemyCount;
    public int baseHealthValue;
    public int baseAttackValue;
    public float speedModifier;
    public float teleportRange = 6.0f;
    public RoomEvent roomEvent;
    public GameObject teleportAnimation;
    private Node teleportNode;
    private bool teleporting = false;
    private GameObject parent;
    private AudioSource audioSource;
    private AudioSource teleportSoundSourceHolder;
    public AudioClip fireSoundEffect;
    public AudioClip teleportSound;
    private float currentHealth;
    private float maxHealthFloat;
    private float healthBarValue;
    public int adjustedHealthValue;
    public int adjustedAttackValue;
    float difficultyModifier;
    GameController gameController;
    private float timeBetweenTeleport;
    public float startTimeToTeleport = 8.0f;
    private bool hasTeleported;
    public bool damageReflection = false;
    [SerializeField] Rigidbody2D enemyBullet;

    // Use this for initialization
    void Start () {
        // initial setup
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        rangedEnemyCount += 1;
        dbAccess = GetComponent<DBAccess>();
        enemyInfo = dbAccess.GetEnemyInfo("goblinSorc");

        // get all of the base values
        baseHealthValue = enemyInfo.getEnemyHealth;
        baseAttackValue = (int)enemyInfo.getBaseAttack;
        speedModifier = (float)enemyInfo.getSpeedModifier;

        // get the difficulty modifer
        difficultyModifier = GameController.GetDDAModifier();

        // get the adjusted values using THE MATHS for health and attack
        float adjustedHealthFloat = baseHealthValue + (baseHealthValue * difficultyModifier / baseHealthValue);
        float adjustedAttackFloat = baseAttackValue + (baseAttackValue * difficultyModifier / baseAttackValue);

        // set the current and max health values
        currentHealth = adjustedHealthFloat;
        maxHealthFloat = adjustedHealthFloat;

        // update the health bar
        healthBarValue = currentHealth / maxHealthFloat;
        healthBar.fillAmount = healthBarValue;

        // adjust how the difficulty modifier is going to be calculated into the ROF
        float modifier = difficultyModifier * .05f;
        startTimeBetweenAttacks = 1 - modifier;

        // the cap for the ROF
        if (startTimeBetweenAttacks < .25f)
        {
            startTimeBetweenAttacks = .25f;
        }
        
        // turn the adjusted value floats into ints to use
        adjustedHealthValue = Mathf.RoundToInt(adjustedHealthFloat);
        adjustedAttackValue = Mathf.RoundToInt(adjustedAttackFloat);

        // this is needed to link to the current room event
        // What is below looks way batter than gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.Find......
        GameObject enemyParent = gameObject.transform.parent.gameObject;
        GameObject enemyCubeParent = enemyParent.transform.parent.gameObject;
        roomEvent = enemyCubeParent.transform.Find("RoomCloser").GetComponent<RoomEvent>();

        // get the teleport sound
        audioSource = gameObject.AddComponent<AudioSource>();
        teleportSoundSourceHolder = gameObject.transform.Find("teleportSoundHolder").GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        // check the time in the timer, then attack the player
        if (timeBetweenAttacks <= 0 && player != null)
        {
            AttackPlayer();
            timeBetweenAttacks = startTimeBetweenAttacks;
        }
        else
        {
            timeBetweenAttacks -= Time.deltaTime;
        }

        // Check the distance to the player, if it's too close, find a place to teleport to and go there.
        // Also ensure that a teleport isn't already occuring, if this isn't checked the entire screen will be populated with
        // teleportation animations
        if (Vector3.Distance(transform.position, player.position) <= teleportRange && !teleporting)
        {
            // get the node for the teleport location
            Node teleportLocation = roomEvent.TeleportLocation();

            // Ensure that the location isn't null, and teleport to the location
            if (teleportLocation != null)
            {
                // set the status to teleporting and the location to the more public variable
                teleporting = true;
                if (!hasTeleported)
                {
                    hasTeleported = true;
                }
                teleportNode = teleportLocation;

                // Play teleport animation at current position
                GameObject outTeleport = Instantiate(teleportAnimation, transform.position, Quaternion.identity);
                outTeleport.GetComponent<Animator>().SetTrigger("AnimationTrigger");

                // Play teleport animation at new position
                GameObject inTeleport = Instantiate(teleportAnimation, teleportLocation.worldPosition, Quaternion.identity);
                inTeleport.GetComponent<Animator>().SetTrigger("AnimationTrigger");

                // Move the enemy to the new position
                Teleport();

                // destroy the animation objects
                Destroy(outTeleport, 1.0f);
                Destroy(inTeleport, 1.0f);

                // reset the timer to teleport
                timeBetweenTeleport = Random.Range(5, 11);
            }
        }

        if (timeBetweenTeleport <= 0 && !teleporting && hasTeleported)
        {
            // check the comments above... it's virutally the same thing
            Node teleportLocation = roomEvent.TeleportLocation();

            if (teleportLocation != null)
            {
                teleporting = true;
                teleportNode = teleportLocation;

                GameObject outTeleport = Instantiate(teleportAnimation, transform.position, Quaternion.identity);
                outTeleport.GetComponent<Animator>().SetTrigger("AnimationTrigger");

                GameObject inTeleport = Instantiate(teleportAnimation, teleportLocation.worldPosition, Quaternion.identity);
                inTeleport.GetComponent<Animator>().SetTrigger("AnimationTrigger");

                Teleport();

                Destroy(outTeleport, 1.0f);
                Destroy(inTeleport, 1.0f);

                timeBetweenTeleport = Random.Range(5, 11);
            }
        }
        else if (startTimeToTeleport <= 0 && !teleporting && !hasTeleported)
        {
            // check the comments above, it's virtually the same thing
            hasTeleported = true;
            Node teleportLocation = roomEvent.TeleportLocation();

            if (teleportLocation != null)
            {
                teleporting = true;
                teleportNode = teleportLocation;

                GameObject outTeleport = Instantiate(teleportAnimation, transform.position, Quaternion.identity);
                outTeleport.GetComponent<Animator>().SetTrigger("AnimationTrigger");

                GameObject inTeleport = Instantiate(teleportAnimation, teleportLocation.worldPosition, Quaternion.identity);
                inTeleport.GetComponent<Animator>().SetTrigger("AnimationTrigger");

                Teleport();

                Destroy(outTeleport, 1.0f);
                Destroy(inTeleport, 1.0f);

                timeBetweenTeleport = Random.Range(5, 11);
            }
        }
        else
        {
            timeBetweenTeleport -= Time.deltaTime;
        }

    }
    
    void AttackPlayer() {
        if (!damageReflection)
        {
            // get and set the sound effect
            audioSource.clip = fireSoundEffect;
            audioSource.volume = 0.3f * GameController.sfxVolume;

            // shoot a bullet at the player
            Rigidbody2D firedBullet = Instantiate(enemyBullet, transform.position, Quaternion.identity);
            Vector2 bulletDirection = (player.transform.position - transform.position).normalized;
            firedBullet.GetComponent<EnemyBulletCollision>().SetDamage(adjustedAttackValue);
            firedBullet.AddForce(new Vector2(bulletDirection.x, bulletDirection.y) * bulletSpeed);

            audioSource.Play();
        }
        else
        {
            TakeDamage(adjustedAttackValue);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        // update the health values and update the health bar
        adjustedHealthValue -= damageAmount;
        currentHealth -= damageAmount;
        UpdateHealthBar();

        // check to see if the enemy is dead
        if (adjustedHealthValue <= 0)
        {
            // remove it from the enemy count and destroy the game object(s)
            rangedEnemyCount -= 1;
            Destroy(transform.parent.gameObject);
            Destroy(gameObject);

            // update the player score
            gameController.UpdateScore(5);
        }
    }

    private void UpdateHealthBar()
    {
        // calculate the float value to update the health bar and update it
        healthBarValue = currentHealth / maxHealthFloat;
        healthBar.fillAmount = healthBarValue;
    }

    private void Teleport()
    {
        // play the teleport sound
        teleportSoundSourceHolder.clip = teleportSound;
        teleportSoundSourceHolder.volume = 0.4f * GameController.sfxVolume;
        teleportSoundSourceHolder.Play();

        // move the enemy to the new position and turn off the teleporting status
        gameObject.transform.position = teleportNode.worldPosition;
        teleporting = false;
    }

    public void PlayPoisonDamageAnimation()
    {
        // instantiate the poison DOT and apply the animations
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
