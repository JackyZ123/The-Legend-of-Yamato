using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [System.Serializable]
    public class SwingWeapon
    {
        public float range = 2.5f;
        public float angle = 120;
        public float minSwingDelay = 1.2f;
        public float swingDelayRandomness = 1f;
        public float attackDelay = 1f;
        public int damage = 1;
    }

    public GameObject player;

    [Header("Swing Attack")]
    public SwingWeapon swingWeapon;
    public float swingDelayTime = 0;

    private void Start()
    {
        swingDelayTime = swingWeapon.minSwingDelay + Random.Range(0, swingWeapon.swingDelayRandomness);

        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private float GetAngle(Vector2 direction)
    {
        float angle = Vector2.Angle(Vector2.up, direction);
        if (direction.x < 0)
        {
            angle = 360 - angle;
        }
        return angle;
    }

    IEnumerator Swing()
    {
        // print("swing");

        float enemyDistance = Vector2.Distance(player.transform.position, transform.position) - player.transform.localScale.x;

        // check in angle
        float playerAngle = GetAngle(player.transform.position - transform.position);

        // wait before attacking
        yield return new WaitForSeconds(swingWeapon.attackDelay);

        Debug.Log("attack");

        float newPlayerAngle = GetAngle(player.transform.position - transform.position);

        float angleDifference = (newPlayerAngle - playerAngle + 360) % 360;

        if (180 - Mathf.Abs(180 - angleDifference) > swingWeapon.angle / 2)
        {
            // out of range
            yield break;
        }

        if (enemyDistance > swingWeapon.range)
        {
            yield break;
        }

        // deal damage
        if (player.GetComponent<PlayerHealth>())
        {
            player.GetComponent<PlayerHealth>().TakeDamage(swingWeapon.damage);
        }

        StartCoroutine("Swing");
    }
}
