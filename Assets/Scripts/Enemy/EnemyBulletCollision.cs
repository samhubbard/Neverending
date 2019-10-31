using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletCollision : MonoBehaviour {

    PlayerController playerController;
    int damage;
    public GameObject animationObject;
    public GameObject projectileHitSound;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if the bullet hits a collidable in the room, destroy it
        if (collision.CompareTag("Collidables"))
        {
            Destroy(gameObject);
        }

        if (collision.CompareTag("Player"))
        {
            // animate an explosion for player feeback
            GameObject hitAnimation = Instantiate(animationObject, collision.transform.position, Quaternion.identity);
            hitAnimation.GetComponent<Animator>().SetTrigger("BulletHit");
            Destroy(hitAnimation, 2.0f);

            Instantiate(projectileHitSound, collision.transform.position, Quaternion.identity);

            // Do damage to the player
            playerController.TakeDamage(damage);

            // Destroy the bullet
            Destroy(gameObject);
        }
    }

    public void SetDamage(int _incomingDamage) {
        // this will set the damage of the bullet based on the damage modifier
        damage = _incomingDamage;
    }
}
