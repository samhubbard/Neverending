using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoisonBulletCollision : MonoBehaviour {

    public GameObject bulletHitAnimation;
    public GameObject damageOverTime;
    public GameObject soundObject;
    PlayerController playerController;
    EnemyController meleeEnemyController;
    RangedEnemy rangedEnemeyController;
    RoomEvent roomEvent;
    int baseDamage;
    int setDamage;
    float calculateDamage;
    float damageModifier;

    // get all references and setup the damage amounts based on the modifier
    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        baseDamage = playerController.GetBulletDamage();
        damageModifier = GameController.GetDDAModifier();

        if (damageModifier > 1f) {
            calculateDamage = baseDamage - (baseDamage * damageModifier / baseDamage);
        } else if (damageModifier < 1f) {
            calculateDamage = baseDamage + (baseDamage * damageModifier / baseDamage);
        } else {
            calculateDamage = baseDamage;
        }
        setDamage = Mathf.RoundToInt(calculateDamage);

    }

    // instantiate the DOT effect on the struck enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Collidables"))
        {
            Destroy(gameObject);
        }

        if (collision.CompareTag("meleeEnemy"))
        {
            // create poison effect
            Instantiate(damageOverTime, collision.transform.position, Quaternion.identity, collision.gameObject.transform);
            Instantiate(soundObject, collision.transform.position, Quaternion.identity);

            Destroy(gameObject);
            meleeEnemyController = collision.gameObject.GetComponent<EnemyController>();
            RoomEvent.bulletsHitEnemy += 1;
            GameObject hitAnimation = Instantiate(bulletHitAnimation, collision.transform.position, Quaternion.identity);
            hitAnimation.GetComponent<Animator>().SetTrigger("BulletHit");
            Destroy(hitAnimation, 2.0f);
        }

        if (collision.CompareTag("rangedEnemy")) 
        {
            Instantiate(damageOverTime, collision.transform.position, Quaternion.identity, collision.gameObject.transform);
            Instantiate(soundObject, collision.transform.position, Quaternion.identity);

            Destroy(gameObject);

            rangedEnemeyController = collision.gameObject.GetComponent<RangedEnemy>();
            RoomEvent.bulletsHitEnemy += 1;
            GameObject hitAnimation = Instantiate(bulletHitAnimation, collision.transform.position, Quaternion.identity);
            hitAnimation.GetComponent<Animator>().SetTrigger("BulletHit");
            Destroy(hitAnimation, 2.0f);
        }

        if (collision.CompareTag("Boss")) {
            if (collision.name == "Flame Knight(Clone)")
            {
                FlameKnightController controller =
                    GameObject.FindGameObjectWithTag("Boss").GetComponent<FlameKnightController>();

                Instantiate(damageOverTime, collision.transform.position, Quaternion.identity, collision.gameObject.transform);
                Instantiate(soundObject, collision.transform.position, Quaternion.identity);

                Destroy(gameObject);
                GameObject hitAnimation = Instantiate(bulletHitAnimation, transform.position, Quaternion.identity);
                hitAnimation.GetComponent<Animator>().SetTrigger("BulletHit");
                Destroy(hitAnimation, 2.0f);
            }
        }
    }
}
