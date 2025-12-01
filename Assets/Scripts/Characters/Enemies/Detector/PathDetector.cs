using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDetector : MonoBehaviour
{
    [Header("Path Settings")]
    [SerializeField] public List<Transform> patrolPoints = new List<Transform>();

    [Header("Gizmos")]
    [SerializeField] private bool showGizmos = true;

    public struct PathPoint
    {
        public int Index;
        public Vector2 Position;
    }

    public PathPoint GetClosestPathPoint(Vector2 enemyPosition)
    {
        var minDistance = float.MaxValue;
        var index = -1;
        for (int i = 0; i < patrolPoints.Count; i++)
        {
            var tempDistance = Vector2.Distance(enemyPosition, patrolPoints[i].position);
            if (tempDistance < minDistance)
            {
                minDistance = tempDistance;
                index = i;
            }
        }

        return new PathPoint { Index = index, Position = patrolPoints[index].position };
    }

    public PathPoint GetNextPathPoint(int index)
    {
        var newIndex = index + 1 >= patrolPoints.Count ? 0 : index + 1;
        return new PathPoint { Index = newIndex, Position = patrolPoints[newIndex].position };
    }

    private void OnDrawGizmos()
    {
        if (patrolPoints.Count == 0 || showGizmos == false)
            return;

        for (int i = patrolPoints.Count - 1; i >= 0; i--)
        {
            if (i == -1 || patrolPoints[i] == null)
                return;

            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(patrolPoints[i].position, 0.3f);

            if (patrolPoints.Count == 1 || i == 0)
                return;

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i - 1].position);

            if (patrolPoints.Count > 2 && i == patrolPoints.Count - 1)
            {
                Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[0].position);
            }
        }
    }
}
