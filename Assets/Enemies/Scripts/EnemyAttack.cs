using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [System.Serializable]
    public class SwingWeapon
    {
        public float range = 2.5f;
        public float angle = 120;
        public float minSwingDelay = 1.2f;
        public float swingDelayRandomness = 1f;
        public float attackDelay = 1f;
        public int damage = 1;

        public GameObject slashParticle;
    }

    public GameObject player;

    [Header("Swing Attack")]
    public SwingWeapon swingWeapon;
    public float swingDelayTime = 0;

    public GameObject exclamationMark;

    private void Start()
    {
        swingDelayTime = swingWeapon.minSwingDelay + Random.Range(0, swingWeapon.swingDelayRandomness);

        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        StartCoroutine("Swing");
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

    IEnumerator Swing()
    {
        // print("swing");
        yield return new WaitForSeconds(swingWeapon.minSwingDelay + Random.Range(0, swingWeapon.swingDelayRandomness));

        GameObject mark = Instantiate(exclamationMark, transform.position + new Vector3(0.75f, 0.75f, 0), Quaternion.identity);
        mark.transform.parent = transform;

        // wait before attacking
        yield return new WaitForSeconds(Mathf.Max(swingWeapon.attackDelay - 0.2f, 0));

        // check in angle
        float playerAngle = GetAngle(player.transform.position - transform.position);

        yield return new WaitForSeconds(0.2f);

        Destroy(mark);

        GameObject slash = Instantiate(swingWeapon.slashParticle);
        slash.transform.position = transform.position;
        slash.transform.parent = transform;
        slash.transform.Rotate(0, -swingWeapon.angle / 2 + playerAngle, 0);
        slash.transform.localScale = Vector3.one * (-1.4f + swingWeapon.range * 0.86f);
        slash.GetComponent<ParticleSystem>().startLifetime = 0.314f / 360 * swingWeapon.angle;

        SwingDamagePlayer script = slash.GetComponent<SwingDamagePlayer>();

        script.SetData(swingWeapon.damage, swingWeapon.angle, playerAngle, 0.314f / 360 * swingWeapon.angle);

        StartCoroutine("Swing");
    }
}
