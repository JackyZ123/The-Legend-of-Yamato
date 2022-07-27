using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyHealth))]
public class SamuraiAI : MonoBehaviour
{
    public float damage = 1;
    public float speed = 10;
    public float timeBetweenAttacks = 3;
    public float difficulty = 1;
    public float weaponAngle = 120;
    public float weaponRange = 5;

    private Rigidbody2D rb;

    public GameObject exclamationMark;

    public GameObject slashParticle;

    public GameObject player;

    private EnemyHealth health;

    public Vector3 targetPos;

    private void Start() {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<EnemyHealth>();

        StartCoroutine(Attack());
        StartCoroutine(Move());
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

    private IEnumerator Attack(){

        while (true){
            yield return new WaitForSeconds(timeBetweenAttacks + Random.Range(0,timeBetweenAttacks/5));


            // ---------- give players a chance to run away ---------- //
            GameObject mark = Instantiate(exclamationMark, transform.position + new Vector3(0.75f, 0.75f, 0), Quaternion.identity);
            mark.transform.parent = transform;

            // choose angle for attack
            float playerAngle = GetAngle(player.transform.position - transform.position);

            yield return new WaitForSeconds(0.7f);

            Destroy(mark);

            // ---------- create slash object ---------- //

            GameObject slash = Instantiate(slashParticle);
            slash.transform.position = transform.position;
            slash.transform.parent = transform;
            slash.transform.Rotate(0, - weaponAngle / 2 + playerAngle, 0);
            slash.transform.localScale = Vector3.one * (-1.4f + weaponRange * 0.86f);
            slash.GetComponent<ParticleSystem>().startLifetime = 0.314f / 360 * weaponAngle;

            SwingDamagePlayer script = slash.GetComponent<SwingDamagePlayer>();

            script.SetData((int)damage, weaponAngle, playerAngle, 0.314f / 360 * weaponAngle);
        }
    }

    private IEnumerator Move(){
        while (true){
            // Debug.Log(Vector3.Distance(transform.position, targetPos));

            // // decide for pause
            // if (Random.Range(1, 3) == 1){
            //     rb.velocity = Vector3.zero;
            //     yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            // }

            // // choose new pos

            // // make 5 raycasts in random directions
            // // higher chance to move closer to player

            // targetPos = player.transform.position;
            // // move closer

            // while( Vector3.Distance(transform.position, targetPos) > 1){
            rb.AddForce((player.transform.position - transform.position).normalized * speed * 2* Time.deltaTime, ForceMode2D.Impulse);

            // rb.velocity = (targetPos - transform.position).normalized * speed; 
            yield return new WaitForEndOfFrame();
            // }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.CompareTag("Wall")){
            health.TakeDamage(rb.velocity.magnitude * rb.velocity.magnitude / 100, 0, Vector2.zero);
        }
    }
}
