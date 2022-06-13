using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameObject healthBar;

    public int health;

    public int max_health = 5;

    public float damageInvulerability = 1.5f;
    public float damageInvulerabilityTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        health = max_health;

        UpdateHealthBar();
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
            print("died");
            health = 0;
            UpdateHealthBar();
            return true;
        }

        UpdateHealthBar();

        return false;
    }


    private void UpdateHealthBar()
    {
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(20 * health, 20);
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
