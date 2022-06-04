using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceBasedMove : MonoBehaviour
{
    public Motor motor;

    public GameObject player;

    public float speed = 5f;
    public float randomness = 0f;
    public float preferredDistance = 2f;
    public float preferredDistanceRange = 0.5f;

    public float timeBetweenMove = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if (!motor)
        {
            motor = GetComponent<Motor>();
        }

        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        StartCoroutine("CheckMove");
    }

    void Move()
    {
        // check distance to player
        Vector2 vectorToPlayer = player.transform.position - transform.position;

        // direction to travel in is direction to player if too far otherwise away from player
        float distanceToPlayer = vectorToPlayer.magnitude;

        Vector2 positionToGo = transform.position;

        if (Mathf.Abs(distanceToPlayer - preferredDistance) < preferredDistanceRange)
        {
            // its at the correct distance from the player

            // move to the side randomly - tangent to circle around player

            // print("just right");

            motor.MoveTo(speed, transform.position, 0);
        }
        else
        {
            // move towards preferred distance

            if (distanceToPlayer > preferredDistance)
            {
                // we are too far

                // print("too far");

                positionToGo += vectorToPlayer.normalized * Mathf.Min(speed / timeBetweenMove, distanceToPlayer - preferredDistance);
            }

            else
            {
                // print("too close");

                positionToGo -= vectorToPlayer.normalized * Mathf.Min(speed / timeBetweenMove, preferredDistance - distanceToPlayer);
            }


            motor.MoveTo(speed, positionToGo, randomness);
        }

    }

    IEnumerator CheckMove()
    {
        Move();

        yield return new WaitForSeconds(timeBetweenMove);

        StartCoroutine("CheckMove");
    }
}
