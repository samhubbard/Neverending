using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameKnightIntroBehavior : StateMachineBehaviour
{
    private int rand;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // get either a zero or one and then tell the boss to attack
        rand = Random.Range(0, 2);

        switch (rand)
        {
            case 0:
                // if the random number was a zero, circle attack
                animator.SetTrigger("CircleAttack");
                break;
            case 1:
                // if the random number was a one, AOE attack
                animator.SetTrigger("SwordAttack");
                break;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // no code needed here, I'm keeping it here incase I need it
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // even though only one trigger was set, reset both of them upon exit
        animator.ResetTrigger("CircleAttack");
        animator.ResetTrigger("SwordAttack");
    }
}
