using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEnemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyHitboxLayer;
    [SerializeField] private Projectile projectile;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & enemyHitboxLayer) != 0)
        {
            Enemy enemy = collision.transform.parent.GetComponent<Enemy>();
            enemy.TakeDamage(projectile.GetDamage());
            Destroy(gameObject);
        }
    }
}
