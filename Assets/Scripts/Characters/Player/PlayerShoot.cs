using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShoot : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private float shootCooldown = 0f;
    [SerializeField] private float startOffset = 0f;

    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 0f;
    [SerializeField] private float maxProjectileDistance = 0f;
    [SerializeField] private int projectileDamage = 0;

    [Header("References")]
    [SerializeField] protected LayerMask wallLayer;
    [SerializeField] private Player player;

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

        player.Attacking();
        Vector2 spawnPosition = (Vector2)player.shootPoint.position + player.attackDirection * startOffset;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.Initialize(
                player.attackDirection,
                maxProjectileDistance,
                wallLayer,
                projectileDamage,
                projectileSpeed,
                0
            );
        }

        lastShootTime = Time.time;
    }
}
