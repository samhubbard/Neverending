using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlow : MonoBehaviour
{
    // timer variables
    public float totalTimeToRunTimeSlow = 5.0f;
    private float timer;

    private GameController gameController;
    private GameObject[] meleeEnemies;
    private GameObject[] rangedEnemies;

    // previous values
    private float normalBulletSpeed;
    private float normalWalkSpeed;
    private float normalAttackSpeed;

    // boss values
    private float normalBossSpeedOne;
    private float normalBossSpeedTwo;


    // Grab references to all of the enemies, get their normal speed values and then slow them way down
    void Start()
    {
        timer = totalTimeToRunTimeSlow;

        if (RoomEvent.roomEventActive)
        {
            meleeEnemies = GameObject.FindGameObjectsWithTag("meleeEnemy");
            rangedEnemies = GameObject.FindGameObjectsWithTag("rangedEnemy");

            if (rangedEnemies.Length != 0)
            {
                normalBulletSpeed = rangedEnemies[0].GetComponent<RangedEnemy>().bulletSpeed;
            }

            if (meleeEnemies.Length != 0)
            {
                normalAttackSpeed = meleeEnemies[0].GetComponent<EnemyController>().startTimeBetweenAttacks;
                normalWalkSpeed = meleeEnemies[0].transform.parent.GetComponent<Unit>().speed;
            }
        }
        else if (StartBossFight.bossRoomEventActive)
        {
            GameObject boss = GameObject.FindGameObjectWithTag("Boss");

            if (boss.name == "Flame Knight(Clone)")
            {
                FlameKnightMoveToAreaAttack moveToAreaAttackSpeed =
                boss.GetComponent<Animator>().GetBehaviour<FlameKnightMoveToAreaAttack>();
                FlameKnightPrepForCircleAttack prepForCircleAttack =
                    boss.GetComponent<Animator>().GetBehaviour<FlameKnightPrepForCircleAttack>();
                FlameKnightCircleAttack circleAttack =
                    boss.GetComponent<Animator>().GetBehaviour<FlameKnightCircleAttack>();

                normalBossSpeedOne = moveToAreaAttackSpeed.speed;
                normalBossSpeedTwo = circleAttack.speed;
            }
            
        }

        SlowDownEnemyTime();
    }

    // Check the timer => notify the player and then set the enemy speeds back to normal
    void Update()
    {
        if (timer <= 0)
        {
            NormalTime();

            string messageToSend = "Time back to normal.";
            DisplayMessage.MessageToQueue(messageToSend);
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    // run through each active enemy and reduce their speed values
    private void SlowDownEnemyTime()
    {
        if (RoomEvent.roomEventActive)
        {
            foreach (GameObject meleeEnemy in meleeEnemies)
            {
                meleeEnemy.GetComponent<EnemyController>().startTimeBetweenAttacks = normalAttackSpeed + 5.0f;
                meleeEnemy.transform.parent.GetComponent<Unit>().speed = normalWalkSpeed * .1f;
            }

            foreach (GameObject rangedEnemy in rangedEnemies)
            {
                rangedEnemy.GetComponent<RangedEnemy>().bulletSpeed = normalBulletSpeed * .1f;
            }
        }
        else if (StartBossFight.bossRoomEventActive)
        {

            GameObject boss = GameObject.FindGameObjectWithTag("Boss");

            if (boss.name == "Flame Knight(Clone)")
            {
                FlameKnightMoveToAreaAttack moveToAreaAttackSpeed =
                boss.GetComponent<Animator>().GetBehaviour<FlameKnightMoveToAreaAttack>();
                FlameKnightPrepForCircleAttack prepForCircleAttack =
                    boss.GetComponent<Animator>().GetBehaviour<FlameKnightPrepForCircleAttack>();
                FlameKnightCircleAttack circleAttack =
                    boss.GetComponent<Animator>().GetBehaviour<FlameKnightCircleAttack>();

                moveToAreaAttackSpeed.speed = normalBossSpeedOne * .1f;
                prepForCircleAttack.speed = normalBossSpeedOne * .1f;
                circleAttack.speed = normalBossSpeedTwo * .1f;
            }
            
        }
    }

    // run through all remaining enemies and set their speed values back to normal
    private void NormalTime()
    {
        if (RoomEvent.roomEventActive)
        {
            meleeEnemies = GameObject.FindGameObjectsWithTag("meleeEnemy");
            rangedEnemies = GameObject.FindGameObjectsWithTag("rangedEnemy");

            foreach (GameObject meleeEnemy in meleeEnemies)
            {
                meleeEnemy.GetComponent<EnemyController>().startTimeBetweenAttacks = normalAttackSpeed;
                meleeEnemy.transform.parent.GetComponent<Unit>().speed = normalWalkSpeed;
            }

            foreach (GameObject rangedEnemy in rangedEnemies)
            {
                rangedEnemy.GetComponent<RangedEnemy>().bulletSpeed = normalBulletSpeed;
            }
        }
        else if (StartBossFight.bossRoomEventActive)
        {
            GameObject boss = GameObject.FindGameObjectWithTag("Boss");

            if (boss.name == "Flame Knight(Clone)")
            {
                FlameKnightMoveToAreaAttack moveToAreaAttackSpeed =
                boss.GetComponent<Animator>().GetBehaviour<FlameKnightMoveToAreaAttack>();
                FlameKnightPrepForCircleAttack prepForCircleAttack =
                    boss.GetComponent<Animator>().GetBehaviour<FlameKnightPrepForCircleAttack>();
                FlameKnightCircleAttack circleAttack =
                    boss.GetComponent<Animator>().GetBehaviour<FlameKnightCircleAttack>();

                moveToAreaAttackSpeed.speed = normalBossSpeedOne;
                prepForCircleAttack.speed = normalBossSpeedOne;
                circleAttack.speed = normalBossSpeedTwo;
            }
        }

        Destroy(gameObject);
    }
}
