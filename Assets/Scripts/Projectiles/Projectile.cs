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

    [Header("Audio Clips")]
    [SerializeField] protected AudioClip destroyedClip;

    private Vector2 direction;
    private Vector2 startPosition;
    private Rigidbody2D rb;

    private int bouncesRemaining = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(
        Vector2 shootDirection,
        float maxDist,
        LayerMask walls,
        int damage,
        float speed,
        int maxBounces
    )
    {
        direction = shootDirection.normalized;
        maxDistance = maxDist;
        wallLayer = walls;
        projectileDamage = damage;
        projectileSpeed = speed;
        bouncesRemaining = maxBounces;
        startPosition = transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed * Time.fixedDeltaTime;
        }
    }

    private void Update()
    {
        float distanceTraveled = Vector2.Distance(startPosition, transform.position);
        if (distanceTraveled >= maxDistance)
        {
            AudioManager.instance.PlaySFXClip(destroyedClip, transform);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    private void HandleCollision(Collider2D collision)
    {
        Destoryable destroyableObj = collision.GetComponent<Destoryable>();
        if (destroyableObj)
        {
            destroyableObj.TakeDamage(projectileDamage);
            Destroy(gameObject);
            return;
        }

        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            if (bouncesRemaining > 0)
            {
                HandleBounce(collision);
            }
            else
            {
                AudioManager.instance.PlaySFXClip(destroyedClip, transform);
                Destroy(gameObject);
                return;
            }
        }
    }

    private void HandleBounce(Collider2D collision)
    {
        bouncesRemaining--;

        Vector2 contact = collision.ClosestPoint(transform.position);
        Vector2 normal = (transform.position - (Vector3)contact).normalized;

        direction = Vector2.Reflect(direction, normal).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        rb.velocity = direction * projectileSpeed * Time.fixedDeltaTime;

        startPosition = transform.position;
    }

    public int GetDamage()
    {
        return projectileDamage;
    }
}
