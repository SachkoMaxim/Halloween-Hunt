using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public abstract class Character : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] protected int maxHealth = 1;
    [SerializeField] protected float moveSpeed = 0f;
    [SerializeField] protected float deathTime = 0f;

    [Header("Audio Clips")]
    [SerializeField] public AudioClip attackClip;
    [SerializeField] protected AudioClip damagedClip;
    [SerializeField] protected AudioClip deathClip;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer sprRend;
    protected int currentHealth;
    protected bool canMove;

    protected static readonly int moveX = Animator.StringToHash("moveX");
    protected static readonly int moveY = Animator.StringToHash("moveY");
    protected static readonly int isMoving = Animator.StringToHash("isMoving");
    protected static readonly int attack = Animator.StringToHash("attack");
    protected static readonly int dead = Animator.StringToHash("dead");
    protected static readonly int flashRed = Animator.StringToHash("flashRed");

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprRend = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        canMove = true;
    }

    public virtual void Start()
    {

    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            OnDeath(deathTime);
        }
        else
        {
            OnDamaged();
        }
    }

    public virtual void OnDeath(float time)
    {
        StartCoroutine(Die(time));
    }

    public virtual void OnDamaged()
    {
        AudioManager.instance.PlaySFXClip(damagedClip, transform);
        animator.SetTrigger(flashRed);
    }

    protected void DisableCharacter()
    {
        foreach (var comp in GetComponentsInChildren<MonoBehaviour>())
            comp.enabled = false;

        foreach (var rb in GetComponentsInChildren<Rigidbody2D>())
            rb.Sleep();

        foreach (var col in GetComponentsInChildren<Collider2D>())
            col.enabled = false;

        SpriteResolver spriteResolver = GetComponent<SpriteResolver>();
        if (spriteResolver != null)
        {
            spriteResolver.enabled = true;
        }
    }

    public virtual void UpdateMovement(float x, float y)
    {
        animator.SetFloat(moveX, x);
        animator.SetFloat(moveY, y);
    }

    public virtual void IsMoving(bool m)
    {
        animator.SetBool(isMoving, m);
    }

    public virtual void CanMove(bool m)
    {
        canMove = m;
    }

    public virtual void Attacking()
    {
        animator.SetTrigger(attack);
    }

    public virtual bool GetIsMoving()
    {
        return animator.GetBool(isMoving);
    }

    public virtual float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public virtual bool GetCanMove()
    {
        return canMove;
    }

    public virtual Rigidbody2D GetRigidbody()
    {
        return rb;
    }

    protected virtual IEnumerator Die(float time)
    {
        DisableCharacter();
        AudioManager.instance.PlaySFXClip(deathClip, transform);
        animator.SetTrigger(dead);

        yield return new WaitForSeconds(time);
    }
}
