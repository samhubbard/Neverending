using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameKnightAreaAttack : StateMachineBehaviour
{

    public float timer;
    public float startTimer = .25f;
    public GameObject fkAreaAttack;

    private int minValue;
    private int maxValue;
    private GameObject attack;
    private GameObject boss;
    private bool instantiated = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // setting all of the primary values
        startTimer = .25f;
        boss = GameObject.FindGameObjectWithTag("Boss");
        minValue = 9;
        maxValue = 12;
        timer = Random.Range(minValue, maxValue);
        instantiated = false;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // instantiate the area attack object in to start the barrage of bullets
        if (startTimer <= 0 && !instantiated)
        {
            instantiated = true;
            attack = Instantiate(fkAreaAttack, boss.transform.position, Quaternion.identity);
        }
        else
        {
            startTimer -= Time.deltaTime;
        }

        // once the attack is over, trigger the next animation and continue the fight
        if (timer <= 0)
        {
            animator.SetTrigger("SwordAttackOver");
            
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // destroy the attack object and reset the trigger so that it doesn't get caught up later
        Destroy(attack);
        animator.ResetTrigger("SwordAttackOver");
    }
}
