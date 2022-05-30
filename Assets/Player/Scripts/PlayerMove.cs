using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Motor))]
public class PlayerMove : MonoBehaviour
{
    public Motor motor;
    public LevelManager levelManager;

    public float speed = 5f;

    private void Awake()
    {
        if (!motor)
        {
            motor = GetComponent<Motor>();
        }
        if (!levelManager)
        {
            levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        }
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

        motor.Move(speed, new Vector2(horizontal_input, 0).normalized + new Vector2(0, vertical_input).normalized);
    }
}
