using UnityEngine;
using System.Collections;
using Grid;

namespace Grid.Pathfind
{
    /// <summary>
    /// Representation of Grid Block for Pathfinding.
    /// </summary>
    public class GridNode
    {
        //Base Movement Cost.
        int baseGCost = 10;
        //Extra Movement Cost.
        int extraCost = 0;

        //If is Blocked
        bool _isObstacle = false;
        public bool isObstacle { get { return (_isObstacle || block.isBlocked); } }

        //Total Movement Cost.
        public int totalCost { get { return baseGCost + extraCost; } }

        //Represented block.
        public GridBlock block;
        public GridPosition position { get { return block.gridPosition; } }

        public GridPathNode GridPathNode()
        {
            return new GridPathNode(this);
        }

        public bool NodeIsValid()
        {
            if (isObstacle)
            {
                return false;
            }
            return true;
        }


        public GridNode(GridBlock block)
        {
            this.block = block;
        }

        //Sets any extra cost
        public void SetCost(int cost)
        {
            extraCost = cost;
        }
    }
}