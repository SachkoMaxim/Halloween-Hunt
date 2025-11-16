using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlayer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask playerHitboxLayer;
    [SerializeField] private Projectile projectile;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerHitboxLayer) != 0)
        {
            Player player = collision.transform.parent.GetComponent<Player>();
            player.TakeDamage(projectile.GetDamage());
            Destroy(gameObject);
        }
    }
}
