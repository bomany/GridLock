using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Grid.Pathfind;

namespace Grid
{
    /// <summary>
    /// Creates and Manages Grid.
    /// Also contains utilities relating to grid and its block.
    /// </summary>
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance = null;

        GridBlock[,] _grid = new GridBlock[,] { };
        public GridBlock[,] grid { get { return _grid; } }

        [Tooltip("Amount of Rows")]
        public int rows = 5;

        [Tooltip("Amount of Columns")]
        public int columns = 5;

        [Tooltip("Block size in Unity Metrics")]
        public float blockSize = 1f;

        // What axis is the grid operating in.
        public enum Axis { xy, yz, xz }
        [Tooltip("Axis the grid is operating in")]
        public Axis axis = Axis.xy;

        // Pathfinding
        public GridNavManager navigation { get { return GridNavManager.Instance; } }

        /// <summary>
        /// Instatiate a new Grid Block
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        GridBlock CreateGridBlock(int row, int column)
        {
            Vector3 position;
            if (axis == Axis.xz)
            {
                position = new Vector3(transform.position.x + column * blockSize, transform.position.y, transform.position.z + row * blockSize);
            }
            else if (axis == Axis.yz)
            {
                position = new Vector3(transform.position.x, transform.position.y + column * blockSize, transform.position.z + row * blockSize);
            }
            else
            {
                position = new Vector3(transform.position.x + column * blockSize, transform.position.y - row * blockSize, transform.position.z);
            }

            return new GridBlock(new GridPosition(row, column), position);
        }

        /// <summary>
        /// Generates Grid Array
        /// </summary>
        void CreateGrid()
        {
            _grid = new GridBlock[rows, columns];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    _grid[r, c] = CreateGridBlock(r, c);
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

        /// <summary>
        /// Returns a random empty block
        /// </summary>
        public GridBlock GetRandomBlock()
        {
            List<GridBlock> blocks = grid.Cast<GridBlock>().Where(x => x.isBlocked == false).ToList();
            return blocks[Random.Range(0, blocks.Count - 1)];
        }

        /// <summary>
        /// Returns a GridBlock at a GridPosition
        /// </summary>
        /// <param name="position"></param>
        public GridBlock GetBlock(GridPosition position)
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

        /// <summary>
        /// Return a GridBlock at a row and column.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public GridBlock GetBlock(int row, int column)
        {
            return GetBlock(new GridPosition(row, column));
        }

        /// <summary>
        /// Returns the distance between two GridPositions
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Distance(GridPosition a, GridPosition b)
        {
            return Mathf.Sqrt(Mathf.Pow(Mathf.Abs(a.row - b.row), 2) + Mathf.Pow(Mathf.Abs(a.column - b.column), 2));
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            foreach (GridBlock block in grid)
            {
                Gizmos.color = Color.yellow;
                if (navigation.GetNode(block).isObstacle)
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawWireCube(block.position, Vector3.one / 2);
            }
        }

        void Awake()
        {
            Singleton();
        }
    }
}