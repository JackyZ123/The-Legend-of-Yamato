using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Motor : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 positionToGo;

    public float speed;

    private void Awake()
    {
        if (!rb)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        positionToGo = transform.position;
    }

    public void Move(float newSpeed, Vector2 direction)
    {
        // moves the gameObject based on direction and speed

        speed = newSpeed;

        positionToGo = (speed + 1) * direction + (Vector2)transform.position;
    }

    public void MoveTo(float newSpeed, Vector2 location, float randomness = 0f, bool avoidObstacles = false)
    {
        // move gameObject to a location

        // randomness is how "floaty" it is
        // avoid obstacles make the object move around obstacle

        speed = newSpeed;

        Vector2 scaledDirection = (location - (Vector2)transform.position) * 1.1f;

        positionToGo = scaledDirection + (Vector2)transform.position;

        positionToGo += new Vector2(Random.Range(-randomness, randomness), Random.Range(-randomness, randomness));
    }

    private void Update()
    {
        Vector2 direction = positionToGo - (Vector2)transform.position;

        if (direction.magnitude > 0.1f)
        {
            rb.velocity = direction.normalized * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
}
