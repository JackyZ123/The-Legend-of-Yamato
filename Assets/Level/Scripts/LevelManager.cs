using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreateLevel))]
public class LevelManager : MonoBehaviour
{
    [SerializeField]
    public class Room
    {
        public int index;

        public int up;
        public int right;
        public int down;
        public int left;

        public Vector2 position;
        public int distanceFromStart;
    }

    public CreateLevel generator;
    public LevelLoader loader;

    [Header("Map Generation")]
    public Vector2 size = new Vector2(10, 10);
    public Vector2 startPos = new Vector2(2, 2);
    public int[] startDirections;
    public int minRoomNum = 8;
    public int[] numPathProbabilities = { 2, 5, 1 };
    public int endRoomThreshold = 2;

    [Header("Map Deatils")]
    public List<Vector2> roomPos;
    public List<Room> rooms;
    public int maxDistance = 0;
    public Room endRoom;


    [Header("Debug")]
    public float debugScale = 0.75f;
    public GameObject debugNode;
    public GameObject debugPath;

    public void GenerateNewMap()
    {
        generator.CreateNewMap((int)size.x, (int)size.y, (int)startPos.x, (int)startPos.y, minRoomNum, startDirections, numPathProbabilities);
        GetMap();
        GetEndRoom();
    }

    void GetMap()
    {
        maxDistance = 0;
        roomPos = generator.GetRoomPositions();
        List<List<int>> conns = generator.GetRoomConnections();
        List<int> distanceToRooms = generator.GetDistanceToRooms();

        rooms = new List<Room>(0);

        for (int i = 0; i < roomPos.Count; i++)
        {
            rooms.Add(new Room());
            rooms[i].index = i;

            int[] paths = GetPaths(i, conns[i]);
            rooms[i].up = paths[0];
            rooms[i].right = paths[1];
            rooms[i].down = paths[2];
            rooms[i].left = paths[3];

            rooms[i].position = roomPos[i];
            rooms[i].distanceFromStart = distanceToRooms[i];
            maxDistance = Mathf.Max(maxDistance, distanceToRooms[i]);
        }
    }

    void GetEndRoom()
    {
        List<Room> possibleRooms = new List<Room>(0);

        foreach (Room room in rooms)
        {
            if (room.distanceFromStart >= maxDistance - endRoomThreshold)
            {
                possibleRooms.Add(room);
            }
        }

        endRoom = possibleRooms[Random.Range(0, possibleRooms.Count)];
    }

    public Room GetRoom(int index)
    {
        return rooms[index];
    }

    public int GetNodeFromPosition(Vector2 position)
    {
        if (!roomPos.Contains(position))
        {
            return -1;
        }
        return roomPos.IndexOf(position);
    }

    public int[] GetPaths(int node, List<int> conns)
    {
        Vector2 currentPos = roomPos[node];

        // print("Paths of " + node.ToString());

        int[] paths = new int[4] { -1, -1, -1, -1 };

        // check rooms

        foreach (int connectedNode in conns)
        {
            int[] deltaPosition = new int[] { (int)(roomPos[connectedNode].x - currentPos.x), (int)(roomPos[connectedNode].y - currentPos.y) };
            if (deltaPosition[1] == -1)
            {
                // the room is up
                paths[0] = connectedNode;
            }
            else if (deltaPosition[0] == 1)
            {
                // the room is to the right
                paths[1] = connectedNode;
            }
            else if (deltaPosition[1] == 1)
            {
                // the room is down
                paths[2] = connectedNode;
            }
            else if (deltaPosition[0] == -1)
            {
                // the room is to the left
                paths[3] = connectedNode;
            }
        }

        return paths;
    }

    public void DebugLevel()
    {
        // shows the level in the scene through squares with connections

        // start at node 0 and check each path and visit remembering the locations to add prefab of room

        if (GameObject.Find("Map Layout"))
        {
            DestroyImmediate(GameObject.Find("Map Layout"));
        }

        GameObject debugObject = new GameObject();
        debugObject.name = "Map Layout";
        GameObject debugNodes = new GameObject();
        debugNodes.name = "Nodes";
        debugNodes.transform.parent = debugObject.transform;
        GameObject debugPaths = new GameObject();
        debugPaths.name = "Paths";
        debugPaths.transform.parent = debugObject.transform;

        List<int> visited = new List<int>(0);

        visited.Add(-1);

        Room currentNode = loader.GetCurrentRoom();

        Queue<Room[]> toCheck = new Queue<Room[]>();

        toCheck.Enqueue(new Room[] { rooms[0], rooms[0] });

        while (toCheck.Count > 0)
        {
            Room[] nodeData = toCheck.Dequeue();
            Room thisNode = nodeData[0];
            Room lastNode = nodeData[1];

            // Debug.Log(thisNode.index);

            // Debug.Log("Node " + nodeData[0].ToString() + ", X: " + nodeData[1].ToString() + ", Y:" + nodeData[2].ToString());

            if (thisNode.index > lastNode.index)
            {
                // add path
                // path should be at pos - pos(node[3], node[4])
                // scale should be .5 for one it came from
                GameObject path = Instantiate(debugPath);
                path.transform.parent = debugPaths.transform;

                Vector2 newPos = thisNode.position + lastNode.position;
                newPos.y = -newPos.y;
                newPos += new Vector2(-8, 9);
                path.transform.position = newPos * debugScale;
                Vector2 scale = new Vector2(1, 1);

                if (thisNode.position.x - lastNode.position.x != 0)
                {
                    scale.y = 0.5f;
                }
                else
                {
                    scale.x = 0.5f;
                }

                path.transform.localScale = scale * debugScale;

                path.name = "Path from " + lastNode.ToString() + " to " + thisNode.ToString();
            }

            if (visited.Contains(thisNode.index))
            {
                continue;
            }

            visited.Add(thisNode.index);

            // add prefab
            GameObject nodePrefab = Instantiate(debugNode);

            if (thisNode.index == 0)
            {
                nodePrefab.GetComponent<SpriteRenderer>().color = Color.green;
            }
            if (thisNode.index == endRoom.index)
            {
                nodePrefab.GetComponent<SpriteRenderer>().color = Color.red;
            }

            if (thisNode.index == currentNode.index)
            {
                nodePrefab.GetComponent<SpriteRenderer>().color = Color.blue;
            }

            nodePrefab.transform.position = new Vector2(thisNode.position.x * 2 - 8, 9 - thisNode.position.y * 2) * debugScale;
            nodePrefab.transform.localScale = Vector2.one * debugScale;
            nodePrefab.name = "Node " + thisNode.index.ToString();
            nodePrefab.GetComponentInChildren<TextMesh>().text = (thisNode.index < 10 ? "0" : "") + thisNode.index.ToString();
            nodePrefab.transform.parent = debugNodes.transform;

            int[] paths = new int[] { thisNode.up, thisNode.right, thisNode.down, thisNode.left };

            // foreach (int path in paths)
            // {
            //     Debug.Log("Path " + nodeData[0].ToString() + " to " + path.ToString());
            // }

            // add connected nodes
            foreach (int connectedNode in paths)
            {
                if (connectedNode > -1)
                    toCheck.Enqueue(new Room[] { rooms[connectedNode], thisNode });
            }
        }

        loader.GetRoomExits();
    }

    private void Awake()
    {
        if (!generator)
        {
            generator = GetComponent<CreateLevel>();
        }

        if (!loader)
        {
            loader = GetComponent<LevelLoader>();
        }

        GenerateNewMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            DebugLevel();
        }
    }
}
