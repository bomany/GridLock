using UnityEngine;
using System.Collections;
using Grid;

namespace Grid.Pathfind
{
    /// <summary>
    /// Representation of a Node in a Path.
    /// </summary>
    public class GridPathNode
    {
        //Distance from target
        int _hCost;
        // Movement Cost
        int _gCost;
        public int gCost { get { return _gCost; } }
        // Total Cost.
        public int fCost { get { return _gCost + _hCost; } }

        GridNode _node;
        public GridNode node { get { return _node; } }

        public GridPathNode parent;

        public GridPosition position { get { return node.position; } }

        public GridBlock block { get { return node.block; } }

        public void CalculateCosts(GridNode target)
        {
            _hCost = Mathf.Abs(target.position.row - node.position.row) + Mathf.Abs(target.position.column - node.position.column);

            _gCost = 0;
            if (parent != null)
            {
                _gCost = parent.gCost + node.totalCost;
            }
        }

        public GridPathNode(GridNode node)
        {
            this._node = node;
        }

    }
}