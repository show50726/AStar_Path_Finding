using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public int GridW;
    public int GridH;

    public GameObject player;
    public Vector3 endPos { get; set; }
    public Sprite GridImg;
    public Sprite ObstImg;

    private List<List<Node>> list;
    private List<Node> openList;
    private List<Node> closeList;
    private Node endNode;

    private const float diagonalCost = 1.4f; 
    private const float straightCost = 1f; 

    // Start is called before the first frame update
    void Start()
    {
        list = new List<List<Node>>();
        openList = new List<Node>();
        closeList = new List<Node>();

        for(int i = 0; i < GridH; i++)
        {
            var row = new List<Node>();
            for(int j = 0; j < GridW; j++)
            {
                Node d = new Node(j, i);
                GameObject obj = Instantiate(new GameObject());
                d.img = obj;
                obj.transform.position = new Vector3(j, i);
                obj.AddComponent<SpriteRenderer>();
                obj.GetComponent<SpriteRenderer>().sprite = GridImg;
                row.Add(d);
            }
            list.Add(row);
        }
        openList.Clear();
        closeList.Clear();

        openList.Add(list[(int)Mathf.Floor(player.transform.position.y - transform.position.y)][(int)Mathf.Floor(player.transform.position.x - transform.position.x)]);
        endNode = openList[0];
    }

    // Update is called once per frame
    void Update()
    {
        List<Node> path = null;
        if (Input.GetMouseButtonDown(0))
        {
            updateTarget();
            path = FindPath();
            player.GetComponent<Player>().newPath(path);
            if (path != null)
            {
                Debug.Log("Success!");
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Node node = path[i];
                    Debug.Log(node.x + " " + node.y);
                    Debug.DrawLine(new Vector3(node.x + 0.5f, node.y + 0.5f), new Vector3(path[i + 1].x + 0.5f, path[i + 1].y + 0.5f), Color.red, 2, false);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            vec.z = 0f;
            Node node = list[(int)Mathf.Floor(vec.y - transform.position.y)][(int)Mathf.Floor(vec.x - transform.position.x)];
            node.walkable = !node.walkable;
            if (node.walkable)
            {
                node.img.GetComponent<SpriteRenderer>().sprite = GridImg;
            }
            else
            {
                node.img.GetComponent<SpriteRenderer>().sprite = ObstImg;
            }
        }

    }

    public void updateTarget()
    {
        //openList.Add(list[(int)Mathf.Floor(player.transform.position.y - transform.position.y)][(int)Mathf.Floor(player.transform.position.x - transform.position.x)]);
        for(int i = 0; i < GridH; i++)
        {
            for(int j = 0; j < GridW; j++)
            {
                list[i][j].Initialize();
            }
        }
        Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vec.z = 0f;
        endPos = vec;
        Debug.Log(endPos);
        getEndNode();
    }

    public void getEndNode()
    {
        endNode = list[(int)Mathf.Floor(endPos.y - transform.position.y)][(int)Mathf.Floor(endPos.x - transform.position.x)];
    }

    public float getHeuristicEstimate(Node node)
    {
        float x = node.x - endNode.x;
        float y = node.y - endNode.y;
        return x + y;
    }

    public List<Node> CalcPath()
    {
        Debug.Log("Start Calculating.");
        List<Node> path = new List<Node>();
        path.Add(endNode);
        Node curNode = endNode.from;
        while(curNode != null)
        {
            path.Add(curNode);
            curNode = curNode.from;
        }
        //path.Reverse();

        return path;
    }

    public List<Node> FindPath()
    {
        openList.Clear();
        closeList.Clear();
        openList.Add(endNode);
        endNode = list[(int)Mathf.Floor(player.transform.position.y - transform.position.y)][(int)Mathf.Floor(player.transform.position.x - transform.position.x)];

        while (openList.Count > 0)
        {
            Node node = openList[0];
            openList.RemoveAt(0);
            closeList.Add(node);

            if (Node.Equal(node, endNode)) return CalcPath();

            var dir = new int[3] { -1, 0, 1 };

            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    //Debug.Log("1");
                    if (i == 0 && j == 0) continue;

                    int newX = node.x + dir[i];
                    int newY = node.y + dir[j];

                    if (newX >= 0 && newX < GridW && newY >= 0 && newY < GridH)
                    {
                        if (closeList.Contains(list[newY][newX]))
                        {
                            continue;
                        }

                        if (!list[newY][newX].walkable)
                        {
                            Debug.Log("No");
                            closeList.Add(list[newY][newX]);
                            continue;
                        }

                        float x = node.x - list[newY][newX].x;
                        float y = node.y - list[newY][newX].y;
                        list[newY][newX].hCost = getHeuristicEstimate(list[newY][newX]);

                        float tmpCost = node.gCost + ((i == j) ? diagonalCost : straightCost);
                        if(tmpCost < list[newY][newX].gCost || list[newY][newX].gCost == 0)
                        {
                            list[newY][newX].gCost = tmpCost;
                            list[newY][newX].from = node;
                            list[newY][newX].CalcCost();

                            if (!openList.Contains(list[newY][newX]))
                            {
                                //Debug.Log("add" + list[newY][newX].x + " " + list[newY][newX].y);
                                openList.Add(list[newY][newX]);
                            }
                        }
                    }
                }
            }

        }

        return null;
    }
}
