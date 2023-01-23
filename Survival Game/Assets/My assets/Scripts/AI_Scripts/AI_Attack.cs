using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AI_Attack : MonoBehaviour
{
    private Animator anim;

    public float attackRange; // play attack
    public float chaseRange; // posle 5 m radius da me brka
    public float stopFollowRange; // posle 10 m radius da prekine da me brka
    public float movementSpeed;

    private bool isChasing = false;
    private bool isAttacking;

    private UnityEvent bearAttack;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        CheckForEnemies();
        ChasePlayer();
        CheckIfPlayerInRange();
        CheckIfInAttackRange();
        AttackPlayer();
    }

    private void CheckForEnemies()
    {
        float playerDistance = Vector3.Distance(transform.position, FirstPersonController.Instance.transform.position);

        if (playerDistance <= chaseRange)
        {
            GetComponent<AI_Movement>().isInControl = false;
            isChasing = true;
            isAttacking = false;
        }
    }

    private void ChasePlayer()
    {

        if (isChasing)
        {
            transform.LookAt(FirstPersonController.Instance.transform);
            transform.position += transform.forward * movementSpeed * Time.deltaTime;
            anim.SetBool("isRunning", true);
        }
    }

    private void AttackPlayer()
    {
        if (isAttacking)
        {
            transform.LookAt(FirstPersonController.Instance.transform);
            anim.SetTrigger("Attack");
            isChasing = false;
        }
    }

    private void CheckIfPlayerInRange()
    {
        float playerDistance = Vector3.Distance(transform.position, FirstPersonController.Instance.transform.position);

        if (playerDistance >= stopFollowRange)
        {
            GetComponent<AI_Movement>().isInControl = true;
            isChasing = false;
            isAttacking = false;
        }
    }

    private void CheckIfInAttackRange()
    {
        float playerDistance = Vector3.Distance(transform.position, FirstPersonController.Instance.transform.position);

        if (playerDistance <= attackRange)
        {
            isAttacking = true;
            isChasing = false;
        }
    }

    private void DamagePlayer()
    {
        float playerDistance = Vector3.Distance(transform.position, FirstPersonController.Instance.transform.position);
        if (playerDistance <= attackRange)
        {
            bearAttack?.Invoke();
            PlayerState.Instance.currentHealth -= 20;
        }
    }

    public void bearAttackSound()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.bearAttackSound);
    }

    public static void DrawGizmoCircle(Vector3 center, float radius, Color color, int segments)
    {
        Gizmos.color = color;
        const float TWO_PI = Mathf.PI * 2;
        float step = TWO_PI / (float)segments;
        float theta = 0;
        float x = radius * Mathf.Cos(theta);
        float y = radius * Mathf.Sin(theta);
        Vector3 pos = center + new Vector3(x, 0, y);
        Vector3 newPos;
        Vector3 lastPos = pos;

        for (theta = step; theta < TWO_PI; theta += step)
        {
            x = radius * Mathf.Cos(theta);
            y = radius * Mathf.Sin(theta);
            newPos = center + new Vector3(x, 0, y);
            Gizmos.DrawLine(pos, newPos);
            pos = newPos;
        }
    }

    private void OnDrawGizmosSelected()
    {
        DrawGizmoCircle(transform.position, attackRange, Color.red, 30);
        DrawGizmoCircle(transform.position, chaseRange, Color.yellow, 30);
        DrawGizmoCircle(transform.position, stopFollowRange, Color.green, 30);
    }
}
