using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAvoid : MonoBehaviour
{
    [Header("Obstacle Detection")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float raycastDistance = 1.5f;
    [SerializeField] private float detectionRadius = 0.4f;

    [Header("Avoidance Behavior")]
    [SerializeField] private float avoidanceForce = 1.5f;
    [SerializeField] private float raycastCount = 9;
    [SerializeField] private float exitStuckDistance = 0.3f;

    [Header("Reference")]
    [SerializeField] private Transform collision;
    [SerializeField] private Enemy enemy;

    private Vector2 lastSafeDirection = Vector2.right;
    private float stuckTimer = 0f;
    private const float stuckThreshold = 0.3f;

    public Vector2 CalculateAvoidance(Transform targetTransform)
    {
        Vector2 directionToTarget = (targetTransform.position - transform.position).normalized;

        if (IsStuck())
        {
            stuckTimer += Time.fixedDeltaTime;
            return GetEmergencyEscapeDirection() * avoidanceForce * 2f;
        }

        stuckTimer = 0f;

        if (!IsPathClear(directionToTarget))
        {
            Vector2 bestAvoidDir = FindBestAvoidanceDirection(directionToTarget);
            lastSafeDirection = bestAvoidDir;
            return bestAvoidDir * avoidanceForce;
        }

        lastSafeDirection = directionToTarget;
        return Vector2.zero;
    }

    private bool IsPathClear(Vector2 direction)
    {
        int rayCount = (int)raycastCount;
        float angleStep = 60f / (rayCount - 1);

        for (int i = 0; i < rayCount; i++)
        {
            float angle = -30f + (i * angleStep);
            Vector2 rayDir = Quaternion.Euler(0, 0, angle) * direction;

            RaycastHit2D hit = Physics2D.Raycast(
                collision.GetComponent<Collider2D>().bounds.center,
                rayDir,
                detectionRadius,
                obstacleLayer
            );

            if (hit.collider != null)
            {
                return false;
            }
        }

        return true;
    }

    private Vector2 FindBestAvoidanceDirection(Vector2 targetDirection)
    {
        Vector2 leftDirection = Quaternion.Euler(0, 0, 90) * targetDirection;
        Vector2 rightDirection = Quaternion.Euler(0, 0, -90) * targetDirection;

        float leftScore = EvaluateDirection(leftDirection, targetDirection);
        float rightScore = EvaluateDirection(rightDirection, targetDirection);

        if (leftScore >= rightScore)
        {
            return leftDirection.normalized;
        }
        else
        {
            return rightDirection.normalized;
        }
    }

    private float EvaluateDirection(Vector2 direction, Vector2 targetDirection)
    {
        float clearScore = 0f;
        int sampleCount = 5;

        for (int i = 0; i < sampleCount; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                direction,
                raycastDistance * (i + 1) / sampleCount,
                obstacleLayer
            );

            if (hit.collider == null)
            {
                clearScore += 1f;
            }
        }

        float alignmentBonus = Vector2.Dot(direction, targetDirection) * 0.5f;
        return clearScore + alignmentBonus;
    }

    private bool IsStuck()
    {
        return enemy.GetRigidbody().velocity.sqrMagnitude < 0.01f && stuckTimer > stuckThreshold;
    }

    private Vector2 GetEmergencyEscapeDirection()
    {
        Vector2[] directions = new Vector2[]
        {
            Vector2.right, Vector2.left, Vector2.up, Vector2.down,
            new Vector2(1, 1).normalized, new Vector2(-1, 1).normalized,
            new Vector2(1, -1).normalized, new Vector2(-1, -1).normalized
        };

        foreach (Vector2 dir in directions)
        {
            bool pathClear = true;
            for (int i = 0; i < 3; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(
                    transform.position,
                    dir,
                    exitStuckDistance * (i + 1) / 3,
                    obstacleLayer
                );

                if (hit.collider != null)
                {
                    pathClear = false;
                    break;
                }
            }

            if (pathClear)
            {
                return dir;
            }
        }

        return lastSafeDirection;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Vector2 forward = transform.right;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + lastSafeDirection * raycastDistance);
    }
}
