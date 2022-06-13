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
        public float knockback = 10;
        public int damage = 5;
        public GameObject slashParticle;
    }

    [Header("Swing Attack")]
    public SwingWeapon swingWeapon;
    public float swingDelayTime = 0;

    private void Start()
    {
        swingDelayTime = 0;
    }

    public float GetAngle(Vector2 direction)
    {
        float angle = Vector2.Angle(Vector2.up, direction);
        if (direction.x < 0)
        {
            angle = 360 - angle;
        }
        return angle;
    }

    public void Attack()
    {
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

        // add particle
        if (swingWeapon.slashParticle)
        {
            GameObject slash = Instantiate(swingWeapon.slashParticle);
            slash.transform.position = transform.position;
            slash.transform.parent = transform;
            slash.transform.Rotate(0, -swingWeapon.angle / 2 + mouseAngle, 0);
            slash.transform.localScale = Vector3.one * (-1.4f + swingWeapon.range * 0.86f);
            slash.GetComponent<ParticleSystem>().startLifetime = 0.314f / 360 * swingWeapon.angle;

            SwingDamageEnemy script = slash.GetComponent<SwingDamageEnemy>();

            script.SetData(swingWeapon.damage, swingWeapon.angle, mouseAngle, swingWeapon.knockback);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && swingDelayTime <= 0)
        {
            // print("swing");

            Attack();
        }


        if (swingDelayTime > 0)
        {
            swingDelayTime -= Time.deltaTime;
        }
    }
}
