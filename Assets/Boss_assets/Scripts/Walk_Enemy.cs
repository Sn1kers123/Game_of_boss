using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Walk_Enemy : StateMachineBehaviour
{
    public float _speed = 2.5f;
    public float _attackRange = 3f;
    Transform player;
    Rigidbody2D rb;
    Boss boss;
    Health health_boss;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
        health_boss = animator.GetComponent<Health>();
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        Collider2D bossCollider = rb.GetComponent<Collider2D>();

        if (playerCollider != null && bossCollider != null)
        {
            Physics2D.IgnoreCollision(playerCollider, bossCollider);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();
        Vector2 target = new Vector2(player.position.x, player.position.y);
        Vector2 NewPosition = Vector2.MoveTowards(rb.position, target, _speed * Time.fixedDeltaTime);
        rb.MovePosition(NewPosition);

        if (Vector2.Distance(player.position, rb.position) <= _attackRange) {
            animator.SetTrigger("Attack");
        }
        if (Vector2.Distance(player.position, rb.position) <= _attackRange*4) {
            animator.SetTrigger("Attack3");
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Attack3");
    }


}
