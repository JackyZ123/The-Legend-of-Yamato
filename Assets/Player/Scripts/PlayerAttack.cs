using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [System.Serializable]
    public class SwingWeapon
    {
        public float range = 2.5f;
        public float delay = 0.8f;
        public float angle = 30;
        public int damage = 5;
    }

    [Header("Swing Attack")]
    public SwingWeapon swingWeapon;
    public float swingDelayTime = 0;

    private void Start()
    {
        swingDelayTime = 0;
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && swingDelayTime <= 0)
        {
            // print("swing");

            swingDelayTime = swingWeapon.delay;

            int screenWidth = Screen.width;
            int screenHeight = Screen.height;

            // get mouse position
            Vector2 mouseScreenPosition = Input.mousePosition;
            Vector2 mouseDirection = mouseScreenPosition - new Vector2(screenWidth / 2, screenHeight / 2);
            mouseDirection = mouseDirection.normalized;
            // print(mouseDirection);

            float mouseAngle = GetAngle(mouseDirection);
            // print(mouseAngle);

            // check for each enemy to see if they are in range
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Vector2 enemyDirection = enemy.transform.position - transform.position;

                // check in range
                // assuming enemy has a round collider
                float enemyDistance = enemyDirection.magnitude - enemy.transform.localScale.x;

                if (enemyDistance > swingWeapon.range)
                {
                    continue;
                }

                // check in angle
                float enemyAngle = GetAngle(enemyDirection);
                float angleDifference = (mouseAngle - enemyAngle + 360) % 360;

                if (180 - Mathf.Abs(180 - angleDifference) > swingWeapon.angle / 2)
                {
                    // out of range
                    continue;
                }

                // deal damage
                if (enemy.GetComponent<EnemyHealth>())
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage(swingWeapon.damage);
                }
            }
        }


        if (swingDelayTime > 0)
        {
            swingDelayTime -= Time.deltaTime;
        }
    }
}
