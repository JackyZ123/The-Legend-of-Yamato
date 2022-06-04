using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 20;
    public int health;

    public Rigidbody2D rb;


    private void Start()
    {
        health = maxHealth;

        if (!rb)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    public bool TakeDamage(float damage, float knockback, Vector2 knockbackDirection)
    {
        health -= (int)damage;

        if (health <= 0)
        {
            Destroy(gameObject);
            return true;
        }

        knockbackDirection = knockbackDirection.normalized;

        rb.AddForce(knockbackDirection * knockback * 100);

        return false;
    }
}
