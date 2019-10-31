using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameKnightPrepForCircleAttack : StateMachineBehaviour
{
    public Transform circleAttackPoints;
    private Vector3 startPoint;
    private Transform boss;
    public float speed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // get the start point for the circle attack
        circleAttackPoints = GameObject.FindGameObjectWithTag("FK_CircleAttackPoints").GetComponent<Transform>();
        startPoint = circleAttackPoints.GetChild(0).position;
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Transform>();
        speed = 12.0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // move the boss to the start point
        // upon arrival, activate the trigger to start the attack
        boss.position = Vector3.MoveTowards(boss.position, startPoint, speed * Time.deltaTime);
        if (boss.position == startPoint)
        {
            animator.SetTrigger("IsAtStart");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // resetting the trigger
        animator.ResetTrigger("IsAtStart");
    }
}
