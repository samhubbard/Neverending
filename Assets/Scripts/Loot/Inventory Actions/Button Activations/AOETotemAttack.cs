using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOETotemAttack : MonoBehaviour
{
    // variables for timer
    public float totalTimeToBeInScene = 0.5f;
    private float totalTimeInScene;

    // AOE explosion
    public GameObject explosion;

    // setup the timer
    private void Start()
    {
        totalTimeInScene = totalTimeToBeInScene;
    }

    // if the timer reaches zero, destroy the object
    private void Update()
    {
        if (totalTimeInScene <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            totalTimeInScene -= Time.deltaTime;
        }
    }

    // if an enemy is in the collision area, apply damage to that enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("meleeEnemy"))
        {
            EnemyController meleeEnemy = collision.GetComponent<EnemyController>();
            meleeEnemy.TakeDamage(3);
            GameObject aoeEffect = Instantiate(explosion, transform.position, Quaternion.identity);
            aoeEffect.GetComponent<Animator>().SetTrigger("Explode");

            Destroy(aoeEffect, 1.0f);

        }

        if (collision.CompareTag("rangedEnemy"))
        {
            RangedEnemy rangedEnemy = collision.GetComponent<RangedEnemy>();
            rangedEnemy.TakeDamage(3);
            GameObject aoeEffect = Instantiate(explosion, transform.position, Quaternion.identity);
            aoeEffect.GetComponent<Animator>().SetTrigger("Explode");

            Destroy(aoeEffect, 1.0f);
        }

        if (collision.CompareTag("Boss"))
        {
            if (collision.name == "Flame Knight(Clone)")
            {
                FlameKnightController controller = collision.GetComponent<FlameKnightController>();
                controller.TakeDamage(3);
                GameObject aoeEffect = Instantiate(explosion, transform.position, Quaternion.identity);
                aoeEffect.GetComponent<Animator>().SetTrigger("Explode");

                Destroy(aoeEffect, 1.0f);
            }
        }
    }
}
