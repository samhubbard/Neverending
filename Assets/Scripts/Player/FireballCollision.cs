using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireballCollision : MonoBehaviour
{

    public GameObject bulletHitAnimation;
    public GameObject aoeAttack;
    public GameObject explosionSound;
    GameController gameController; // I might be able to remove this...
    PlayerController playerController;
    EnemyController meleeEnemyController;
    RangedEnemy rangedEnemeyController;
    RoomEvent roomEvent;
    int baseDamage;
    int setDamage;
    float calculateDamage;
    float damageModifier;

    // gets references to the player and game controllers
    // sets the damage based on the damage modifier
    private void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        baseDamage = playerController.GetBulletDamage();
        damageModifier = GameController.GetDDAModifier();

        if (damageModifier > 1f)
        {
            calculateDamage = baseDamage - (baseDamage * damageModifier / baseDamage);
        }
        else if (damageModifier < 1f)
        {
            calculateDamage = baseDamage + (baseDamage * damageModifier / baseDamage);
        }
        else
        {
            calculateDamage = baseDamage;
        }
        setDamage = Mathf.RoundToInt(calculateDamage);
    }

    // this handles the bullet collision
    // if it hits an enemy it creates an explosion effect and does damage to the enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Collidables"))
        {
            Destroy(gameObject);
        }

        if (collision.CompareTag("meleeEnemy"))
        {
            // create AOE explosion effect
            Instantiate(aoeAttack, collision.transform.position, Quaternion.identity);
            Instantiate(explosionSound, collision.transform.position, Quaternion.identity);

            Destroy(gameObject);
            meleeEnemyController = collision.gameObject.GetComponent<EnemyController>();
            meleeEnemyController.TakeDamage(setDamage);
            RoomEvent.bulletsHitEnemy += 1;
            GameObject hitAnimation = Instantiate(bulletHitAnimation, collision.transform.position, Quaternion.identity);
            hitAnimation.GetComponent<Animator>().SetTrigger("BulletHit");
            Destroy(hitAnimation, 2.0f);
        }

        if (collision.CompareTag("rangedEnemy"))
        {
            // create AOE explosion effect
            Instantiate(aoeAttack, collision.transform.position, Quaternion.identity);
            Instantiate(explosionSound, collision.transform.position, Quaternion.identity);

            Destroy(gameObject);

            rangedEnemeyController = collision.gameObject.GetComponent<RangedEnemy>();
            rangedEnemeyController.TakeDamage(setDamage);
            RoomEvent.bulletsHitEnemy += 1;
            GameObject hitAnimation = Instantiate(bulletHitAnimation, collision.transform.position, Quaternion.identity);
            hitAnimation.GetComponent<Animator>().SetTrigger("BulletHit");
            Destroy(hitAnimation, 2.0f);
        }

        if (collision.CompareTag("Boss"))
        {

            print(collision.name);
            if (collision.name == "Flame Knight(Clone)")
            {
                FlameKnightController controller =
                    GameObject.FindGameObjectWithTag("Boss").GetComponent<FlameKnightController>();

                Instantiate(aoeAttack, transform.position, Quaternion.identity);
                Instantiate(explosionSound, collision.transform.position, Quaternion.identity);

                controller.TakeDamage(setDamage);

                GameObject hitAnimation = Instantiate(bulletHitAnimation, transform.position, Quaternion.identity);
                hitAnimation.GetComponent<Animator>().SetTrigger("BulletHit");
                Destroy(gameObject);
                Destroy(hitAnimation, 2.0f);
            }

            
        }
    }
}
