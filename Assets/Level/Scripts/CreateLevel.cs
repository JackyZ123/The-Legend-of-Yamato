using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLevel : MonoBehaviour
{
    private int minRooms = 8;

    private int[] numPathProbabilities;

    private int[] size;

    private int[,] map;

    private List<Vector2> roomPos = new List<Vector2>(0);

    private List<List<int>> conns = new List<List<int>>(0);

    private List<int> distanceToRooms = new List<int>(0);

    public List<Vector2> GetRoomPositions()
    {
        return roomPos;
    }

    public List<List<int>> GetRoomConnections()
    {
        return conns;
    }

    public List<int> GetDistanceToRooms()
    {
        return distanceToRooms;
    }


    int MakeLayout(int x, int y, int lx, int ly, int direction)
    {
        // return if out of range
        if (x < 0 || y < 0 || x >= size[0] || y >= size[1])
            return 0;

        int node = map[x, y];
        int lastNode = map[lx, ly];

        // starting node should only have one exit
        if (node == 0)
            return 0;

        if (node != -1)
        {
            // make a connection between nodes if it doesnt already exist
            if (!conns[node].Contains(lastNode))
                conns[node].Add(lastNode);
            if (!conns[lastNode].Contains(node))
                conns[lastNode].Add(node);

            return 0;
        }

        // this is a new node
        node = conns.Count;
        map[x, y] = node;
        roomPos.Add(new Vector2(y, x));
        conns.Add(new List<int> { });
        distanceToRooms.Add(-1);

        // add a connection between nodes
        if (!conns[node].Contains(lastNode))
            conns[node].Add(lastNode);
        if (!conns[lastNode].Contains(node))
            conns[lastNode].Add(node);

        List<int> canGo = new List<int> { 0, 1, 2, 3 };

        canGo.Remove((direction + 2) % 4);

        List<int> numPathsChoices = new List<int> { };

        // probabilities of other room sizes
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < numPathProbabilities[i]; j++)
            {
                numPathsChoices.Add(i + 1);
            }
        }

        // decrease chance of large maps
        if (node > 8)
        {
            for (int i = 0; i < node - 5; i++)
            {
                numPathsChoices.Add(0);
            }
        }

        // get number of connections
        int numPaths = numPathsChoices[Random.Range(0, numPathsChoices.Count)];

        // record the size of the map
        int maxSize = node;

        for (int i = 0; i < numPaths; i++)
        {
            // choose a direction and remove from choices
            int chosenDir = canGo[Random.Range(0, canGo.Count)];
            canGo.Remove(chosenDir);

            int newX, newY;
            newX = x;
            newY = y;

            // find new location
            if (chosenDir == 0 || chosenDir == 2)
            {
                newX += chosenDir - 1;
            }
            else if (chosenDir == 1 || chosenDir == 3)
            {
                newY += 2 - chosenDir;
            }

            // dp and record size
            int newSize = MakeLayout(newX, newY, x, y, chosenDir);
            if (newSize > maxSize)
            {
                maxSize = newSize;
            }
        }

        return maxSize;
    }

    int MakeFirst(int x, int y, int[] dirs)
    {
        // make first node
        map[x, y] = 0;
        roomPos.Add(new Vector2(y, x));
        conns.Add(new List<int>(0));
        distanceToRooms.Add(-1);

        int chosenDir = dirs[Random.Range(0, dirs.Length)];

        int newX, newY;
        newX = x;
        newY = y;

        // find new location
        if (chosenDir == 0 || chosenDir == 2)
        {
            newX += chosenDir - 1;
        }
        else if (chosenDir == 1 || chosenDir == 3)
        {
            newY += 2 - chosenDir;
        }

        return MakeLayout(newX, newY, x, y, chosenDir);
    }

    void ResetMap(int x, int y)
    {
        map = new int[y, x];
        size = new int[] { y, x };
        roomPos = new List<Vector2>(0);
        conns = new List<List<int>>(0);
        distanceToRooms = new List<int>(0);

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                map[i, j] = -1;
            }
        }
    }

    int CalculateDistances()
    {
        // dijkstras to find distances

        Queue<int[]> toVisit = new Queue<int[]>();
        toVisit.Enqueue(new int[] { 0, 0 });

        int maxDistance = -1;

        while (toVisit.Count > 0)
        {
            // check node
            int[] currentNode = toVisit.Dequeue();

            if (distanceToRooms[currentNode[1]] > -1)
            {
                // we have been here
                continue;
            }

            distanceToRooms[currentNode[1]] = currentNode[0];
            maxDistance = currentNode[0];

            // loop through connected nodes
            foreach (int node in conns[currentNode[1]])
            {
                toVisit.Enqueue(new int[] { currentNode[0] + 1, node });
            }
        }

        return maxDistance;
    }

    void PrintDetails()
    {
        string toPrint = "\n";

        int maxNode = conns.Count;

        toPrint += "Number of Rooms: " + maxNode.ToString() + "\n";

        int maxDistance = -1;

        foreach (int distance in distanceToRooms)
        {
            maxDistance = Mathf.Max(maxDistance, distance);
        }

        toPrint += "Max Distance: " + maxDistance.ToString() + "\n\n";


        // map layout
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == -1)
                    toPrint += "     ";

                else if (map[i, j] < 10)
                    toPrint += "  " + map[i, j].ToString();

                else
                    toPrint += map[i, j].ToString();

                toPrint += "  ";
            }

            toPrint += "\n";
        }

        toPrint += "\n";

        // room positions
        for (int i = 0; i < roomPos.Count; i++)
        {
            toPrint += distanceToRooms[i].ToString() + " - ";
            toPrint += i.ToString() + ": " + roomPos[i][0].ToString() + " " + roomPos[i][1].ToString() + "\n";
        }

        Debug.Log(toPrint);
    }

    public void CreateNewMap(int sizeX, int sizeY, int startPosX, int startPosY, int minRoomNum, int[] startDirections, int[] pathNums)
    {
        minRooms = minRoomNum;
        numPathProbabilities = pathNums;

        ResetMap(sizeX, sizeY);

        int maxNode = MakeFirst(startPosX, startPosY, startDirections);
        int distance = CalculateDistances();

        while (maxNode < 8 || distance < 5)
        {
            ResetMap(sizeX, sizeY);
            maxNode = MakeFirst(startPosX, startPosY, startDirections);
            distance = CalculateDistances();
        }

        PrintDetails();
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
