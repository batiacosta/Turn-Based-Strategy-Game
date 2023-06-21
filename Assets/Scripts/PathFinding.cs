using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public static PathFinding Instance { get; private set; }
    
    private const int MoveStraightCost = 10;
    private const int MoveDiagonaltCost = 14;
    [SerializeField] private Transform gridDebugObjectPrefab;
    
    private int _width;
    private int _height;
    private float _cellSize;
    private GridSystem<PathNode> _gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        _gridSystem = new GridSystem<PathNode>(10, 10, 2f, 
            (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        _gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closeList = new List<PathNode>();

        PathNode startNode = _gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = _gridSystem.GetGridObject(endGridPosition);
        
        openList.Add(startNode);
        for (int x = 0; x < _gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < _gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = _gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }
        
        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);
            if (currentNode == endNode)
            {
                //  Reached the final node
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);
            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closeList.Contains(neighbourNode))
                {
                    continue;
                }

                int tentativeGCost = currentNode.GetGCost() +
                                     CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());
                if (tentativeGCost < neighbourNode.GetGCost())
                {// This is a better path
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();
                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        //  No path found
        return null;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.GetCameFromPathNode() != null)
        {
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }
        pathNodeList.Reverse();
        List<GridPosition> gridPositionList = new List<GridPosition>();
        foreach (PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }
        return gridPositionList;
    }

    public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition gridPositionDistance = gridPositionA = gridPositionB;
        int distance = Mathf.Abs(gridPositionDistance.x) + Mathf.Abs(gridPositionDistance.z);
        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        var diagonalMovement = MoveDiagonaltCost * Mathf.Min(xDistance, zDistance);
        var straigtMovement = MoveStraightCost * remaining;
        return diagonalMovement * straigtMovement;
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
            {
                lowestFCostPathNode = pathNodeList[i];
            }
        }

        return lowestFCostPathNode;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();
        GridPosition gridPosition = currentNode.GetGridPosition();
        if (gridPosition.x - 1 >= 0)
        {
            //  Left node
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));
            if (gridPosition.z - 1 >= 0)
            {
                //  Left Down
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
            }

            if (gridPosition.z + 1 < _gridSystem.GetHeight())
            {
                //  Left Up
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
            }
        }

        if (gridPosition.x + 1 < _gridSystem.GetWidth())
        {
            //  Right node
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
            if (gridPosition.z - 1 >= 0)
            {
                //  Right Down
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            }
            if (gridPosition.z + 1 < _gridSystem.GetHeight())
            {
                //  Right Up
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
            }
        }

        if (gridPosition.z - 1 >= 0)
        {
            //  Down node
            neighbourList.Add(GetNode(gridPosition.x - 0, gridPosition.z - 1));
        }

        if (gridPosition.z + 1 < _gridSystem.GetHeight())
        {
            //  Up node
            neighbourList.Add(GetNode(gridPosition.x - 0, gridPosition.z + 1));
        }

        return neighbourList;
    }

    private PathNode GetNode(int x, int z)
    {
        return _gridSystem.GetGridObject(new GridPosition(x, z));
    }
}
