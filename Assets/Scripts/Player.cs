using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 2f;

    private int nowIndex;
    private bool Finished = true;
    private Vector3 direction = Vector3.zero;
    private List<Node> path = null;
    
    public void newPath(List<Node> list)
    {
        if (list != null)
        {
            path = list;
            nowIndex = 1;
            Finished = false;
            direction = new Vector3(path[nowIndex].x + 0.5f - transform.position.x, path[nowIndex].y + 0.5f - transform.position.y);
        }
    }

    public void followPath()
    {
        if (Vector3.Distance(transform.position, new Vector3(path[nowIndex].x + 0.5f, path[nowIndex].y + 0.5f)) < 0.001f)
        {
            nowIndex++;
            if (nowIndex == path.Count)
            {
                Finished = true;
                return;
            }
            direction = new Vector3(path[nowIndex].x + 0.5f - transform.position.x, path[nowIndex].y + 0.5f - transform.position.y);
        }

        transform.position += direction * Time.fixedDeltaTime * speed;
    }

    private void FixedUpdate()
    {
        if(!Finished)
        {
            followPath();
        }
    }


}
