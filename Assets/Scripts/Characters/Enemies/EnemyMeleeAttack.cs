using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [Header("Melee Attack Settings")]
    [SerializeField] private float attackDistance = 0f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private float attackCooldown = 0f;
    [SerializeField] private int attackDamage = 0;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float stopDuration = 0f;

    [Header("References")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private AiDetector detector;
    [SerializeField] private EnemyChase chase;

    private float lastAttackTime = -999f;
    private bool isPreparingAttack = false;

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

    void FixedUpdate()
    {
        if (isPreparingAttack || detector.Target == null || !detector.TargetVisible)
        {
            return;
        }

        if (CanAttack())
        {
            Vector2 directionToTarget = (detector.Target.position - transform.position).normalized;
            Vector2 attackDirection = GetBestAttackDirection(directionToTarget);
            attackPoint.localPosition = attackDirection;
            StartCoroutine(PrepareAndAttack());
        }
    }

    private bool CanAttack()
    {
        float distance = Vector2.Distance(transform.position, detector.Target.position);
        if (Time.time - lastAttackTime < attackCooldown || distance > attackDistance)
        {
            return false;
        }

        RaycastHit2D wallCheck = Physics2D.Raycast(
            transform.position,
            (detector.Target.position - transform.position).normalized,
            distance,
            wallLayer
        );

        return wallCheck.collider == null;
    }

    private IEnumerator PrepareAndAttack()
    {
        isPreparingAttack = true;
        chase.SetCanMove(false);

        yield return new WaitForSeconds(stopDuration);

        PerformAttack();

        lastAttackTime = Time.time;
        attackPoint.localPosition = Vector2.zero;
        isPreparingAttack = false;
        chase.SetCanMove(true);
    }

    private void PerformAttack()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        Collider2D[] destroyableTargets = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, wallLayer);

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
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
