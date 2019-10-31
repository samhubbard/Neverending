using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartBossFight : MonoBehaviour
{
    Animator bossAnimator;
    public Slider bossHealthBar;
    public Image bossIcon;
    public Image background;
    public Image fill;
    public GameObject tileMap;
    public GameObject closingCollider;
    public static bool bossRoomEventActive = false;
    private bool fadeIn = false;
    private bool activated = false;

    private void Start()
    {
        // get the animator for the boss
        bossAnimator = GameObject.FindGameObjectWithTag("Boss").GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // once the player crosses the threshold, start the sequence
        if (collision.CompareTag("Player") && !activated)
        {
            // turn on the room event and get the camera to transitioning
            bossRoomEventActive = true;
            CameraController.transitioning = true;

            // close the room off
            tileMap.SetActive(true);
            closingCollider.SetActive(true);

            // lock the player movement and shooting abilities
            PlayerController.disabled = true;

            // Pan the camera to the boss
            CameraController camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
            camera.SetToBoss();

            // Get the enemy into the idle animation
            bossAnimator.SetTrigger("Appearance");

            // move on to the next section
            Invoke("ShowHealthBar", 0.2f);
        }
    }

    private void ShowHealthBar()
    {
        // Fade in the boss health bar and name
        bossHealthBar.gameObject.SetActive(true);
        bossIcon.gameObject.SetActive(true);

        // link in the boss controller
        FlameKnightController controller =
            GameObject.FindGameObjectWithTag("Boss").GetComponent<FlameKnightController>();

        // setup the health bar
        controller.SetUpHealthBar();

        // move on to the next section
        Invoke("BackToPlayer", 3.5f);
    }

    private void BackToPlayer()
    {
        // Pan the camera back to the player
        CameraController camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        camera.SetBackToPlayer();
        Invoke("SetUpCamera", 0.5f);
    }

    private void SetUpCamera()
    {
        CameraController.transitioning = false;
        Invoke("StartFight", 0.5f);
    }

    private void StartFight()
    {
        activated = true;
        // Unlock player movement and shooting abilities
        PlayerController.disabled = false;

        // Start the fight
        bossAnimator.SetTrigger("StartFight");
    }
}
