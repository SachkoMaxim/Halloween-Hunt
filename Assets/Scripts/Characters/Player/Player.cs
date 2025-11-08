using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private int maxHealth = 1;
    [SerializeField] public float moveSpeed = 0f;

    [Header("References")]
    [SerializeField] public Transform shootPoint;

    [HideInInspector] public Rigidbody2D rb;
    private Animator animator;
    private int currentHealth;

    private static readonly int isMoving = Animator.StringToHash("isMoving");
    private static readonly int shoot = Animator.StringToHash("shoot");
    private static readonly int moveX = Animator.StringToHash("moveX");
    private static readonly int moveY = Animator.StringToHash("moveY");

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    public void UpdateMovement(float inputX, float inputY)
    {
        animator.SetFloat(moveX, inputX);
        animator.SetFloat(moveY, inputY);
    }

    public void IsMoving(bool meaning)
    {
        animator.SetBool(isMoving, meaning);
    }

    public void Shooting()
    {
        animator.SetTrigger(shoot);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Debug.Log("Player took damage");
        }
    }

    public bool GetIsMoving()
    {
        return animator.GetBool(isMoving);
    }
}
