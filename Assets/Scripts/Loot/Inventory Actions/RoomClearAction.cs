using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomClearAction : MonoBehaviour {

    private GameController gameController;
    public GameObject soundEffect;
    public GameObject flashHolder;

    // checks to make sure that there is a room event active
    // gets references to all of the enemies in the scene and then kills them
	public void DestroyAllEnemies()
    {
        if (RoomEvent.roomEventActive)
        {
            RoomEvent roomEvent = GameObject.FindGameObjectWithTag("RoomEvent").GetComponent<RoomEvent>();
            GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();

            GameObject[] meleeEnemies = GameObject.FindGameObjectsWithTag("meleeEnemy");
            GameObject[] rangedEnemies = GameObject.FindGameObjectsWithTag("rangedEnemy");
            Instantiate(flashHolder);

            foreach (GameObject meleeEnemy in meleeEnemies)
            {
                EnemyController.meleeEnemyCount -= 1;
                Destroy(meleeEnemy.transform.parent.gameObject);
                Destroy(meleeEnemy);

                gameController.UpdateScore(5);
            }

            foreach (GameObject rangedEnemy in rangedEnemies)
            {
                RangedEnemy.rangedEnemyCount -= 1;
                Destroy(rangedEnemy.transform.parent.gameObject);
                Destroy(rangedEnemy);

                gameController.UpdateScore(5);
            }

            Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

            Instantiate(soundEffect, player.position, Quaternion.identity);

            Destroy(gameObject);

        }
        else if (StartBossFight.bossRoomEventActive)
        {
            GameObject boss = GameObject.FindGameObjectWithTag("Boss");

            if (boss.name == "Flame Knight(Clone)")
            {
                FlameKnightController controller =
                    boss.GetComponent<FlameKnightController>();
                int maxHealth = controller.GetMaxHealth();
                controller.TakeDamage(maxHealth / 2);

                Destroy(gameObject);
            }
        }
        else
        {
            // Display error message
            string message = "No enemies in the area.";
            DisplayMessage.MessageToQueue(message);
        }
    }
}
