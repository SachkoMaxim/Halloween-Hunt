using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Music Settings")]
    [SerializeField] public AudioClip stepsClip;
    [SerializeField] public float stepsSpeed = 0f;

    [Header("References")]
    [SerializeField] private Player player;

    private Vector2 input;
    private bool playingSound = false;

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (input.x != 0) input.y = 0;

        Vector2 movement = input * player.GetMoveSpeed();
        player.GetRigidbody().velocity = movement;
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (input != Vector2.zero)
        {
            player.UpdateMovement(input.x, input.y);
            player.IsMoving(true);
            if (!playingSound)
            {
                playingSound = true;
                InvokeRepeating(nameof(PlayFootSteps), 0f, stepsSpeed);
            }
        }
        else
        {
            player.IsMoving(false);
            playingSound = false;
            CancelInvoke(nameof(PlayFootSteps));
        }
    }

    private void PlayFootSteps()
    {
        AudioManager.instance.PlaySFX(stepsClip);
    }
}
