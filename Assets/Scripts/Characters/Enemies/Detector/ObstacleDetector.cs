using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetector : Detector
{
    [Header("Detection Settings")]
    [Range(0, 15)][SerializeField] private float detectionRadius = 0f;

    [Header("Layers")]
    [SerializeField] public LayerMask obstacleLayer;

    [Header("Gizmos")]
    [SerializeField] private bool showGizmos = true;

    private Collider2D[] colliders;

    public override void Detect(EnemyAI enemyAI)
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, obstacleLayer);
        enemyAI.obstacles = colliders;
    }

    private void OnDrawGizmos()
    {
        if (showGizmos == false)
            return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        if (Application.isPlaying && colliders != null)
        {
            Gizmos.color = Color.red;
            foreach (Collider2D obstacleCollider in colliders)
            {
                Gizmos.DrawSphere(obstacleCollider.transform.position, 0.2f);
            }
        }
    }
}
