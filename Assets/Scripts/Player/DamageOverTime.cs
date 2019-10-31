using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : MonoBehaviour
{
    // create variables for a timer to apply damage
    public float totalSecondsToApplyDamage = 0.5f;
    private float runningSecondsToApplyDamage;

    // create variables for a timer to kill the DOT
    public float totalSecondsToRunDOT = 5.0f;
    public float runningSecondsToRunDOT;

    private AudioSource audioSource;

    // sets up the timers, audio source, and volume
    void Start()
    {
        runningSecondsToApplyDamage = totalSecondsToApplyDamage;
        runningSecondsToRunDOT = totalSecondsToRunDOT;
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = 0.4f * GameController.sfxVolume;
    }

    // checks the timers and either applies damage to the target or destroys the game object
    void Update()
    {
        audioSource.volume = 0.4f * GameController.sfxVolume;
        // run a check to see if it's time to kill the DOT
        if (runningSecondsToRunDOT <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            runningSecondsToRunDOT -= Time.deltaTime;
        }
        

        // run a check to see if it's time to apply damage
        if (runningSecondsToApplyDamage <= 0)
        {
            if (gameObject.transform.parent.CompareTag("meleeEnemy"))
            {
                EnemyController meleeEnemy = gameObject.transform.parent.GetComponent<EnemyController>();
                meleeEnemy.PlayPoisonDamageAnimation();
                audioSource.Play();
                meleeEnemy.TakeDamage(2);
            }
            else if (gameObject.transform.parent.CompareTag("rangedEnemy"))
            {
                RangedEnemy rangedEnemy = gameObject.transform.parent.GetComponent<RangedEnemy>();
                rangedEnemy.PlayPoisonDamageAnimation();
                audioSource.Play();
                rangedEnemy.TakeDamage(2);
            }
            else if (gameObject.transform.parent.CompareTag("Boss"))
            {

                if (gameObject.transform.parent.name == "Flame Knight(Clone)")
                {
                    FlameKnightController controller =
                        GameObject.FindGameObjectWithTag("Boss").GetComponent<FlameKnightController>();
                    controller.PlayPoisonDamageAnimation();
                    audioSource.Play();
                    controller.TakeDamage(2);
                }
            }
            // need to add this stuff to the boss(es).
            runningSecondsToApplyDamage = totalSecondsToApplyDamage;
        }
        else
        {
            runningSecondsToApplyDamage -= Time.deltaTime;
        }
    }
}
