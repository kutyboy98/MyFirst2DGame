using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    public static Dictionary<PointComponent, Node> nodes;
    public static void CreateNodes()
    {
        nodes = new Dictionary<PointComponent, Node>();

        foreach (PointManager item in GrassManager.Instance.GrassCoordinatesDict.Values)
        {
            nodes.Add(item.Point, new Node(item));
        }
    }

    public static Stack<Node> GetPath(PointComponent startPoint, PointComponent goalPoint)
    {
        if (nodes == null)
        {
            CreateNodes();
        }

        HashSet<Node> openList = new HashSet<Node>();
        HashSet<Node> closeList = new HashSet<Node>();
        Stack<Node> finalPath = new Stack<Node>();
        finalPath.Push(nodes[goalPoint]);

        Node currentNode = nodes[startPoint];

        openList.Add(currentNode);

        while (openList.Count > 0)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    PointComponent neighborPosition = new PointComponent(currentNode.GridPoint.X + x, currentNode.GridPoint.Y + y);
                    if (GrassManager.Instance.InBound(neighborPosition) && nodes[neighborPosition].PointRef.IsWalkAble && neighborPosition != currentNode.GridPoint)
                    {
                        int gCost = 0;
                        if (Math.Abs(x - y) == 1)
                        {
                            gCost = 10;
                        }
                        else
                        {
                            if (!DiagonallyAble(currentNode, nodes[neighborPosition]))
                            {
                                continue;
                            }
                            gCost = 14;
                        }
                        Node neighborNode = nodes[neighborPosition];
                        if (openList.Contains(neighborNode))
                        {
                            if (currentNode.G + gCost < neighborNode.G)
                            {
                                neighborNode.CalcScore(currentNode, goalPoint, gCost);
                            }
                        }
                        else if (!closeList.Contains(neighborNode))
                        {
                            openList.Add(neighborNode);
                            neighborNode.CalcScore(currentNode, goalPoint, gCost);
                        }

                        //if (!openList.Contains(neighborNode))
                        //{
                        //    openList.Add(neighborNode);
                        //    //neighborNode.PointRef.spriteRenderer.color = Color.blue;
                        //}

                        //neighborNode.CalcScore(currentNode, goalPoint, gCost);
                    }
                }
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            if (openList.Count > 0)
            {
                currentNode = openList.OrderBy(x => x.F).First();
            }

            if (currentNode == nodes[goalPoint])
            {                
                while (currentNode != nodes[startPoint])
                {
                    currentNode = currentNode.Parent;
                    finalPath.Push(currentNode);
                }
                return finalPath;
            }
        }
        return null;
        //GameObject.Find("AStarDebugger").GetComponent<AStarDebugger>().DebugPath(openList, closeList, finalPath);
    }

    private static bool DiagonallyAble(Node currentNode, Node neighborNode)
    {
        PointComponent direction = neighborNode.GridPoint - currentNode.GridPoint;
        PointComponent firstAsidePoint = new PointComponent(currentNode.GridPoint.X, currentNode.GridPoint.Y + direction.Y);
        PointComponent secondAsidePoint = new PointComponent(currentNode.GridPoint.X + direction.X, currentNode.GridPoint.Y);
        if (GrassManager.Instance.InBound(firstAsidePoint) && !nodes[firstAsidePoint].PointRef.IsWalkAble)
        {
            return false;
        }
        if (GrassManager.Instance.InBound(secondAsidePoint) && !nodes[secondAsidePoint].PointRef.IsWalkAble)
        {
            return false;
        }
        return true;
    }
}
