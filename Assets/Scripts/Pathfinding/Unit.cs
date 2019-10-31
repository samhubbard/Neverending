using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Unit : MonoBehaviour
{
    // needed variables for the unit
    public Transform player; // this will hold the transform for the player
    public float speed = 0.1f; // This will be set based on the current difficulty modifier
    Vector3[] path; // this will hold the path for the enemy to travel
    int targetIndex; // this is variable that will move the enemy along the array
    // let's try a time delay to see if I can't get a more dynamic (and smooth) pathing
    private float timeBetweenPathing;
    public float startTimeBetweenPathing = 1.0f;
    public float attackRange = 0.1f;
    private bool isAttacking;

    private void Start()
    {
        GameController c = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        float difficultyModifier = c.playerStats.CurrentDifficultyModifier;
        float modifiedModifier = difficultyModifier * .1f;
        speed = 5 + (5 * modifiedModifier);
        if (speed > 12)
        {
            speed = 12;
        }

    }

    private void Update()
    {
        if (timeBetweenPathing <= 0 && Vector3.Distance(transform.localPosition, player.localPosition) > attackRange 
            && !isAttacking)
        {
            PathRequestManager.RequestPath(transform.localPosition, player.localPosition, OnPathFound);
            timeBetweenPathing = startTimeBetweenPathing;
        }
        else
        {
            timeBetweenPathing -= Time.deltaTime;
        }
    }

    // once the path has been sent back, start the co-routine
    public void OnPathFound(Vector3[] newPath, bool pathSuccess)
    {
        if (pathSuccess)
        {
            path = newPath; // set the array to the path that just came in
            StopCoroutine("FollowPath"); // if the co-routine is already running, stop it
            StartCoroutine("FollowPath"); // start the co-routine with the (new) path
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine("FollowPath");
            isAttacking = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isAttacking = false;
        }
    }

    // this is the block of code that will send the enemy toward the player
    IEnumerator FollowPath()
    {
        if (path == null || path.Length == 0) {
            yield return null;
        }
        Vector3 currentWaypoint = path[0]; // setting the current waypoint for the enemy to travel
        // infinite loop that will be manually broken out of
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++; // when the enemy reaches the waypoint, increase the index
                if (targetIndex >= path.Length)
                {
                    yield break; // when the target index reaches the end of the array, break out of the loop
                }
                // set the new waypoint for the enemy
                currentWaypoint = path[targetIndex];
            }
            // move the enemy to the current waypoint in the array
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }
}