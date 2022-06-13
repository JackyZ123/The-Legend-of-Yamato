using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelManager))]
public class LevelLoader : MonoBehaviour
{
    [System.Serializable]
    public class GameObjectList
    {
        [SerializeField]
        public List<GameObject> list;
    }

    [System.Serializable]
    public class GameObjectNestedList
    {
        [SerializeField]
        public List<GameObjectList> list;
    }


    public LevelManager manager;

    public LevelManager.Room currentRoom;

    [Header("Map Details")]
    public float roomScale = 30;
    public float roomSpread = 50;


    [Header("Room Details")]
    public GameObjectNestedList roomBackgrounds;
    public List<GameObject> roomWalls;
    public List<GameObject> pathBackgrounds;
    public GameObject pathWall;


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

    public GameObject GetRoomBackground(int type)
    {
        List<GameObject> backgrounds = roomBackgrounds.list[type].list;
        return backgrounds[Random.Range(0, backgrounds.Count)];
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
            arrow.transform.position += Vector3.back;
            arrow.name = "Arrow Up";
        }
        if (currentRoom.right > -1)
        {

            GameObject arrow = Instantiate(arrowPointer);
            arrow.transform.position = Vector2.right * 9;
            arrow.transform.position += Vector3.back;
            arrow.transform.Rotate(new Vector3(0, 0, -90));
            arrow.name = "Arrow Right";
        }
        if (currentRoom.down > -1)
        {
            GameObject arrow = Instantiate(arrowPointer);
            arrow.transform.position = Vector2.down * 9;
            arrow.transform.position += Vector3.back;
            arrow.transform.Rotate(new Vector3(0, 0, 180));
            arrow.name = "Arrow Down";
        }
        if (currentRoom.left > -1)
        {
            GameObject arrow = Instantiate(arrowPointer);
            arrow.transform.position = Vector2.left * 9;
            arrow.transform.position += Vector3.back;
            arrow.transform.Rotate(new Vector3(0, 0, 90));
            arrow.name = "Arrow Left";
        }
    }


    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.UpArrow))
        // {
        //     // we move up if possible
        //     if (currentRoom.up > -1)
        //     {
        //         // move up
        //         currentRoom = manager.GetRoom(currentRoom.up);
        //         manager.DebugLevel();
        //     }
        //     else
        //     {
        //         Debug.Log("Can't go up");
        //     }
        // }
        // if (Input.GetKeyDown(KeyCode.RightArrow))
        // {
        //     // we move right if possible
        //     if (currentRoom.right > -1)
        //     {
        //         // move right
        //         currentRoom = manager.GetRoom(currentRoom.right);
        //         manager.DebugLevel();
        //     }
        //     else
        //     {
        //         Debug.Log("Can't go right");
        //     }
        // }
        // if (Input.GetKeyDown(KeyCode.DownArrow))
        // {
        //     // we move down if possible
        //     if (currentRoom.down > -1)
        //     {
        //         // move down
        //         currentRoom = manager.GetRoom(currentRoom.down);
        //         manager.DebugLevel();
        //     }
        //     else
        //     {
        //         Debug.Log("Can't go up");
        //     }
        // }
        // if (Input.GetKeyDown(KeyCode.LeftArrow))
        // {
        //     // we move up if possible
        //     if (currentRoom.left > -1)
        //     {
        //         // move left
        //         currentRoom = manager.GetRoom(currentRoom.left);
        //         manager.DebugLevel();
        //     }
        //     else
        //     {
        //         Debug.Log("Can't go left");
        //     }
        // }
        // if (Input.GetKeyDown(KeyCode.N))
        // {
        //     manager.GenerateNewMap();
        //     currentRoom = manager.GetRoom(0);
        //     manager.DebugLevel();
        // }
    }

    void LoadRoom(LevelManager.Room room, GameObject parent)
    {
        GameObject background = Instantiate(room.data.background);
        background.transform.parent = parent.transform;
        background.transform.localPosition = Vector3.zero;
        background.name = "Background";

        if (room.data.type == 1)
        {
            // its a small room
            GameObject walls = Instantiate(roomWalls[1]);
            walls.transform.parent = parent.transform;
            walls.transform.localPosition = Vector3.zero;
            walls.name = "Walls";

            if (room.up == -1)
            {
                // small room so remove little bit hanging
                Destroy(background.transform.Find("Up").gameObject);
            }
            else
            {
                walls.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            }

            if (room.right == -1)
            {
                // small room so remove little bit hanging
                Destroy(background.transform.Find("Right").gameObject);
            }
            else
            {
                walls.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            }

            if (room.down == -1)
            {
                // small room so remove little bit hanging
                Destroy(background.transform.Find("Down").gameObject);
            }
            else
            {
                walls.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
            }

            if (room.left == -1)
            {
                // small room so remove little bit hanging
                Destroy(background.transform.Find("Left").gameObject);
            }
            else
            {
                walls.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
            }

        }
        else
        {
            // its a big room
            GameObject walls = Instantiate(roomWalls[0]);
            walls.transform.parent = parent.transform;
            walls.transform.localPosition = Vector3.zero;
            walls.name = "Walls";

            if (room.up != -1)
            {
                walls.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            }
            if (room.right != -1)
            {
                walls.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            }
            if (room.down != -1)
            {
                walls.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
            }
            if (room.left != -1)
            {
                walls.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
            }
        }

        parent.transform.localScale = new Vector3(1, 1, 0) * roomScale + Vector3.forward;
    }

    void MakePath(LevelManager.Room room, GameObject background, GameObject directory, Vector3 position, Vector3 scale, bool is_vertical)
    {
        GameObject path = Instantiate(background);
        path.transform.position = position;
        path.transform.localScale = scale;
        path.transform.parent = directory.transform;
        path.name = "Path (" + room.up.ToString() + ", " + room.index.ToString() + ")";

        GameObject wall = Instantiate(pathWall);
        wall.transform.position = position;
        wall.transform.localScale = scale;
        wall.transform.GetChild(is_vertical ? 1 : 0).gameObject.SetActive(false);
        wall.transform.parent = directory.transform;
    }

    void LoadPaths(LevelManager.Room room, GameObject parent, GameObject directory)
    {
        if (parent.name.Substring(0, 4) != "Room")
        {
            Debug.LogWarning("Room not Found!");
        }

        if (room.up > -1 && room.index > room.up)
        {
            // make path upwards
            GameObject background = pathBackgrounds[Random.Range(0, pathBackgrounds.Count)];
            Vector3 position = parent.transform.position + new Vector3(0, roomSpread / 2);
            Vector3 scale = new Vector3(roomScale / 5, roomSpread - roomScale);

            MakePath(room, background, directory, position, scale, true);
        }
        if (room.right > -1 && room.index > room.right)
        {
            // make path to the right
            GameObject background = pathBackgrounds[Random.Range(0, pathBackgrounds.Count)];
            Vector3 position = parent.transform.position + new Vector3(roomSpread / 2, 0);
            Vector3 scale = new Vector3(roomSpread - roomScale, roomScale / 5);

            MakePath(room, background, directory, position, scale, false);
        }
        if (room.down > -1 && room.index > room.down)
        {
            // make path downwards
            GameObject background = pathBackgrounds[Random.Range(0, pathBackgrounds.Count)];
            Vector3 position = parent.transform.position - new Vector3(0, roomSpread / 2);
            Vector3 scale = new Vector3(roomScale / 5, roomSpread - roomScale);

            MakePath(room, background, directory, position, scale, true);
        }
        if (room.left > -1 && room.index > room.left)
        {
            // make path to the left
            GameObject background = pathBackgrounds[Random.Range(0, pathBackgrounds.Count)];
            Vector3 position = parent.transform.position - new Vector3(roomSpread / 2, 0);
            Vector3 scale = new Vector3(roomSpread - roomScale, roomScale / 5);

            MakePath(room, background, directory, position, scale, false);
        }
    }

    public void LoadLevel(List<LevelManager.Room> rooms)
    {
        GameObject RoomDirectory = new GameObject();
        RoomDirectory.name = "Rooms";
        GameObject PathDirectory = new GameObject();
        PathDirectory.name = "Paths";

        foreach (LevelManager.Room room in rooms)
        {
            GameObject RoomParent = new GameObject();
            RoomParent.name = "Room " + room.index.ToString();
            RoomParent.transform.position = room.mapPosition;
            RoomParent.transform.parent = RoomDirectory.transform;

            LoadRoom(room, RoomParent);

            // make paths
            LoadPaths(room, RoomParent, PathDirectory);
        }
    }


    private void Start()
    {
        currentRoom = manager.GetRoom(0);
        // manager.DebugLevel();
        LoadLevel(manager.rooms);
    }
}
