using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public LevelManager levelManager;

    public float speed = 5f;

    [Header("Boost")]
    public float boost = 2f;
    public float boost_time;
    public float boost_multiplier;
    public float boost_delay = 2f;
    public float boost_delay_time;
    private void Awake()
    {
        if (!rb)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        if (!levelManager)
        {
            levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        }

        boost_time = boost;
        boost_delay_time = boost_delay;
    }

    private void Start()
    {
        Vector3 startPos = levelManager.GetRoom(0).mapPosition;
        startPos.z = -1;

        transform.position = startPos;
    }

    private void Update()
    {
        float vertical_input = Input.GetAxisRaw("Vertical");
        float horizontal_input = Input.GetAxisRaw("Horizontal");

        bool is_boost = Input.GetKey(KeyCode.LeftShift);

        float move_speed = speed;

        animator.SetFloat("vertical speed", vertical_input);
        animator.SetFloat("horizontal speed", horizontal_input);

        if (is_boost && boost_time > 0)
        {
            move_speed *= boost_multiplier;
            boost_time -= Time.deltaTime;
            boost_delay_time = boost_delay;
        }
        else
        {
            if (boost_delay_time <= 0)
            {
                if (boost_time <= boost)
                {
                    boost_time += Time.deltaTime;
                }
            }
            else
            {
                boost_delay_time -= Time.deltaTime;
                // Debug.Log("Test");
            }
        }

        Vector2 moveDirection = new Vector2(horizontal_input, 0).normalized + new Vector2(0, vertical_input).normalized;

        rb.velocity = moveDirection * move_speed;
    }
}
