using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Motor : MonoBehaviour
{
    public Rigidbody2D rb;
    Vector2 velocity = Vector2.zero;

    private void Awake()
    {
        if (!rb)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    public void Move(float speed, Vector2 direction)
    {
        // moves the gameObject based on direction and speed

        velocity = speed * direction;
        rb.velocity = velocity;
    }

    public void MoveTo(float speed, Vector2 location, float randomness = 0f, bool avoidObstacles = false)
    {
        // move gameObject to a location

        // randomness is how "floaty" it is
        // avoid obstacles make the object move around obstacle
    }
}
