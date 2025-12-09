using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] protected float attackDistance = 0f;
    [SerializeField] protected float attackCooldown = 0f;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected float stopDuration = 0f;
    [SerializeField] private float attackDuration = 0f;

    [Header("Damage Settings")]
    [SerializeField] protected int attackDamage = 0;

    [Header("References")]
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected LayerMask wallLayer;
    [SerializeField] protected EnemyAI enemyAI;
    [SerializeField] protected Enemy enemy;

    protected float lastAttackTime = -999f;
    protected bool isPreparingAttack = false;

    protected virtual void FixedUpdate()
    {
        if (isPreparingAttack || enemyAI.currentTarget == null || !enemyAI.targetVisible)
        {
            return;
        }

        if (CanAttack())
        {
            StartCoroutine(PrepareAndAttack());
        }
    }

    protected virtual void PerformAttack()
    {

    }

    private bool CanAttack()
    {
        float distance = Vector2.Distance(transform.position, enemyAI.currentTarget.position);
        if (Time.time - lastAttackTime < attackCooldown || distance > attackDistance)
        {
            return false;
        }

        RaycastHit2D wallCheck = Physics2D.Raycast(
            transform.position,
            (enemyAI.currentTarget.position - transform.position).normalized,
            distance,
            wallLayer
        );

        return wallCheck.collider == null;
    }

    protected virtual IEnumerator PrepareAndAttack()
    {
        isPreparingAttack = true;
        enemy.CanMove(false);
        enemy.Attacking();

        yield return new WaitForSeconds(stopDuration);

        AudioManager.instance.PlaySFXClip(enemy.attackClip, transform);
        PerformAttack();

        yield return new WaitForSeconds(attackDuration);

        lastAttackTime = Time.time;
        attackPoint.localPosition = Vector2.zero;
        isPreparingAttack = false;
        enemy.CanMove(true);
    }
}
