using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileAttack : EnemyAttack
{
    [Header("Projectile Attack Settings")]
    [SerializeField] private int projectilesPerShot = 1;
    [SerializeField] private float spreadAngle = 0f;
    [SerializeField] private float multiShotDelay = 0.1f;

    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 0f;
    [SerializeField] private float maxProjectileDistance = 0f;
    [SerializeField] private int maxBounces = 0;

    private Vector2 currentShotDirection;

    protected override IEnumerator PrepareAndAttack()
    {
        currentShotDirection = (enemyAI.currentTarget.position - attackPoint.position).normalized;
        enemy.UpdateMovement(currentShotDirection.x, currentShotDirection.y);

        yield return StartCoroutine(base.PrepareAndAttack());
    }

    protected override void PerformAttack()
    {
        Vector2 shotDirection = currentShotDirection;

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

        Vector2 spawnPosition = (Vector2)attackPoint.position;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.Initialize(
                direction,
                maxProjectileDistance,
                wallLayer,
                attackDamage,
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
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
