using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackArea : MonoBehaviour
{

    private int damage = 1;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Health>() != null)
        //if enemy is in range of attack, enemy takes damage
        {
            Health health = collider.GetComponent<Health>();
            health.Damage(damage);
            Debug.Log("i've been hit");
        }

    }
}
