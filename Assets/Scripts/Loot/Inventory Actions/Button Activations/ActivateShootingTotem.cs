using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateShootingTotem : MonoBehaviour
{
    // timer variables
    public float totalTotemActiveTime = 5.5f;
    private float timer;

    public float timeBetweenShots = 0.5f;
    private float bulletTimer;

    private GameObject targetEnemy;
    private Transform firingPoint;
    private AudioSource audioSource;

    [SerializeField]
    Rigidbody2D bullet;

    // Grab references and setup the timers
    // then get a target for the totem to focus in on
    void Start()
    {
        audioSource = gameObject.transform.parent.GetComponent<AudioSource>();
        firingPoint = gameObject.transform.Find("Spawn Point").transform;
        timer = totalTotemActiveTime;
        bulletTimer = timeBetweenShots;
        GetTarget();
    }

    // Check the timers
    // if the time is up, display a message to the player and destroy the object
    // if the ROF timer hits, shoot a bullet at the enemy
    void Update()
    {
        if (timer <= 0)
        {
            // time's up

            string messageToSend = "Attack Totem Expired.";
            DisplayMessage.MessageToQueue(messageToSend);

            Destroy(gameObject.transform.parent.gameObject, 0.5f);
            Destroy(gameObject, 0.5f);
        }
        else
        {
            timer -= Time.deltaTime;
        }

        if (bulletTimer <= 0)
        {
            // shoot
            Shoot();
            bulletTimer = timeBetweenShots;
        }
        else
        {
            bulletTimer -= Time.deltaTime;
        }
    }

    // get the first enemy in the array and shoot at it
    private void GetTarget()
    {
        
        if (RoomEvent.roomEventActive)
        {
            GameObject[] meleeEnemies = GameObject.FindGameObjectsWithTag("meleeEnemy");
            targetEnemy = meleeEnemies[0];
        }
        else
        {
            targetEnemy = GameObject.FindGameObjectWithTag("Boss");
        }
        
    }

    // play the fire sound, instantiate a bullet, set the direction and send it toward the enemy
    private void Shoot()
    {
        if (targetEnemy != null)
        {
            audioSource.volume = audioSource.volume * GameController.sfxVolume;
            audioSource.Play();
            Rigidbody2D firedBullet = Instantiate(bullet, firingPoint.position, Quaternion.identity);
            Vector2 bulletDirection = (targetEnemy.transform.position - firingPoint.position).normalized;
            firedBullet.GetComponent<KnightAttackCollision>().SetTotemDamage(5);
            firedBullet.AddForce(new Vector2(bulletDirection.x, bulletDirection.y) * 600);
        }
    }
}
