using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackArea : MonoBehaviour
{

    private int damage = 5;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Health>()! = null)
        //if enemy is in range of attack, enemy takes damage
        {
            Health health = collider.GetComponent<Health>();
            Health.Damage(damage);
        }

    }
}
