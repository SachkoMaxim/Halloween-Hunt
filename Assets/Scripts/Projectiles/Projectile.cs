using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float projectileSpeed = 0f;
    [SerializeField] private float maxDistance = 0f;
    [SerializeField] private LayerMask wallLayer;

    private Vector2 direction;
    private Vector2 startPosition;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 shootDirection, float maxDist, LayerMask walls)
    {
        direction = shootDirection.normalized;
        maxDistance = maxDist;
        wallLayer = walls;
        startPosition = transform.position;

        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }
    }

    private void Update()
    {
        float distanceTraveled = Vector2.Distance(startPosition, transform.position);
        if (distanceTraveled >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            Destroy(gameObject);
        }
    }
}
