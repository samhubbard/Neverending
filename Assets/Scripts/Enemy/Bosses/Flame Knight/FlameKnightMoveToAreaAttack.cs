using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameKnightMoveToAreaAttack : StateMachineBehaviour
{
    public Transform areaAttackPoint;
    private Vector3 startPoint;
    private Transform boss;
    public float speed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // find the point and set that as the target position
        areaAttackPoint = GameObject.FindGameObjectWithTag("FK_AreaAttackPoint").GetComponent<Transform>();
        startPoint = areaAttackPoint.position;
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Transform>();
        speed = 12.0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // move the boss to the start position, upon reaching that, activate the trigger
        // to move to the state where the area attack will start
        boss.position = Vector3.MoveTowards(boss.position, startPoint, speed * Time.deltaTime);
        if (boss.position == startPoint)
        {
            animator.SetTrigger("PerformSwordAttack");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // reseting the trigger
        animator.ResetTrigger("PerformSwordAttack");
    }
}
