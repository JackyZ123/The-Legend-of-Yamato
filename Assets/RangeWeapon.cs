using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : MonoBehaviour
{
    public int minDamage = 5;
    public int maxDamage = 5;
    public float delay = 0.8f;
    public float charge = 1f;
    public float accuracy = 90;
    public float knockback = 10;
    public float projectileSpeed = 5;
    public int pierce = 1;
    public float trailDelay = 1;

    public GameObject projectile;
    public GameObject contactParticle;
    public GameObject trailParticle;

    private float delayTime = 0;
    private float chargeTime = 0;

    private GameObject player;

    private void Start()
    {
        player = transform.root.gameObject;
    }


    public void Attack()
    {
        if (delayTime > 0)
        {
            return;
        }

        chargeTime += Time.deltaTime;
        chargeTime = Mathf.Min(charge, chargeTime);
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

    public void Release()
    {

        if (delayTime > 0)
        {
            chargeTime = 0;
            return;
        }

        if (projectile)
        {
            Vector2 direction = player.GetComponent<PlayerMove>().GetMoveDirection().normalized;

            int newDamage = maxDamage;
            float newKnockback = knockback;
            float newSpeed = projectileSpeed;

            if (charge > 0)
            {
                newDamage = (int)(minDamage + (maxDamage - minDamage) * chargeTime / charge);
                newKnockback = (knockback / 2 + (knockback / 2) * chargeTime / charge);
                newSpeed = (projectileSpeed / 2 + (projectileSpeed / 2) * chargeTime / charge);
            }


            GameObject currentProjectile = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, -GetAngle(direction)));

            if (currentProjectile.GetComponent<PlayerProjectileMove>())
            {
                currentProjectile.GetComponent<PlayerProjectileMove>().SetParameters(newDamage, newKnockback, newSpeed, pierce, contactParticle, trailParticle, trailDelay);
            }

            delayTime = delay;
        }
        else
        {
            Debug.LogWarning("No projectile to spawn");
        }

        chargeTime = 0;
    }

    // Update is called once per frame
    void Update()
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
