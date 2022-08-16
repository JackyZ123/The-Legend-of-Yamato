using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerProjectileMove : MonoBehaviour
{
    private Rigidbody2D rb;

    private int damage = 5;
    private float knockback = 10;
    private float speed = 5;
    private int pierce = 1;

    private float trailDelay = 1;

    private GameObject contactParticle;
    private GameObject trailParticle;

    private int startFrame;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = transform.up.normalized * speed;

        startFrame = Time.frameCount;

        if (trailParticle)
        {
            StartCoroutine(TrailSpawner());
        }
    }

    public void SetParameters(int newDamage, float newKnockback, float newSpeed, int newPierce = 1, GameObject newContactParticle = null, GameObject newTrailParticle = null, float newTrailDelay = 1)
    {
        damage = newDamage;
        knockback = newKnockback;
        speed = newSpeed;
        pierce = newPierce;
        contactParticle = newContactParticle;
        trailParticle = newTrailParticle;
        trailDelay = newTrailDelay;
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

    private IEnumerator TrailSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(trailDelay);

            Instantiate(trailParticle, transform.position, Quaternion.Euler(0, 0, -GetAngle(transform.up)));
        }
    }

    private void Kill()
    {
        if (contactParticle)
        {
            GameObject particle = Instantiate(contactParticle, transform.position - transform.up, transform.rotation);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<EnemyHealth>())
            {
                other.GetComponent<EnemyHealth>().TakeDamage(damage, knockback, transform.up);
                pierce--;
            }
        }
        else if (other.CompareTag("Player"))
        {
            if (Time.frameCount <= startFrame + 10)
                return;

            if (other.GetComponent<PlayerHealth>())
            {
                other.GetComponent<PlayerHealth>().TakeDamage(1);
                pierce--;
            }
        }
        else if (other.CompareTag("Wall"))
        {
            if (Time.frameCount <= startFrame + 10)
                return;

            pierce = 0;
        }

        if (pierce <= 0)
        {
            Kill();
        }
    }
}
