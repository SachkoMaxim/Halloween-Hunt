using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private int maxHealth = 1;

    private SpriteRenderer spriteRenderer;
    private int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            DisableEnemy();
            ShowDeadBody();
            FindObjectOfType<GameController>().EnemyDied(this);
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }

    private void DisableEnemy()
    {
        foreach (var comp in GetComponentsInChildren<MonoBehaviour>())
            comp.enabled = false;

        foreach (var rb in GetComponentsInChildren<Rigidbody2D>())
            rb.Sleep();

        foreach (var col in GetComponentsInChildren<Collider2D>())
            col.enabled = false;
    }

    private void ShowDeadBody()
    {
        spriteRenderer.color = Color.gray;
        spriteRenderer.sortingLayerName = "Background";
        spriteRenderer.sortingOrder = 2;
    }

    private IEnumerator FlashRed()
    {
        Color color = spriteRenderer.color;
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        spriteRenderer.color = color;
    }
}
