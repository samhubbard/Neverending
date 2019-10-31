using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameKnightAreaAttackObject : MonoBehaviour
{
    public float totalTimeBetweenShots = 0.5f;
    public GameObject bullet;
    public FlameKnightController boss;

    private bool firstActive;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        // set the initial values
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<FlameKnightController>();
        timer = totalTimeBetweenShots;
    }

    // Update is called once per frame
    void Update()
    {
        // check to see if the ROF timer has expired
        if (timer <= 0)
        {
            // shoot a barrage of bullets, reset the timer, and alternate the boolean
            // to ensure that the bullet spread is shifting
            Shoot();
            timer = totalTimeBetweenShots;
            if (firstActive)
            {
                firstActive = false;
            }
            else
            {
                firstActive = true;
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    private void Shoot()
    {
        // play the sound effect
        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = audio.volume * GameController.sfxVolume;
        audio.Play();
        // check the boolean, set the initial rotation
        // and then run a for loop that instantiates a bullet in and sets the rotation
        // then adds a force to the bullet to get it moving
        if (firstActive)
        {
            int rotation = 30;
            for (int i = 0; i < 11; i++)
            {
                GameObject clone = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, rotation));
                clone.GetComponent<EnemyBulletCollision>().SetDamage(boss.GetAreaDamage());
                clone.GetComponent<Rigidbody2D>().AddForce(clone.transform.up * 800);
                rotation += 30;
            }
        }
        else
        {
            int rotation = 15;
            for (int i = 0; i < 12; i++)
            {
                GameObject clone = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, rotation));
                clone.GetComponent<EnemyBulletCollision>().SetDamage(boss.GetAreaDamage());
                clone.GetComponent<Rigidbody2D>().AddForce(clone.transform.up * 800);
                rotation += 30;
            }
        }
    }
}
