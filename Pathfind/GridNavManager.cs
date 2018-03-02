using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Grid;

namespace Grid.Pathfind
{
    /// <summary>
    /// Grid Copy with Nodes instead of Blocks for Pathfinding.
    /// </summary>
    public class GridNavManager : MonoBehaviour
    {
        public static GridNavManager Instance = null;
        GridManager manager { get { return GridManager.Instance; } }

        GridNode[,] _grid = new GridNode[,] { };
        public GridNode[,] grid { get { return _grid; } }

        public int rows { get { return manager.rows; } }
        public int columns { get { return manager.columns; } }

        void CreateGrid()
        {
            _grid = new GridNode[rows, columns];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    GridBlock block = manager.GetBlock(new GridPosition(r, c));
                    _grid[r, c] = new GridNode(block);
                }
            }
        }

        void Singleton()
        {
            if (Instance == null)
            {
                Instance = this;
                CreateGrid();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public GridNode GetNode(GridPosition position)
        {
            GridPosition pos = new GridPosition(position.row, position.column);

            if (position.row < 0)
            {
                pos.row = 0;
            }

            if (position.row > rows - 1)
            {
                pos.row = rows - 1;
            }

            if (position.column < 0)
            {
                pos.column = 0;
            }

            if (position.column > columns - 1)
            {
                pos.column = columns - 1;
            }

            return grid[pos.row, pos.column];
        }

        public GridNode GetNode(GridBlock block)
        {
            return GetNode(block.gridPosition);
        }

        void Awake()
        {
            Singleton();
        }
    }
}