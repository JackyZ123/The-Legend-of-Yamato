using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public List<GameObject> weapons;
    private int currentWeaponIndex = 0;

    private void Start()
    {
        foreach (Transform t in transform.GetComponentsInChildren<Transform>())
        {
            if (t == transform)
            {
                continue;
            }

            weapons.Add(t.gameObject);
        }
    }

    public GameObject GetCurrentWeapon()
    {
        return weapons[currentWeaponIndex];
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            GameObject weapon = weapons[currentWeaponIndex];
            weapon.BroadcastMessage("Attack");
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            GameObject weapon = weapons[currentWeaponIndex];
            weapon.BroadcastMessage("Release", SendMessageOptions.DontRequireReceiver);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
            GameObject weapon = weapons[currentWeaponIndex];
            weapon.BroadcastMessage("Delay", new Vector2(0.5f, 0));
        }
    }
}
