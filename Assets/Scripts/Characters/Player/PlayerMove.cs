using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player player;

    private Vector2 input;

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (input.x != 0) input.y = 0;

        Vector2 movement = input * player.moveSpeed * Time.fixedDeltaTime;
        player.rb.velocity = movement;
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (input != Vector2.zero)
        {
            player.UpdateMovement(input.x, input.y);
            player.IsMoving(true);
        }
        else
        {
            player.IsMoving(false);
        }
    }
}
