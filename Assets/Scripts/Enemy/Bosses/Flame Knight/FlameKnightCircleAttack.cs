using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameKnightCircleAttack : StateMachineBehaviour
{
    public Transform circlePoints;
    private Vector3[] path;
    private Transform boss;
    public float speed = 20.0f;
    private int targetPointIndex;
    private Vector3 targetPoint;

    public float timer;
    private int minValue;
    private int maxValue;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // setting the intitial values
        minValue = 7;
        maxValue = 10;
        timer = Random.Range(minValue, maxValue);
        circlePoints = GameObject.FindGameObjectWithTag("FK_CircleAttackPoints").GetComponent<Transform>();
        
        // setting up the random path that the boss will follow
        path = new Vector3[20];
        Vector3 currentPoint = circlePoints.GetChild(0).position;
        path[0] = currentPoint;
        for (int i = 1; i < path.Length; i++)
        {
            int rand = Random.Range(0, 4);
            Vector3 possiblePoint = circlePoints.GetChild(rand).position;
            if (possiblePoint != currentPoint)
            {
                path[i] = possiblePoint;
                currentPoint = possiblePoint;
            }
            else
            {
                i--;
            }
        }

        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Transform>();

        // setting up the target index point to get the boss moving and then resetting
        // at every point
        int targetPointIndex = 1;
        targetPoint = path[targetPointIndex];

        FlameKnightController.circleAttackActive = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // moving the boss to the next point
        boss.position = Vector3.MoveTowards(boss.position, targetPoint, speed * Time.deltaTime);

        // once the boss hits the target point, reset the target index and moving the boss
        if (boss.position == targetPoint)
        {
            targetPointIndex = (targetPointIndex + 1) % path.Length;
            targetPoint = path[targetPointIndex];
        }

        // once the boss timer is up, trigger the next animation
        if (timer <= 0)
        {
            animator.SetTrigger("CircleAttackComplete");
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // reset the trigger and boolean to signify that the circle attack is no longer active
        // so the player won't take damage for walking into that circle collider
        animator.ResetTrigger("CircleAttackComplete");
        FlameKnightController.circleAttackActive = false;
    }
}
