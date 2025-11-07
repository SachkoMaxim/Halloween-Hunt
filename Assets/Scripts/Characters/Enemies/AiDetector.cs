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
        StartCoroutine(DetectionCoroutine());
    }

    void Update()
    {
        if (Target != null)
        {
            TargetVisible = CheckTargetVisible();
        }
    }

    private bool CheckTargetVisible()
    {
        var result = Physics2D.Raycast(
            transform.position,
            Target.position - transform.position,
            viewRadius,
            visibilityLayer
        );

        if (result.collider != null)
        {
            return (playerLayer & (1 << result.collider.gameObject.layer)) != 0;
        }
        return false;
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
        }
    }

    private void CheckInRange()
    {
        Collider2D collision = Physics2D.OverlapCircle(transform.position, viewRadius, playerLayer);

        if (collision != null)
        {
            Target = collision.transform;
        }
    }

    IEnumerator DetectionCoroutine()
    {
        yield return new WaitForSeconds(detectionCheckDelay);
        DetectTarget();
        StartCoroutine(DetectionCoroutine());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }
}
