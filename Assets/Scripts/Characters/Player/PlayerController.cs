using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    private Vector2 input;

    public float moveSpead = 100;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (input.x != 0) input.y = 0;
        rb.velocity = new Vector2(input.x * moveSpead * Time.deltaTime, input.y * moveSpead * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }
}
