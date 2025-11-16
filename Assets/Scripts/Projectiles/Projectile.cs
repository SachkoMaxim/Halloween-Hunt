using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float projectileSpeed = 0f;
    [SerializeField] private float maxDistance = 0f;
    [SerializeField] private int projectileDamage = 0;
    [SerializeField] private LayerMask wallLayer;

    private Vector2 direction;
    private Vector2 startPosition;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 shootDirection, float maxDist, LayerMask walls, int damage, float speed)
    {
        direction = shootDirection.normalized;
        maxDistance = maxDist;
        wallLayer = walls;
        projectileDamage = damage;
        projectileSpeed = speed;
        startPosition = transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

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
        Destoryable destroyableObj = collision.GetComponent<Destoryable>();
        if (destroyableObj)
        {
            destroyableObj.TakeDamage(projectileDamage);
            Destroy(gameObject);
        }

        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            Destroy(gameObject);
        }
    }
}
