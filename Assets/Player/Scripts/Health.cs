using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int health = 5;

    private int Maxhealth = 5;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            //Damage(1);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            //Heal(1);
        }
    }

    public void Damage(int amount)
    //damage done to health
    {
        if (amount < 0)
        {
            Debug.Log("cannot have negative damage");
        }
        health -= amount;
        Debug.Log(health);
        Debug.Log(amount);
        if (health < 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    //healing done to health
    {
        if (amount < 0)
        {
            Debug.Log("cannot have negative heal");
        }
        if (health + amount > Maxhealth)
        {
            health = Maxhealth;
        }
        else
        {
            health += amount;
        }
    }

    private void Die()
    //if health is 0
    {
        Debug.Log("YOU DIED");
        Destroy(gameObject);
    }
}
