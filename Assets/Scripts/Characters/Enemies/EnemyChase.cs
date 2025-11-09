using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] private bool canMove = true;

    [Header("References")]
    [SerializeField] private AiDetector detector;

    private Rigidbody2D rb;
    private Vector2 currentDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!canMove || detector.Target == null || !detector.TargetVisible)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        currentDirection = (detector.Target.position - transform.position).normalized;

        rb.velocity = currentDirection * moveSpeed * Time.fixedDeltaTime;
    }

    public bool IsMoving()
    {
        return rb.velocity.sqrMagnitude > 0.01f;
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
