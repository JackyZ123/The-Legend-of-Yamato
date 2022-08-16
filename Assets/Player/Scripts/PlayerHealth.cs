using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health;

    public int max_health = 5;

    public float damageInvulerability = 1.5f;
    public float damageInvulerabilityTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        health = max_health;
    }

    public bool TakeDamage(float damage = 1)
    {
        if (damageInvulerabilityTime > 0)
        {
            return false;
        }

        damageInvulerabilityTime = damageInvulerability;

        health -= (int)damage;

        if (health <= 0)
        {
            SceneManager.LoadScene("Death");
            return true;
        }

        return false;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }

    private void Update()
    {
        if (damageInvulerabilityTime > 0)
        {
            damageInvulerabilityTime -= Time.deltaTime;
        }
    }
}
