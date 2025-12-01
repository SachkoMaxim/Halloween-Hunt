using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehavior : SteeringBehavior
{
    [Header("Patrol Behavior")]
    [SerializeField] public PathDetector patrolPath;
    [Range(0.1f, 1)][SerializeField] public float arriveDistance = 0.1f;

    [Header("Gizmos")]
    [SerializeField] private bool showGizmos = true;

    private int currentIndex = -1;
    private Vector2 currentTarget;
    private bool initialized = false;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, EnemyAI enemyAI)
    {
        if (patrolPath == null || patrolPath.patrolPoints.Count < 2)
            return (danger, interest);

        if (!initialized)
        {
            var closest = patrolPath.GetClosestPathPoint(transform.position);
            currentIndex = closest.Index;
            currentTarget = closest.Position;
            initialized = true;
        }

        float dist = Vector2.Distance(transform.position, currentTarget);

        if (dist < arriveDistance)
        {
            var next = patrolPath.GetNextPathPoint(currentIndex);
            currentIndex = next.Index;
            currentTarget = next.Position;
        }

        Vector2 direction = (currentTarget - (Vector2)transform.position).normalized;

        for (int i = 0; i < 8; i++)
        {
            float dot = Vector2.Dot(direction, Directions.eightDirections[i]);
            if (dot > 0)
                interest[i] = Mathf.Max(interest[i], dot);
        }

        return (danger, interest);
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(currentTarget, 0.2f);
        }
    }
}
