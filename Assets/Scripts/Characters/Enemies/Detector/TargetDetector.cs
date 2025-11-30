using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : Detector
{
    [Header("Detection Settings")]
    [Range(0, 15)][SerializeField] private float viewRadius = 0f;

    [Header("Layers")]
    [SerializeField] public LayerMask playerLayer;
    [SerializeField] public LayerMask obstacleLayer;

    [Header("Gizmos")]
    [SerializeField] private bool showGizmos = true;

    private List<Transform> playerColliders;
    private Collider2D[] enemyColliders;

    private void Start()
    {
        enemyColliders = GetComponents<Collider2D>();
    }

    public override void Detect(EnemyAI enemyAI)
    {
        foreach (var enemyCollider in enemyColliders)
        {
            Vector2[] checkPoints = GetColliderCheckPoints(enemyCollider);

            bool playerFound = false;

            foreach (var checkPoint in checkPoints)
            {
                if (playerFound) break;

                Collider2D playerCollider = Physics2D.OverlapCircle(
                    checkPoint,
                    viewRadius,
                    playerLayer
                );

                if (playerCollider != null)
                {
                    enemyAI.isPlayerInRange = true;
                    Vector2 direction = (playerCollider.transform.position - transform.position).normalized;
                    RaycastHit2D hit = Physics2D.Raycast(
                        transform.position,
                        direction,
                        viewRadius,
                        obstacleLayer
                    );

                    if (hit.collider != null && (playerLayer & (1 << hit.collider.gameObject.layer)) != 0)
                    {
                        enemyAI.targetVisible = true;
                        Debug.DrawRay(transform.position, direction * viewRadius, Color.magenta);
                        playerColliders = new List<Transform>() { playerCollider.transform };
                        playerFound = true;
                    }
                    else
                    {
                        enemyAI.targetVisible = false;
                        playerColliders = null;
                    }
                }
                else
                {
                    playerColliders = null;
                    enemyAI.isPlayerInRange = false;
                    enemyAI.targetVisible = false;
                }
            }
        }
        enemyAI.targets = playerColliders;
    }

    private Vector2[] GetColliderCheckPoints(Collider2D collider)
    {
        Bounds bounds = collider.bounds;
        Vector2 center = bounds.center;
        Vector2 extents = bounds.extents;

        return new Vector2[]
        {
            center,
            new Vector2(center.x, center.y - extents.y),
            new Vector2(center.x - extents.x, center.y),
            new Vector2(center.x + extents.x, center.y),
            new Vector2(center.x, center.y + extents.y)
        };
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos == false)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        if (playerColliders == null)
            return;

        Gizmos.color = Color.magenta;
        foreach (var item in playerColliders)
        {
            Gizmos.DrawSphere(item.position, 0.2f);
        }
    }
}
