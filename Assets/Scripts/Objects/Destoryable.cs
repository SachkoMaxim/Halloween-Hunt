using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destoryable : MonoBehaviour
{
    [Header("Destroyable Settings")]
    [SerializeField] private int maxHealth = 2;
    [SerializeField] private Sprite newSprite;

    [Header("References")]
    [SerializeField] private GameObject destroyedPrefab;

    private SpriteRenderer spriteRenderer;
    private int currentHealth;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth == 1)
        {
            spriteRenderer.sprite = newSprite;
        }
        else
        {

            if (currentHealth <= 0)
            {
                Instantiate(destroyedPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
