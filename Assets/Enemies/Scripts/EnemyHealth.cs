using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 20;
    public int health;


    private void Start()
    {
        health = maxHealth;
    }

    public bool TakeDamage(float damage = 1)
    {
        health -= (int)damage;

        if (health <= 0)
        {
            Destroy(gameObject);
            return true;
        }

        return false;
    }
}
