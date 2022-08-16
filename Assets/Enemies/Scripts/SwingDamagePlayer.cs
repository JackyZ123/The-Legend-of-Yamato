using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingDamagePlayer : MonoBehaviour
{
    public int damage = 0;
    public float angle = 90;
    public float firstPlayerAngle = 0;

    public GameObject player;

    private List<GameObject> hasHit = new List<GameObject>();

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void SetData(int newDamage, float newAngle, float newPlayerAngle, float swingTime)
    {
        damage = newDamage;
        angle = newAngle;
        firstPlayerAngle = newPlayerAngle;
        Destroy(GetComponent<Collider2D>(), swingTime);
    }

    public float GetAngle(Vector2 direction)
    {
        float angle = Vector2.Angle(Vector2.up, direction);
        if (direction.x < 0)
        {
            angle = 360 - angle;
        }
        return angle;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (hasHit.Contains(player))
        {
            return;
        }

        hasHit.Add(player);

        Vector2 playerDirection = player.transform.position - transform.position;

        // check in angle
        float playerAngle = GetAngle(playerDirection);

        float angleDifference = (firstPlayerAngle - playerAngle + 360) % 360;

        if (180 - Mathf.Abs(180 - angleDifference) > angle / 2)
        {
            // out of range
            return;
        }

        // deal damage
        if (player.GetComponent<PlayerHealth>())
        {
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
}
