using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(LevelManager))]
public class LevelLoader : MonoBehaviour
{
    public LevelManager manager;

    public LevelManager.Room currentRoom;

    [Header("Debug")]
    public GameObject arrowPointer;

    private void Awake()
    {
        if (!manager)
        {
            manager = GetComponent<LevelManager>();
        }
    }

    public LevelManager.Room GetCurrentRoom()
    {
        return currentRoom;
    }

    public void GetRoomExits()
    {
        foreach (string arrowName in new string[] { "Up", "Right", "Down", "Left" })
        {
            if (GameObject.Find("Arrow " + arrowName))
            {
                GameObject.DestroyImmediate(GameObject.Find("Arrow " + arrowName));
            }
        }

        // check for connection upwards
        if (currentRoom.up > -1)
        {
            GameObject arrow = Instantiate(arrowPointer);
            arrow.transform.position = Vector2.up * 9;
            arrow.name = "Arrow Up";
        }
        if (currentRoom.right > -1)
        {

            GameObject arrow = Instantiate(arrowPointer);
            arrow.transform.position = Vector2.right * 9;
            arrow.transform.Rotate(new Vector3(0, 0, -90));
            arrow.name = "Arrow Right";
        }
        if (currentRoom.down > -1)
        {
            GameObject arrow = Instantiate(arrowPointer);
            arrow.transform.position = Vector2.down * 9;
            arrow.transform.Rotate(new Vector3(0, 0, 180));
            arrow.name = "Arrow Down";
        }
        if (currentRoom.left > -1)
        {
            GameObject arrow = Instantiate(arrowPointer);
            arrow.transform.position = Vector2.left * 9;
            arrow.transform.Rotate(new Vector3(0, 0, 90));
            arrow.name = "Arrow Left";
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // we move up if possible
            if (currentRoom.up > -1)
            {
                // move up
                currentRoom = manager.GetRoom(currentRoom.up);
                manager.DebugLevel();
            }
            else
            {
                Debug.Log("Can't go up");
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // we move right if possible
            if (currentRoom.right > -1)
            {
                // move right
                currentRoom = manager.GetRoom(currentRoom.right);
                manager.DebugLevel();
            }
            else
            {
                Debug.Log("Can't go right");
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // we move down if possible
            if (currentRoom.down > -1)
            {
                // move down
                currentRoom = manager.GetRoom(currentRoom.down);
                manager.DebugLevel();
            }
            else
            {
                Debug.Log("Can't go up");
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // we move up if possible
            if (currentRoom.left > -1)
            {
                // move left
                currentRoom = manager.GetRoom(currentRoom.left);
                manager.DebugLevel();
            }
            else
            {
                Debug.Log("Can't go left");
            }
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            manager.GenerateNewMap();
            currentRoom = manager.GetRoom(0);
            manager.DebugLevel();
        }
    }


    private void Start()
    {
        currentRoom = manager.GetRoom(0);
        manager.DebugLevel();
    }
}
