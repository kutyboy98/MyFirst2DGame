using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class AStarDebugger : MonoBehaviour
{
    private PointManager start, goal;
    // Start is called before the first frame update
    [SerializeField]
    private GameObject arrowPrefab;
    [SerializeField]
    private GameObject blankPrefab;
    void Start()
    {

    }

    // Update is called once per frame
    //void Update()
    //{
    //    ClickGrass();
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        AStar.GetPath(start.Point, goal.Point);
    //    }
    //}

    private void ClickGrass()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                PointManager tmp = hit.collider.GetComponent<PointManager>();
                if (tmp != null)
                {
                    if (start == null)
                    {
                        start = tmp;
                        CreateBlankGrass(start.CenterPoint, Color.blue);
                    }
                    else
                    {
                        if (goal != null)
                        {
                            CreateBlankGrass(goal.CenterPoint, Color.clear);
                        }
                        goal = tmp;
                        CreateBlankGrass(goal.CenterPoint, Color.red);
                    }
                }
            }
        }
    }

    public void DebugPath(HashSet<Node> openList, HashSet<Node> closeList, Stack<Node> path)
    {
        foreach (var node in openList)
        {
            if (node.GridPoint != start.Point)
            {
                if (node.GridPoint != goal.Point)
                {
                    CreateBlankGrass(node.PointRef.CenterPoint, Color.gray, node);
                }
                PointToParent(node, node.PointRef.CenterPoint);
            }
        }
        foreach (var node in closeList)
        {
            if (node.GridPoint != start.Point && node.GridPoint != goal.Point)
            {
                CreateBlankGrass(node.PointRef.CenterPoint, Color.yellow, node);
                PointToParent(node, node.PointRef.CenterPoint);
            }
        }
        foreach (var node in path)
        {
            if (node.GridPoint != start.Point && node.GridPoint != goal.Point)
            {
                CreateBlankGrass(node.PointRef.CenterPoint, Color.green, node);
                PointToParent(node, node.PointRef.CenterPoint);
            }
        }
    }

    private void PointToParent(Node node, Vector2 position)
    {
        GameObject arrow = Instantiate(arrowPrefab, position, Quaternion.identity);
        if (node.GridPoint.X < node.Parent.GridPoint.X)
        {
            if (node.GridPoint.Y < node.Parent.GridPoint.Y)
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 315);
            }
            else if (node.GridPoint.Y == node.Parent.GridPoint.Y)
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 45);
            }
        }
        else if (node.GridPoint.X == node.Parent.GridPoint.X)
        {
            if (node.GridPoint.Y < node.Parent.GridPoint.Y)
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 270);
            }
            else if (node.GridPoint.Y == node.Parent.GridPoint.Y)
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 90);
            }

        }
        else
        {
            if (node.GridPoint.Y < node.Parent.GridPoint.Y)
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 225);
            }
            else if (node.GridPoint.Y == node.Parent.GridPoint.Y)
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 180);
            }
            else
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 135);
            }
        }
    }

    private void CreateBlankGrass(Vector3 position, Color color, Node node = null)
    {
        GameObject blankGrass = Instantiate(blankPrefab, position, Quaternion.identity);
        if (node == null)
            blankGrass.GetComponent<SpriteRenderer>().color = color;
        else if (node != null && node.GridPoint != goal.Point)
            blankGrass.GetComponent<SpriteRenderer>().color = color;
        if (node != null)
        {
            var blankGrassDebuggerScript = blankGrass.GetComponent<BlankGrassDebugger>();
            blankGrassDebuggerScript.H.text += node.H;
            blankGrassDebuggerScript.G.text += node.G;
            blankGrassDebuggerScript.F.text += node.F;
        }
    }
}
