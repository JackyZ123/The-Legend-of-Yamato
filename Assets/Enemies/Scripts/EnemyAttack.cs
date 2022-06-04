using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [System.Serializable]
    public class SwingWeapon
    {
        public float range = 2.5f;
        public float minDelay = 1.2f;
        public float delayRandomness = 1f;
        public int damage = 1;
    }

    public GameObject player;

    [Header("Swing Attack")]
    public SwingWeapon swingWeapon;
    public float swingDelayTime = 0;

    private void Start()
    {
        swingDelayTime = swingWeapon.minDelay + Random.Range(0, swingWeapon.delayRandomness);

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

    void Swing()
    {
        // print("swing");

        swingDelayTime = swingWeapon.minDelay + Random.Range(0, swingWeapon.delayRandomness);


        float enemyDistance = Vector2.Distance(player.transform.position, transform.position) - player.transform.localScale.x;

        if (enemyDistance > swingWeapon.range)
        {
            return;
        }

        // deal damage
        if (player.GetComponent<PlayerHealth>())
        {
            player.GetComponent<PlayerHealth>().TakeDamage(swingWeapon.damage);
        }
    }

    private void Update()
    {
        if (swingDelayTime <= 0)
        {
            Swing();
        }


        if (swingDelayTime > 0)
        {
            swingDelayTime -= Time.deltaTime;
        }
    }
}
