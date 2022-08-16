using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Animator animator;

    public Rigidbody2D rb;
    public LevelManager levelManager;
    public GameObject staminaBar;

    public float speed = 5f;

    [Header("Boost")]
    public float boost = 2f;
    public float boostTime;
    public float boostMultiplier;
    public float boostDelay = 2f;
    public float boostDelayTime;

    public float inputDelay = 0;

    public Vector2 moveDirection = Vector2.zero;
    public Vector2 lookDirection = Vector2.up;

    private void Awake()
    {
        if (!rb)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        if (!levelManager)
        {
            levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        }

        boostTime = boost;
        boostDelayTime = boostDelay;
    }

    private void Start()
    {
        Vector3 startPos = levelManager.GetRoom(0).mapPosition;
        startPos.z = -1;

        transform.position = startPos;
    }

    public Vector2 GetMoveDirection()
    {
        return lookDirection;
    }

    void GetMoveInput()
    {
        float newVerticalInput = Input.GetAxisRaw("Vertical");
        float newHorizontalInput = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("vertical speed", newVerticalInput);
        animator.SetFloat("horizontal speed", newHorizontalInput);

        // check for diagonal
        // if let go of a key, in the next 0.05s, if the other key is let go, dont record
        if (moveDirection.x != 0 && newHorizontalInput == 0 || moveDirection.y != 0 && newVerticalInput == 0)
        {
            // we just let go of a key
            inputDelay = 0.05f;
        }

        if (inputDelay > 0)
        {
            // we have just let go of a key
            inputDelay -= Time.deltaTime;

            // we should not care about zeros but take others
            lookDirection.x = newHorizontalInput == 0 ? lookDirection.x : newHorizontalInput;
            lookDirection.y = newVerticalInput == 0 ? lookDirection.y : newVerticalInput;
        }
        else
        {
            // we want the new input if its not (0,0)
            lookDirection.x = newHorizontalInput == 0 && newVerticalInput == 0 ? lookDirection.x : newHorizontalInput;
            lookDirection.y = newHorizontalInput == 0 && newVerticalInput == 0 ? lookDirection.y : newVerticalInput;
        }

        moveDirection = new Vector2(newHorizontalInput, 0).normalized + new Vector2(0, newVerticalInput).normalized;
    }

    private void Update()
    {
        GetMoveInput();

        bool is_boost = Input.GetKey(KeyCode.LeftShift);

        float move_speed = speed;

        if (is_boost && boostTime > 0)
        {
            move_speed *= boostMultiplier;
            boostTime -= Time.deltaTime;
            boostDelayTime = boostDelay;

            UpdateStaminaBar();
        }
        else
        {
            if (boostDelayTime <= 0)
            {
                if (boostTime <= boost)
                {
                    boostTime += Time.deltaTime;

                    boostTime = Mathf.Min(boostTime, boost);

                    UpdateStaminaBar();
                }
            }
            else
            {
                boostDelayTime -= Time.deltaTime;
                // Debug.Log("Test");
            }
        }

        rb.velocity = moveDirection * move_speed;
    }

    void UpdateStaminaBar()
    {
        staminaBar.GetComponent<RectTransform>().sizeDelta = new Vector2(boostTime / boost * 98, 13);
    }
}
