using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEMageAttack : MonoBehaviour
{
    // variables for timer
    public float totalTimeToBeInScene = 0.5f;
    private float totalTimeInScene;

    // AOE explosion
    public GameObject explosion;

    // sets up the timer
    private void Start()
    {
        totalTimeInScene = totalTimeToBeInScene;
    }

    // checks the timer and destroys the game object with time is up
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

    // when the bullet hits the enemy, instantiate an explosion that wil do damage to all surrounding enemies
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
                FlameKnightController controller = 
                    GameObject.FindGameObjectWithTag("Boss").GetComponent<FlameKnightController>();
                controller.TakeDamage(3);
                GameObject aoeEffect = Instantiate(explosion, transform.position, Quaternion.identity);
                aoeEffect.GetComponent<Animator>().SetTrigger("Explode");

                Destroy(aoeEffect, 1.0f);
            }
        }
    }
}
