using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 30f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 input;

    private static readonly int isMoving = Animator.StringToHash("isMoving");
    public static readonly int shoot = Animator.StringToHash("shoot");
    private static readonly int moveX = Animator.StringToHash("moveX");
    private static readonly int moveY = Animator.StringToHash("moveY");

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (input.x != 0) input.y = 0;

        Vector2 movement = input * moveSpeed * Time.fixedDeltaTime;
        rb.velocity = movement;
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (input != Vector2.zero)
        {
            animator.SetFloat(moveX, input.x);
            animator.SetFloat(moveY, input.y);
            animator.SetBool(isMoving, true);
        }
        else
        {
            animator.SetBool(isMoving, false);
        }
    }

    public void Shooting()
    {
        animator.SetTrigger(shoot);
    }

    public bool IsMoving()
    {
        return animator.GetBool(isMoving);
    }
}
