using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelManager))]
public class LevelLoader : MonoBehaviour
{
    public LevelManager manager;

    public GameObject debugNode;
    public GameObject debugPath;

    private void Awake()
    {
        if (!manager)
        {
            manager = GetComponent<LevelManager>();
        }
    }


    void DebugLevel()
    {
        // shows the level in the scene through squares with connections

        // start at node 0 and check each path and visit remembering the locations to add prefab of room

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

        Queue<int[]> toCheck = new Queue<int[]>();

        toCheck.Enqueue(new int[] { 0, (int)manager.startPos.x, (int)manager.startPos.y, 0, 0 });

        while (toCheck.Count > 0)
        {
            int[] nodeData = toCheck.Dequeue();

            // Debug.Log("Node " + nodeData[0].ToString() + ", X: " + nodeData[1].ToString() + ", Y:" + nodeData[2].ToString());

            Vector2 pos = new Vector2(nodeData[1], nodeData[2]);

            if (nodeData[0] > manager.GetNode(new Vector2(nodeData[1] + nodeData[3], nodeData[2] + nodeData[4])))
            {
                // add path
                // path should be at pos - pos(node[3], node[4])
                // scale should be .5 for one it came from
                GameObject path = Instantiate(debugPath);
                path.transform.parent = debugPaths.transform;

                path.transform.position = new Vector2(pos.x * 2 - 8, 9 - pos.y * 2) + new Vector2(nodeData[3], -nodeData[4]);
                Vector2 scale = new Vector2(1, 1);

                if (nodeData[3] != 0)
                {
                    scale.y = 0.5f;
                }
                else
                {
                    scale.x = 0.5f;
                }

                path.transform.localScale = scale;

                path.name = "Path from " + manager.GetNode(new Vector2(nodeData[1] + nodeData[3], nodeData[2] + nodeData[4])).ToString() + " to " + nodeData[0].ToString();
            }

            if (visited.Contains(nodeData[0]))
            {
                continue;
            }

            visited.Add(nodeData[0]);

            // add prefab
            GameObject node = Instantiate(debugNode);

            node.transform.position = new Vector2(pos.x * 2 - 8, 9 - pos.y * 2);
            node.name = "Node " + nodeData[0].ToString();
            node.GetComponentInChildren<TextMesh>().text = (nodeData[0] < 10 ? "0" : "") + nodeData[0].ToString();
            node.transform.parent = debugNodes.transform;

            int[] paths = manager.GetPaths(pos);

            // foreach (int path in paths)
            // {
            //     Debug.Log("Path " + nodeData[0].ToString() + " to " + path.ToString());
            // }

            // add connected nodes
            if (paths[0] > -1)
            {
                toCheck.Enqueue(new int[] { paths[0], (int)pos.x, (int)pos.y - 1, 0, 1 });
            }
            if (paths[1] > -1)
            {
                toCheck.Enqueue(new int[] { paths[1], (int)pos.x + 1, (int)pos.y, -1, 0 });
            }
            if (paths[2] > -1)
            {
                toCheck.Enqueue(new int[] { paths[2], (int)pos.x, (int)pos.y + 1, 0, -1 });
            }
            if (paths[3] > -1)
            {
                toCheck.Enqueue(new int[] { paths[3], (int)pos.x - 1, (int)pos.y, 1, 0 });
            }
        }
    }

    private void Start()
    {
        DebugLevel();
    }
}
