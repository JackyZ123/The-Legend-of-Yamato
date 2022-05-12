using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int minRooms = 8;

    public int[] numPathProbabilities;

    public int[] size = { 10, 10 };

    public int[] startPos;
    public int[] startDirs;

    public int[,] map;
    public List<List<int>> conns = new List<List<int>>(0);

    int MakeMap(int x, int y, int lx, int ly, int direction)
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
        conns.Add(new List<int> { });

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
            int newSize = MakeMap(newX, newY, x, y, chosenDir);
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
        conns.Add(new List<int>(0));

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

        return MakeMap(newX, newY, x, y, chosenDir);
    }

    void ResetMap()
    {
        for (int i = 0; i < size[0]; i++)
        {
            for (int j = 0; j < size[1]; j++)
            {
                map[i, j] = -1;
            }
        }
    }

    void PrintDetails(int maxNode)
    {
        Debug.Log("Size: " + maxNode.ToString());

        string toPrint = "";

        for (int i = 0; i < size[0]; i++)
        {
            for (int j = 0; j < size[1]; j++)
            {
                if (map[i, j] == -1)
                    toPrint += "    ";

                else if (map[i, j] < 10)
                    toPrint += " " + map[i, j].ToString() + " ";

                else
                    toPrint += map[i, j].ToString() + " ";
            }

            toPrint += "\n";
        }

        Debug.Log(toPrint);
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        map = new int[10, 10];

        ResetMap();

        int maxNode = MakeFirst(startPos[0], startPos[1], startDirs);

        while (maxNode < 8)
        {
            ResetMap();
            maxNode = MakeFirst(startPos[0], startPos[1], startDirs);
        }

        PrintDetails(maxNode);
    }
}
