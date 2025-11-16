using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAttack : MonoBehaviour
{
    [Header("Range Attack Settings")]
    [SerializeField] private float shotDistance = 0f;
    [SerializeField] private int projectilesPerShot = 1;
    [SerializeField] private float spreadAngle = 0f;
    [SerializeField] private float multiShotDelay = 0.1f;
    [SerializeField] private float shotCooldown = 0f;
    [SerializeField] private Transform shotPoint;
    [SerializeField] private float stopDuration = 0f;

    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 0f;
    [SerializeField] private int projectileDamage = 0;
    [SerializeField] private float maxProjectileDistance = 0f;
    [SerializeField] private int maxBounces = 0;

    [Header("References")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private AiDetector detector;
    [SerializeField] private EnemyChase chase;

    private float lastShotTime = -999f;
    private bool isPreparingAttack = false;

    void FixedUpdate()
    {
        if (isPreparingAttack || detector.Target == null || !detector.TargetVisible)
        {
            return;
        }

        if (CanAttack())
        {
            StartCoroutine(PrepareAndAttack());
        }
    }

    private bool CanAttack()
    {
        float distance = Vector2.Distance(transform.position, detector.Target.position);
        if (Time.time - lastShotTime < shotCooldown || distance > shotDistance)
        {
            return false;
        }

        RaycastHit2D wallCheck = Physics2D.Raycast(
            transform.position,
            (detector.Target.position - transform.position).normalized,
            distance,
            wallLayer
        );

        return wallCheck.collider == null;
    }

    private IEnumerator PrepareAndAttack()
    {
        isPreparingAttack = true;
        chase.SetCanMove(false);

        yield return new WaitForSeconds(stopDuration);

        PerformAttack();

        lastShotTime = Time.time;
        isPreparingAttack = false;
        chase.SetCanMove(true);
    }

    private void PerformAttack()
    {
        Vector2 shotDirection = (detector.Target.position - shotPoint.position).normalized;

        if (projectilesPerShot == 1)
        {
            ShootProjectile(shotDirection);
        }
        else if (spreadAngle == 0f)
        {
            StartCoroutine(ShootSequentially(shotDirection));
            return;
        }
        else
        {
            float halfSpread = spreadAngle * (projectilesPerShot - 1) / 2f;

            for (int i = 0; i < projectilesPerShot; i++)
            {
                float angle = -halfSpread + (i * spreadAngle);
                Vector2 shootDir = Quaternion.Euler(0, 0, angle) * shotDirection;
                ShootProjectile(shootDir);
            }
        }
    }

    private void ShootProjectile(Vector2 direction)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile prefab not assigned!");
            return;
        }

        Vector2 spawnPosition = (Vector2)shotPoint.position;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.Initialize(
                direction,
                maxProjectileDistance,
                wallLayer,
                projectileDamage,
                projectileSpeed,
                maxBounces
            );
        }
    }

    private IEnumerator ShootSequentially(Vector2 direction)
    {
        for (int i = 0; i < projectilesPerShot; i++)
        {
            ShootProjectile(direction);
            yield return new WaitForSeconds(multiShotDelay);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, shotDistance);
    }
}
