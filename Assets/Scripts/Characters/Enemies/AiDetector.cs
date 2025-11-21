using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDetector : MonoBehaviour
{
    [Header("Detection Setting")]
    [Range(0, 15)][SerializeField] private float viewRadius = 0f;
    [SerializeField] private float detectionCheckDelay = 0.1f;

    [Header("Layers")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask visibilityLayer;

    [Header("Target")]
    [SerializeField] private Transform target = null;
    [field: SerializeField] public bool TargetVisible { get; private set; }

    [Header("References")]
    [SerializeField] public GameObject exclamationMark;

    private Collider2D[] enemyColliders;
    private Collider2D[] targetColliders;
    private bool isPlayerInRange = false;

    public Transform Target
    {
        get => target;
        set
        {
            target = value;
            TargetVisible = false;
        }
    }

    void Start()
    {
        enemyColliders = GetComponents<Collider2D>();
        StartCoroutine(DetectionCoroutine());
    }

    void Update()
    {
        if (Target != null)
        {
            TargetVisible = CheckTargetVisible();

            if (TargetVisible && isPlayerInRange)
            {
                StartCoroutine(Alert());
            }
        }
    }

    private bool CheckTargetVisible()
    {
        if (targetColliders == null || targetColliders.Length == 0)
            return false;

        foreach (var enemyCollider in enemyColliders)
        {
            Vector2[] enemyCheckPoints = GetColliderCheckPoints(enemyCollider);

            foreach (var targetCollider in targetColliders)
            {
                Vector2[] targetCheckPoints = GetColliderCheckPoints(targetCollider);

                foreach (var enemyPoint in enemyCheckPoints)
                {
                    foreach (var targetPoint in targetCheckPoints)
                    {
                        Vector2 direction = targetPoint - enemyPoint;
                        float distance = direction.magnitude;

                        if (distance > viewRadius)
                            continue;

                        var result = Physics2D.Raycast(
                            enemyPoint,
                            direction.normalized,
                            distance,
                            visibilityLayer
                        );

                        if (result.collider != null &&
                            (playerLayer & (1 << result.collider.gameObject.layer)) != 0)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
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

    private void DetectTarget()
    {
        if (Target == null)
        {
            CheckInRange();
        }
        else if (Target != null)
        {
            DetectOutOfRange();
        }
    }

    private void DetectOutOfRange()
    {
        if (Target == null ||
            Target.gameObject.activeSelf == false ||
            Vector2.Distance(transform.position, Target.position) > viewRadius)
        {
            Target = null;
            targetColliders = null;
            isPlayerInRange = false;
        }
    }

    private void CheckInRange()
    {
        foreach (var enemyCollider in enemyColliders)
        {
            Vector2[] checkPoints = GetColliderCheckPoints(enemyCollider);

            foreach (var checkPoint in checkPoints)
            {
                Collider2D collision = Physics2D.OverlapCircle(
                    checkPoint,
                    viewRadius,
                    playerLayer
                );

                if (collision != null)
                {
                    Target = collision.transform;
                    targetColliders = Target.GetComponents<Collider2D>();
                    isPlayerInRange = true;
                    return;
                }
            }
        }
    }

    IEnumerator DetectionCoroutine()
    {
        yield return new WaitForSeconds(detectionCheckDelay);
        DetectTarget();
        StartCoroutine(DetectionCoroutine());
    }

    IEnumerator Alert()
    {
        exclamationMark.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        exclamationMark.SetActive(false);
        isPlayerInRange = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }
}
