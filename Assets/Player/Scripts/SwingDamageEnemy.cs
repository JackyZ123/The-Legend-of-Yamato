using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingDamageEnemy : MonoBehaviour
{
    public int damage = 0;
    public float knockback = 0;
    public float angle = 90;
    public float mouseAngle = 0;

    public GameObject player;
    public int width;
    public int height;

    private List<GameObject> hasHit = new List<GameObject>();

    private void Start()
    {
        width = Screen.width;
        height = Screen.height;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void SetData(int newDamage, float newAngle, float newMouseAngle, float newKnockback = 0, float swingTime = 0)
    {
        damage = newDamage;
        angle = newAngle;
        mouseAngle = newMouseAngle;
        knockback = newKnockback;
        Destroy(GetComponent<Collider2D>(), swingTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
        {
            return;
        }

        GameObject enemy = other.gameObject;

        if (hasHit.Contains(enemy))
        {
            return;
        }

        hasHit.Add(enemy);

        Vector2 enemyDirection = enemy.transform.position - transform.position;

        // check in angle
        float enemyAngle = player.GetComponent<PlayerAttack>().GetAngle(enemyDirection);
        float angleDifference = (mouseAngle - enemyAngle + 360) % 360;

        if (180 - Mathf.Abs(180 - angleDifference) > angle / 2)
        {
            // out of range
            return;
        }

        // deal damage
        if (enemy.GetComponent<EnemyHealth>())
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(damage, knockback, enemyDirection);
        }
    }
}
