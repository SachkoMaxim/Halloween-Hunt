using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShoot : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private float shootCooldown = 0f;

    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 0f;
    [SerializeField] private int projectileDamage = 0;

    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private Trajectory trajectory;

    [Header("Cooldown")]
    public UnityEvent<float> onCooldown;

    private float lastShootTime = -999f;

    void Start()
    {
        onCooldown.Invoke(0);
    }

    void Update()
    {
        if (InputBlocker.Blocked) return;

        onCooldown.Invoke((shootCooldown - Time.time + lastShootTime) / shootCooldown);
        bool isPlayerMoving = player.GetIsMoving();

        if (!isPlayerMoving && Input.GetKeyDown(KeyCode.E))
        {
            TryShoot();
        }
    }

    private void TryShoot()
    {
        if (Time.time - lastShootTime < shootCooldown)
        {
            return;
        }

        if (projectilePrefab == null)
        {
            Debug.LogWarning("Projectile prefab is not assigned!");
            return;
        }

        player.Shooting();
        Vector2 spawnPosition = (Vector2)player.shootPoint.position + trajectory.shootDirection * trajectory.startOffset;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.Initialize(
                trajectory.shootDirection,
                trajectory.maxBulletDistance,
                trajectory.wallLayer,
                projectileDamage,
                projectileSpeed,
                0
            );
        }

        lastShootTime = Time.time;
    }
}
