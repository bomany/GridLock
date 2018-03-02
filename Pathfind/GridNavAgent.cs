using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Grid;

namespace Grid.Pathfind
{
    /// <summary>
    /// Calulates Path and Stores it.
    /// </summary>
    public class GridNavAgent : MonoBehaviour
    {
        //Target Destination
        GridNode target;

        // Start Point
        GridNode start;

        List<GridPathNode> openList = new List<GridPathNode>();
        List<GridPathNode> closedList = new List<GridPathNode>();
        List<GridPathNode> finalPath = new List<GridPathNode>();
        List<GridNode> nodePath = new List<GridNode>();

        public List<GridNode> path { get { return nodePath; } }

        GridNavManager navigation { get { return GridNavManager.Instance; } }
        GridNode[,] grid { get { return navigation.grid; } }
        int maxRows { get { return navigation.rows - 1; } }
        int maxColumns { get { return navigation.columns - 1; } }

        GridPathNode currentNode;
        GridEntity _entity;
        GridEntity entity
        {
            get
            {
                if (_entity == null)
                {
                    _entity = GetComponent<GridEntity>();
                }
                return _entity;
            }
        }

        // Gets total F Cost of current path.
        int totalFCost
        {
            get
            {
                int total = 0;
                foreach (GridPathNode node in finalPath)
                {
                    total += node.fCost;
                }
                return total;
            }

        }

        public float speed = 1f;
        public float distanceTolerance = 0f;
        public bool walkTo = false;

        public bool isReseting = false;

        Rigidbody rb;
        Rigidbody2D rb2D;

        //Returns path if not calculated already.
        public List<GridNode> Path()
        {
            if (path.Count == 0)
            {
                CalculatePath();
            }
            return path;
        }

        //Pathfinds to target.
        public void CalculatePath()
        {
            if (target != null && start != null)
            {
                finalPath.Clear();
                nodePath.Clear();
                closedList.Clear();
                openList.Clear();

                openList.Add(new GridPathNode(start));

                while (finalPath.Count <= 0 && openList.Count > 0)
                {
                    GetNextNode();
                    GetSurroundingNodes();
                }
            }
        }

        //Builds final path.
        void BuildPath()
        {
            GridPathNode node = openList.Single(x => x.node == target);
            while (node.parent != null)
            {
                finalPath.Add(node);
                node = node.parent;
            }
            finalPath.Reverse();
            nodePath = finalPath.Select(x => x.node).ToList();
        }

        //Switch to next node.
        void GetNextNode()
        {
            openList = openList.OrderBy(x => x.fCost).ToList();

            currentNode = openList.FirstOrDefault();
            closedList.Add(currentNode);
            openList.RemoveAt(0);
        }


        void ValidateAndAddNode(GridNode node, ref List<GridPathNode> nodes)
        {
            GridPathNode pathNode;
            if (!closedList.Any(x => x.node == node) && node.NodeIsValid())
            {
                pathNode = new GridPathNode(node);
                nodes.Add(pathNode);
                pathNode.parent = currentNode;
                pathNode.CalculateCosts(target);
            }
        }

        // Checks and adds surrounding nodes.
        void GetSurroundingNodes()
        {
            List<GridPathNode> nodes = new List<GridPathNode>();
            GridNode node;

            // Add Above Node
            if (currentNode.position.row > 0)
            {
                node = navigation.GetNode(new GridPosition(currentNode.position.row - 1, currentNode.position.column));
                ValidateAndAddNode(node, ref nodes);
            }

            //Add Below Node
            if (currentNode.position.row < maxRows)
            {
                node = navigation.GetNode(new GridPosition(currentNode.position.row + 1, currentNode.position.column));
                ValidateAndAddNode(node, ref nodes);
            }

            //Add Left Node
            if (currentNode.position.column > 0)
            {
                node = navigation.GetNode(new GridPosition(currentNode.position.row, currentNode.position.column - 1));
                ValidateAndAddNode(node, ref nodes);
            }

            //Add Right Node
            if (currentNode.position.column < maxColumns)
            {
                node = navigation.GetNode(new GridPosition(currentNode.position.row, currentNode.position.column + 1));
                ValidateAndAddNode(node, ref nodes);
            }

            openList.AddRange(nodes);

            // Found Path
            if (nodes.Any(x => x.node == target))
            {
                BuildPath();
            }
        }

        // Set Target with GridNode
        public void SetTarget(GridNode target)
        {
            this.target = target;
            //CalculatePath();
        }

        // Set Target with GridBlock
        public void SetTarget(GridBlock target)
        {
            SetTarget(navigation.GetNode(target));
        }

        // Set Target with GridPosition
        public void SetTarget(GridPosition target)
        {
            SetTarget(navigation.GetNode(target));
        }

        // Set Start with GridNode
        public void SetStart(GridNode start)
        {
            this.start = start;
        }

        // Set Start with GridBlock
        public void SetStart(GridBlock start)
        {
            SetStart(navigation.GetNode(start));
        }

        // Set Start with GridPosition
        public void SetStart(GridPosition start)
        {
            SetStart(navigation.GetNode(start));
        }

        // MoveTo Target
        void MoveTo(Vector3 target)
        {
            //transform.Translate((target - transform.position).normalized * (speed * Time.deltaTime));
            //return;
            StopMove();
            if (rb != null)
            {
                rb.velocity = (target - transform.position).normalized * (speed * Time.deltaTime);
            }
            else if (rb2D != null)
            {
                rb2D.velocity = (target - transform.position).normalized * (speed * Time.deltaTime);
            }
        }

        // Stops all Movement.
        void StopMove()
        {
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
            }
            else if (rb2D != null)
            {
                rb2D.velocity = Vector3.zero;
            }
        }

        // Move along Path
        void FollowPath()
        {
            if (path.Count > 0)
            {
                GridBlock block = path.First().block;
                float distance = Vector3.Distance(transform.position, block.position);
                if (distance <= distanceTolerance)
                {
                    entity.SetBlock(block);
                    finalPath.RemoveAt(0);
                    nodePath.RemoveAt(0);
                    StopMove();
                }
                else
                {
                    MoveTo(block.position);
                }

                if (path.Count <= 0)
                {
                    MoveTo(transform.position);
                }
            }
        }

        void ResetToStart()
        {
            float distance = Vector3.Distance(transform.position, entity.block.position);
            MoveTo(entity.block.position);
            if (distance <= distanceTolerance)
            {
                StopMove();
                isReseting = false;
            }

        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            for (int i = path.Count - 1; i > 0; i--)
            {
                GridPathNode node = finalPath[i];
                if (node.parent != null)
                {
                    Gizmos.DrawLine(node.block.position, node.parent.block.position);
                }
            }
        }

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb2D = GetComponent<Rigidbody2D>();
        }

        //TODO Should be handled by user script.
        void FixedUpdate()
        {
            if (walkTo)
            {
                if (isReseting)
                {
                    ResetToStart();
                }
                else
                {
                    FollowPath();
                }
            }
        }
    }
}