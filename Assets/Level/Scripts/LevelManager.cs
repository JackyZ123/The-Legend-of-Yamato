using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreateLevel))]
public class LevelManager : MonoBehaviour
{
    public CreateLevel generator;

    [Header("Map Information")]
    public Vector2 size = new Vector2(10, 10);
    public Vector2 startPos = new Vector2(2, 2);
    public int minRoomNum = 8;
    public int[] numPathProbabilities = { 2, 5, 1 };

    public int[,] map;
    public List<List<int>> conns;

    [Header("Node Navigation")]
    public int currentNode = 0;
    public Vector2 currentPosition = new Vector2(2, 2);

    void GenerateNewMap()
    {
        generator.CreateNewMap((int)size.x, (int)size.y, (int)startPos.x, (int)startPos.y, minRoomNum, numPathProbabilities);
        currentPosition = startPos;
        GetMap();
    }

    void GetMap()
    {
        map = generator.map;
        conns = generator.conns;
    }

    public int GetNode(Vector2 pos)
    {
        return map[(int)pos.y, (int)pos.x];
    }

    int CheckForPath(int node, int posX, int posY, int deltaX, int deltaY)
    {
        int path = -1;

        if (posX + deltaX >= 0 && posX + deltaX < size.x && posY + deltaY >= 0 && posY + deltaY < size.y - 1)
        {
            // we are in bounds
            int room = map[(int)posY + deltaY, (int)posX + deltaX];

            // Debug.Log("Room " + room.ToString() + " from " + node.ToString() + " at X=" + (posX + deltaX).ToString() + " and Y=" + (posY + deltaY).ToString());

            if (room > -1)
            {
                // there is a room up there
                if (conns[node].Contains(room))
                {
                    // there is a path to top room
                    path = room;
                }
            }
        }

        return path;
    }

    public int[] GetPaths(Vector2 position)
    {
        int node = map[(int)position.y, (int)position.x];
        currentNode = node;

        // print("Paths of " + node.ToString());

        int[] paths = new int[4] { -1, -1, -1, -1 };

        // check rooms

        paths[0] = CheckForPath(node, (int)position.x, (int)position.y, 0, -1);
        paths[1] = CheckForPath(node, (int)position.x, (int)position.y, 1, 0);
        paths[2] = CheckForPath(node, (int)position.x, (int)position.y, 0, 1);
        paths[3] = CheckForPath(node, (int)position.x, (int)position.y, -1, 0);

        return paths;
    }


    private void Awake()
    {
        if (!generator)
        {
            generator = GetComponent<CreateLevel>();
        }

        GenerateNewMap();
        int[] paths = GetPaths(startPos);

        string stringPath = "";

        // foreach (int path in paths)
        // {
        //     stringPath += path.ToString() + " ";
        // }

        Debug.Log(stringPath);
    }
}
