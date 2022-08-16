using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingWeapon : MonoBehaviour
{
    public int damage = 5;
    public float range = 2.5f;
    public float delay = 0.8f;
    public float angle = 30;
    public float knockback = 10;
    public GameObject slashParticle;

    private float delayTime = 0;

    private GameObject player;

    private void Start()
    {
        player = transform.root.gameObject;
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
        if (delayTime > 0)
        {
            return;
        }

        delayTime = delay;

        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        // get mouse position
        // Vector2 mouseScreenPosition = Input.mousePosition;
        // Vector2 mouseDirection = mouseScreenPosition - new Vector2(screenWidth / 2, screenHeight / 2);
        // mouseDirection = mouseDirection.normalized;
        // print(mouseDirection);

        // key direction

        Vector2 mouseDirection = player.GetComponent<PlayerMove>().GetMoveDirection();

        float mouseAngle = GetAngle(mouseDirection);
        // print(mouseAngle);

        // add particle
        if (slashParticle)
        {
            GameObject slash = Instantiate(slashParticle);
            slash.transform.position = transform.position;
            slash.transform.parent = transform;
            slash.transform.Rotate(0, -angle / 2 + mouseAngle, 0);
            slash.transform.localScale = Vector3.one * (-1.4f + range * 0.86f);
            slash.GetComponent<ParticleSystem>().startLifetime = 0.314f / 360 * angle;

            SwingDamageEnemy script = slash.GetComponent<SwingDamageEnemy>();

            script.SetData(damage, angle, mouseAngle, knockback, 0.314f / 360 * angle);
        }
    }

    private void Update()
    {
        if (delayTime > 0)
        {
            delayTime -= Time.deltaTime;
        }
    }

    public void Delay(Vector2 amount)
    {
        if (amount.x > 0)
        {
            delayTime = Mathf.Max(Mathf.Min(amount.x, 100), 0) * delay;
        }
        else
        {
            delayTime = Mathf.Max(Mathf.Min(amount.y, delay), 0);
        }
    }
}
