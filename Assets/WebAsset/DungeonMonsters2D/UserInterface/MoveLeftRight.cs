using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftRight : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = 10f;
    }
    public void MoveRight()
    {
        rb.velocity = Vector2.right * moveSpeed;
    }
    public void MoveLeft()
    {
        rb.velocity = Vector2.left * moveSpeed;
    }
    public void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }
}