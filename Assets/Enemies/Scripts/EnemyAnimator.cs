using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAnimator : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        if (!animator){
            animator = GetComponent<Animator>();
        }
        if (!rb){
            rb = GetComponent<Rigidbody2D>();
        }
    }

    int getSign(float x){
        if (x == 0){
            return 0;
        }

        return (int)(x / Mathf.Abs(x));
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(rb.velocity.y) > 2*Mathf.Abs(rb.velocity.x)){
            animator.SetFloat("vertical speed", getSign(rb.velocity.y));
            animator.SetFloat("horizontal speed", 0);
        }
        else{
            animator.SetFloat("vertical speed", 0);
            animator.SetFloat("horizontal speed", getSign(rb.velocity.x));
        }
    }
}
