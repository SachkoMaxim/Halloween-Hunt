using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShoot : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shootCooldown = 0f;

    [Header("References")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private PlayerController playerController;
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
        onCooldown.Invoke((shootCooldown - Time.time + lastShootTime) / shootCooldown);
        bool isPlayerMoving = playerRigidbody != null && playerRigidbody.velocity.sqrMagnitude > 0.01f;

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

        playerController.IsShooting();
        Vector2 spawnPosition = (Vector2)shootPoint.position + trajectory.shootDirection * trajectory.startOffset;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.Initialize(trajectory.shootDirection, trajectory.maxBulletDistance, trajectory.wallLayer);
        }

        lastShootTime = Time.time;
    }
}
