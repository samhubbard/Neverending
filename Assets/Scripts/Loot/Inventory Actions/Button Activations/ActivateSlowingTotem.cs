using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSlowingTotem : MonoBehaviour
{
    // variables for timer
    public float totalTimeTotemActive = 5.0f;
    private float timer;
    private float normalSpeed;
    private float bossSpeedOne;
    private float bossSpeedTwo;

    // instantiate the speeds and set the timer
    void Start()
    {
        normalSpeed = 0;
        bossSpeedOne = 0;
        bossSpeedTwo = 0;
        timer = totalTimeTotemActive;
    }

    // check the timer
    // if the time is up, set the speeds back to normal for the enemies and destroy the object
    void Update()
    {
        if (timer <= 0)
        {
            // Time's up!

            // if enemies are still slowed, speed them back up
            GameObject[] meleeEnemies = GameObject.FindGameObjectsWithTag("meleeEnemy");
            foreach (GameObject meleeEnemy in meleeEnemies)
            {
                meleeEnemy.transform.parent.GetComponent<Unit>().speed = normalSpeed;
            }

            string messageToSend = "Slowing Totem Expired.";
            DisplayMessage.MessageToQueue(messageToSend);

            Destroy(gameObject.transform.parent.gameObject);
            Destroy(gameObject, 0.2f);
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    // when an enemy walks into the collision area, reduce their speed to 10% of normal
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // melee enemies will slow down
        if (RoomEvent.roomEventActive)
        {
            if (collision.CompareTag("meleeEnemy"))
            {
                if (normalSpeed == 0)
                {
                    normalSpeed = collision.transform.parent.GetComponent<Unit>().speed;
                }
                collision.transform.parent.GetComponent<Unit>().speed = normalSpeed * .1f;

            }
        }
        else if (StartBossFight.bossRoomEventActive)
        {
            if (collision.CompareTag("Boss"))
            {
                if (collision.name == "Flame Knight(Clone)")
                {
                    GameObject flameKnight = GameObject.FindGameObjectWithTag("Boss");
                    FlameKnightMoveToAreaAttack moveToAreaAttackSpeed =
                        flameKnight.GetComponent<Animator>().GetBehaviour<FlameKnightMoveToAreaAttack>();
                    FlameKnightPrepForCircleAttack prepForCircleAttack =
                        flameKnight.GetComponent<Animator>().GetBehaviour<FlameKnightPrepForCircleAttack>();
                    FlameKnightCircleAttack circleAttack =
                        flameKnight.GetComponent<Animator>().GetBehaviour<FlameKnightCircleAttack>();
                        
                    if (bossSpeedOne == 0 && bossSpeedTwo == 0)
                    {

                        bossSpeedOne = moveToAreaAttackSpeed.speed;
                        bossSpeedTwo = circleAttack.speed;
                    }

                    moveToAreaAttackSpeed.speed = moveToAreaAttackSpeed.speed * .1f;
                    prepForCircleAttack.speed = prepForCircleAttack.speed * .1f;
                    circleAttack.speed = circleAttack.speed * .1f;
                }
            }
        }
    }

    // once the enemy leaves the collision area, set their speed back to normal
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (RoomEvent.roomEventActive)
        {
            // enemies are sped back up
            if (collision.CompareTag("meleeEnemy"))
            {
                collision.transform.parent.GetComponent<Unit>().speed = normalSpeed;
            }
        }
        else if (StartBossFight.bossRoomEventActive)
        {
            if (collision.CompareTag("Boss"))
            {
                if (collision.name == "Flame Knight(Clone)")
                {
                    GameObject flameKnight = GameObject.FindGameObjectWithTag("Boss");
                    FlameKnightMoveToAreaAttack moveToAreaAttackSpeed =
                        flameKnight.GetComponent<Animator>().GetBehaviour<FlameKnightMoveToAreaAttack>();
                    FlameKnightPrepForCircleAttack prepForCircleAttack =
                        flameKnight.GetComponent<Animator>().GetBehaviour<FlameKnightPrepForCircleAttack>();
                    FlameKnightCircleAttack circleAttack =
                        flameKnight.GetComponent<Animator>().GetBehaviour<FlameKnightCircleAttack>();

                    moveToAreaAttackSpeed.speed = bossSpeedOne;
                    prepForCircleAttack.speed = bossSpeedOne;
                    circleAttack.speed = bossSpeedTwo;
                }
            }
        }
    }
}
