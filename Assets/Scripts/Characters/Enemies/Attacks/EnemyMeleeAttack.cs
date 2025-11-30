using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : EnemyAttack
{
    [Header("Melee Attack Settings")]
    [SerializeField] private float meleeRange = 0f;

    private Vector2[] attackDirections = new Vector2[]
    {
        new Vector2(0.5f, 0),
        new Vector2(0, 0.5f),
        new Vector2(-0.5f, 0),
        new Vector2(0, -0.5f),
        new Vector2(0.35f, 0.35f),
        new Vector2(-0.35f, 0.35f),
        new Vector2(-0.35f, -0.35f),
        new Vector2(0.35f, -0.35f)
    };

    protected override IEnumerator PrepareAndAttack()
    {
        Vector2 directionToTarget = (enemyAI.currentTarget.position - transform.position).normalized;
        Vector2 attackDirection = GetBestAttackDirection(directionToTarget);
        attackPoint.localPosition = attackDirection;

        enemy.UpdateMovement(attackPoint.localPosition.normalized.x, attackPoint.localPosition.normalized.y);

        yield return StartCoroutine(base.PrepareAndAttack());
        attackPoint.localPosition = Vector2.zero;
    }

    protected override void PerformAttack()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, meleeRange, playerLayer);
        Collider2D[] destroyableTargets = Physics2D.OverlapCircleAll(attackPoint.position, meleeRange, wallLayer);

        foreach (Collider2D collider in destroyableTargets)
        {
            Destoryable destroyable = collider.GetComponent<Destoryable>();
            if (destroyable != null)
            {
                destroyable.TakeDamage(attackDamage);
            }
        }

        if (hitPlayer != null)
        {
            Player player = hitPlayer.transform.parent.GetComponent<Player>();
            player.TakeDamage(attackDamage);
        }
    }

    private Vector2 GetBestAttackDirection(Vector2 targetDirection)
    {
        float maxDot = -999f;
        Vector2 bestDirection = Vector2.zero;

        foreach (Vector2 dir in attackDirections)
        {
            float dot = Vector2.Dot(targetDirection, dir.normalized);
            if (dot > maxDot)
            {
                maxDot = dot;
                bestDirection = dir;
            }
        }

        return bestDirection;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, meleeRange);
    }
}
