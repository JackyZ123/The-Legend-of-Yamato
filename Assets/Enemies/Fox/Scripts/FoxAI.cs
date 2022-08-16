using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyHealth))]
public class FoxAI : MonoBehaviour
{

    public float damage = 1;
    public float speed = 10;
    public float timeBetweenAttacks = 3;
    private float latestTime;
    private float directionChangeTime = 3f;

    private Rigidbody2D rb;

    private EnemyHealth health;

    private Vector2 movementDirection;

    private Vector2 movementTime;

    // Start is called before the first frame update
    void Start()
    {
        latestTime = 0f;
        newMovementDirection();
    }

    void newMovementDirection()
    {
        movementDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        movementTime = movementDirection * speed;
    }
    // direction to player = player position - my position

    IEnumerator wait()
    {
        speed = 0;
        print("wait");
        transform.position = new Vector3(transform.position.x + 0, transform.position.y + 0, -1);
        yield return new WaitForSeconds(2);
        newMovementDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - latestTime > directionChangeTime)
        {
            latestTime = Time.time;
            //enemy move
            StartCoroutine(wait());
        }
        transform.position = new Vector3(transform.position.x + (movementTime.x * Time.deltaTime),
        transform.position.y + (movementTime.y * Time.deltaTime), -1);

    }
}
