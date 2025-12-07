using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [Header("AI Settings")]
    [SerializeField] private float aiUpdateDelay = 0f;
    [SerializeField] private float detectionDelay = 0f;
    [SerializeField] private float stopDistance = 0f;

    [Header("References")]
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private List<SteeringBehavior> chaseSteeringBehaviors;
    [SerializeField] private List<SteeringBehavior> patrolSteeringBehaviors;
    [SerializeField] private List<Detector> detectors;

    private SteeringController moveController;
    private Vector2 movementInput;
    private bool following = false;

    public override void Start()
    {
        base.Start();
        moveController = GetComponent<SteeringController>();
        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }

    private void PerformDetection()
    {
        foreach (Detector detector in detectors)
        {
            detector.Detect(enemyAI);
        }
    }

    private void Update()
    {
        if (enemyAI.currentTarget != null)
        {
            if (!following)
            {
                following = true;
                StartCoroutine(ChaseAndAttack());
            }
        }
        else if (enemyAI.GetTargetsCount() > 0)
        {
            enemyAI.currentTarget = enemyAI.targets[0];
        }
        else
        {
            StartCoroutine(Patrol());
        }
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            rb.velocity = Vector2.zero;
            IsMoving(false);
            return;
        }

        if (movementInput.sqrMagnitude > 0.01f)
        {
            Vector2 movement = movementInput.normalized * moveSpeed;
            rb.velocity = movement;
            UpdateMovement(movementInput.x, movementInput.y);
            IsMoving(true);
        }
        else
        {
            rb.velocity = Vector2.zero;
            IsMoving(false);
        }
    }

    private IEnumerator ChaseAndAttack()
    {
        while (enemyAI.currentTarget != null)
        {
            float distance = Vector2.Distance(enemyAI.currentTarget.position, transform.position);

            if (distance < stopDistance)
            {
                movementInput = Vector2.zero;
                yield return new WaitForSeconds(aiUpdateDelay);
            }
            else
            {
                if (moveController != null)
                {
                    movementInput = moveController.GetDirectionToMove(chaseSteeringBehaviors, enemyAI);
                }
                yield return new WaitForSeconds(aiUpdateDelay);
            }
        }

        movementInput = Vector2.zero;
        following = false;
    }

    private IEnumerator Patrol()
    {
        if (moveController != null)
        {
            movementInput = moveController.GetDirectionToMove(patrolSteeringBehaviors, enemyAI);
        }

        yield return new WaitForSeconds(aiUpdateDelay);
    }

    protected override IEnumerator Die(float time)
    {
        sprRend.sortingLayerName = "Background";
        sprRend.sortingOrder = 2;
        yield return StartCoroutine(base.Die(time));
        FindObjectOfType<GameController>().EnemyDied(this);
    }
}
