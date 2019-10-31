using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDamageReflection : MonoBehaviour
{
    // timer variables
    public float totalTimeToRunDamageReflection = 5.0f;
    private float timer;

    private GameObject[] meleeEnemies;
    private GameObject[] rangedEnemies;
    private GameObject boss;


    // Grab references to all active enemies on the field and start the reflection
    void Start()
    {
        timer = totalTimeToRunDamageReflection;

        if (RoomEvent.roomEventActive)
        {
            meleeEnemies = GameObject.FindGameObjectsWithTag("meleeEnemy");
            rangedEnemies = GameObject.FindGameObjectsWithTag("rangedEnemy");
        }
        else if (StartBossFight.bossRoomEventActive)
        {
            boss = GameObject.FindGameObjectWithTag("Boss");

        }
        
        StartDamageReflection();
    }

    // Once the timer is up, stop the reflection
    void Update()
    {
        if (timer <= 0)
        {
            StopDamageReflection();
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    // set all of the booleans located inside of the enemy controllers to true
    private void StartDamageReflection()
    {
        if (RoomEvent.roomEventActive)
        {
            foreach (GameObject meleeEnemy in meleeEnemies)
            {
                meleeEnemy.GetComponent<EnemyController>().damageRelflection = true;
            }

            foreach (GameObject rangedEnemy in rangedEnemies)
            {
                rangedEnemy.GetComponent<RangedEnemy>().damageReflection = true;
            }
        }
        else if (StartBossFight.bossRoomEventActive)
        {
            if (boss.name == "Flame Knight(Clone)")
            {
                boss.GetComponent<FlameKnightController>().damageReflectionActive = true;
            }
        }
    }

    // set all of the remaining reflection booleans in the remaining enemies to false
    // alert the player that the damage reflection is no longer active and destroy the object
    private void StopDamageReflection()
    {
        if (RoomEvent.roomEventActive)
        {
            meleeEnemies = GameObject.FindGameObjectsWithTag("meleeEnemy");
            rangedEnemies = GameObject.FindGameObjectsWithTag("rangedEnemy");

            foreach (GameObject meleeEnemy in meleeEnemies)
            {
                meleeEnemy.GetComponent<EnemyController>().damageRelflection = false;
            }

            foreach (GameObject rangedEnemy in rangedEnemies)
            {
                rangedEnemy.GetComponent<RangedEnemy>().damageReflection = false;
            }

            string messageToSend = "Damage no longer reflected.";
            DisplayMessage.MessageToQueue(messageToSend);

            Destroy(gameObject);
        }
        else if (StartBossFight.bossRoomEventActive)
        {
            if (boss.name == "Flame Knight(Clone)")
            {
                boss.GetComponent<FlameKnightController>().damageReflectionActive = false;
            }

            string messageToSend = "Damage no longer reflected.";
            DisplayMessage.MessageToQueue(messageToSend);

            Destroy(gameObject);
        }
    }
}
