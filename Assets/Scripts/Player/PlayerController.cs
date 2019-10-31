using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    // needed variables
    public Transform player;
    public Joystick movementStick;
    public Joystick firingStick;
    GameController gameController;
    public float firingSpeed = 0.5f;
    private float lastShot = 0.0f;
    public bool mainMenu;
    private float firingStickDeadZone = .45f;
    private string className;
    public static bool invincible = false;
    public AudioClip footsteps;
    private AudioSource soundEffect;
    private bool moving = false;
    private bool playing = false;
    public AudioClip fireSoundEffect;
    public static bool disabled = false;
    public Text floatingCombatTextPrefab;

    float dirX, dirY, rotateAngle, fireX, fireY;
    Animator animator;
    
    public float moveSpeed = 0.1f;

    [SerializeField] Transform gun;
    [SerializeField] Rigidbody2D poisonBullet;
    [SerializeField] Rigidbody2D fireball;
    [SerializeField] Rigidbody2D knightBullet;
    
    public float bulletSpeed = 10.0f;

    public int bulletDamage;

    // Get references, setup the footstep sound effect
    void Start () {
        if (!mainMenu)
        {
            gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
            firingStick = GameObject.FindWithTag("ShootingStick").GetComponent<Joystick>();
        }
        movementStick = GameObject.FindWithTag("MovementStick").GetComponent<Joystick>();
        soundEffect = GetComponent<AudioSource>();
        soundEffect.clip = footsteps;
        soundEffect.volume = 0.3f * GameController.sfxVolume;
        soundEffect.loop = true;


        // setting the rotate angle and setting up the animator
        rotateAngle = 0f;
        animator = GetComponent<Animator>();
        animator.speed = 1;
    }

    // check the end game and work through playing the footsteps
    private void Update()
    {
        if (gameController != null)
        {
            if (gameController.currentHealth <= 0 && gameController.gameEnded == false)
            {
                gameController.EndGame(false);
            }
        }

        if (moving && !playing)
        {
            soundEffect.volume = 0.3f * GameController.sfxVolume;
            soundEffect.Play();
            playing = true;
        }
        else if (!moving)
        {
            if (playing)
            {
                soundEffect.Stop();
                playing = false;
            }
        }
    }

    // Move, fire and animate
    void FixedUpdate () {
        // checking to see if the player should move/rotate
        if (!disabled)
        {
            Move();
            if (!mainMenu)
            {
                Fire();
            }
            AnimatePlayer();
        }
        else
        {
            animator.speed = 0;
            moving = false;
        }
	}

    // get the direction that the movement stick is pointed, normalize the movement, and move the player
    void Move()
    {
        // getting the player's inputs for movement
        dirX = Mathf.RoundToInt(movementStick.Horizontal);
        dirY = Mathf.RoundToInt(movementStick.Vertical);

        // setting up the direction and speed to move the player
        Vector2 input = new Vector2(dirX, dirY);
        Vector2 direction = input.normalized;
        Vector2 velocity = direction * moveSpeed;
        Vector2 moveAmount = velocity * Time.deltaTime;

        // move the player sprite
        transform.Translate(moveAmount);

    }

    // get the direction that the movement stick is pointing and animate based on the direction that the player is moving
    void AnimatePlayer()
    {
        // checking the player's inputs for animation
        dirX = Mathf.RoundToInt(movementStick.Horizontal);
        dirY = Mathf.RoundToInt(movementStick.Vertical);

        // based on the values from the virtual stick, set the animator speed and set the direction
        if (dirX == 0 && dirY == 1)
        {
            moving = true;
            animator.speed = 1;
            animator.SetInteger("Direction", 1);
        }

        if (dirX == 1 && dirY == 1)
        {
            moving = true;
            animator.speed = 1;
            animator.SetInteger("Direction", 1);
        }

        if (dirX == 1 && dirY == 0)
        {
            moving = true;
            animator.speed = 1;
            animator.SetInteger("Direction", 4);
        }

        if (dirX == 1 && dirY == -1)
        {
            moving = true;
            animator.speed = 1;
            animator.SetInteger("Direction", 2);
        }

        if (dirX == 0 && dirY == -1)
        {
            moving = true;
            animator.speed = 1;
            animator.SetInteger("Direction", 2);
        }

        if (dirX == -1 && dirY == -1)
        {
            moving = true;
            animator.speed = 1;
            animator.SetInteger("Direction", 2);
        }

        if (dirX == -1 && dirY == 0)
        {
            moving = true;
            animator.speed = 1;
            animator.SetInteger("Direction", 3);
        }

        if (dirX == -1 && dirY == 1)
        {
            moving = true;
            animator.speed = 1;
            animator.SetInteger("Direction", 1);
        }

        if (dirX == 0 && dirY == 0)
        {
            moving = false;
            animator.speed = 0;
        }
    }

    // check the direction that the firing stick is pointing and rotate the "gun" in that direction
    // then check the timer and fire the projectile
    void Fire() {
        fireX = Mathf.RoundToInt(firingStick.Horizontal);
        fireY = Mathf.RoundToInt(firingStick.Vertical);

        if (fireX == 0 && fireY == 1)
        {
            rotateAngle = 0;
        }

        if (fireX == 1 && fireY == 1)
        {
            rotateAngle = -45f;
        }

        if (fireX == 1 && fireY == 0)
        {
            rotateAngle = -90f;
        }

        if (fireX == 1 && fireY == -1)
        {
            rotateAngle = -135f;
        }

        if (fireX == 0 && fireY == -1)
        {
            rotateAngle = -180f;
        }

        if (fireX == -1 && fireY == -1)
        {
            rotateAngle = -225f;
        }

        if (fireX == -1 && fireY == 0)
        {
            rotateAngle = -270f;
        }

        if (fireX == -1 && fireY == 1)
        {
            rotateAngle = -315f;
        }

        if (fireX == 0 && fireY == 0)
        {
            //animator.speed = 0;
        }
        gun.rotation = Quaternion.Euler(0f, 0f, rotateAngle);

        Vector3 moveVector = (Vector3.right * firingStick.Horizontal + Vector3.up * firingStick.Vertical);

        if (moveVector != Vector3.zero && (Time.time > firingSpeed + lastShot))
        {
            // this conditional statement adds in a dead zone to the stick
            if (Mathf.Abs(firingStick.Direction.x) > firingStickDeadZone 
                || Mathf.Abs(firingStick.Direction.y) > firingStickDeadZone)
            {
                FireBullet();
            }
        }
    }

    // play a sound effect, instantiate the correct type of bullet, and add a force to that
    void FireBullet()
    {
        AudioSource audioSource = gameObject.transform.Find("Gun").gameObject.AddComponent<AudioSource>();
        audioSource.clip = fireSoundEffect;
        audioSource.volume = 0.3f * GameController.sfxVolume;

        Rigidbody2D firedBullet;
        if (className == "Rogue")
        {
            firedBullet = Instantiate(poisonBullet, gun.position, gun.rotation);
        }
        else if (className == "Mage")
        {
            firedBullet = Instantiate(fireball, gun.position, gun.rotation);
        }
        else
        {
            firedBullet = Instantiate(knightBullet, gun.position, gun.rotation);
        }
        
        firedBullet.AddForce(gun.up * bulletSpeed);
        audioSource.Play();
        lastShot = Time.time;
        RoomEvent.bulletsFired += 1;
    }

    // reduce the player health based on the incoming damage amount
    // display the floating combat text
    public void TakeDamage(int damageAmount) {
        if (gameController.currentHealth > 0)
        {
            if (!invincible)
            {
                gameController.currentHealth -= damageAmount;
                gameController.UpdateHealthBarPostDamage();
                RoomEvent.healthLost += damageAmount;
                InitCombatText(damageAmount.ToString(), "damage");
            }
            
        }
    }

    // increase the player health based on the incoming healing amount
    // display the floating combat text
    public void TakeHealing(int healingAmount)
    {
        if (gameController.currentHealth < GameController.maxHealth &&
            gameController.currentHealth > 0)
        {
            gameController.healthBeforeHealing = gameController.currentHealth;
            gameController.currentHealth += healingAmount;
            gameController.UpdateHealingBar();
            InitCombatText(healingAmount.ToString(), "healing");
        }
    }

    // setup the player based on the the player stats pulled in by the game controller
    public void SetUpPlayer(PlayerStats playerStats)
    {
        className = playerStats.Class;
        moveSpeed = playerStats.TotalRunSpeed;
        bulletSpeed = playerStats.ProjectileFlightSpeed;
        firingSpeed = playerStats.TotalRateOfFire;
        bulletDamage = Mathf.RoundToInt(playerStats.AttackDamageIncreaseAmount);
    }

    // return the bullet damage to outside scripts
    public int GetBulletDamage()
    {
        return bulletDamage;
    }

    // setup and instantiate the floating combat text
    private void InitCombatText(string damageAmount, string type)
    {
        Text temp = Instantiate(floatingCombatTextPrefab);
        if (type == "damage")
        {
            temp.color = Color.red;
        }
        else if (type == "healing")
        {
            temp.color = Color.green;
        }
        RectTransform tempRect = temp.GetComponent<RectTransform>();
        temp.transform.SetParent(transform.Find("PlayerCanvas"));
        tempRect.transform.localPosition = floatingCombatTextPrefab.transform.localPosition;
        tempRect.transform.localScale = floatingCombatTextPrefab.transform.localScale;
        tempRect.transform.localRotation = floatingCombatTextPrefab.transform.localRotation;
        temp.text = damageAmount;
        temp.GetComponent<Animator>().SetTrigger("Hit");

        Destroy(temp, 2.0f);
    }
}
