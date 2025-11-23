using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] private float stopDistance = 0f;

    [Header("References")]
    [SerializeField] private AiDetector detector;
    [SerializeField] private Enemy enemy;

    private Vector2 currentDirection;
    private EnemyAvoid avoid;

    void Start()
    {
        avoid = GetComponent<EnemyAvoid>();

        if (enemy.GetRigidbody() != null)
        {
            enemy.GetRigidbody().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void FixedUpdate()
    {
        if (!enemy.GetCanMove() || detector.Target == null || !detector.TargetVisible)
        {
            enemy.GetRigidbody().velocity = Vector2.zero;
            currentDirection = Vector2.zero;
            enemy.IsMoving(false);
            return;
        }

        float distance = Vector2.Distance(transform.position, detector.Target.position);
        if (distance <= stopDistance)
        {
            enemy.GetRigidbody().velocity = Vector2.zero;
            enemy.IsMoving(false);
            return;
        }

        Vector2 directionToTarget = (detector.Target.position - transform.position).normalized;
        Vector2 avoidanceForce = avoid.CalculateAvoidance(detector.Target);

        currentDirection = (directionToTarget + avoidanceForce).normalized;

        enemy.GetRigidbody().velocity = currentDirection * moveSpeed * Time.fixedDeltaTime;

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (currentDirection != Vector2.zero)
        {
            enemy.UpdateMovement(currentDirection.x, currentDirection.y);
            enemy.IsMoving(true);
        }
        else
        {
            enemy.IsMoving(false);
        }
    }

    public void SetCanMove(bool value)
    {
        enemy.CanMove(value);
        if (!value)
        {
            enemy.GetRigidbody().velocity = Vector2.zero;
            enemy.IsMoving(false);
        }
    }

    public bool IsMoving()
    {
        return enemy.GetRigidbody().velocity.sqrMagnitude > 0.01f;
    }

    public Vector2 GetCurrentDirection()
    {
        return currentDirection;
    }

    public float GetDistanceToTarget()
    {
        if (detector.Target != null)
        {
            return Vector2.Distance(transform.position, detector.Target.position);
        }
        return float.MaxValue;
    }
}
