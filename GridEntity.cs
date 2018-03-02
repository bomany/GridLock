using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Grid
{
    /// <summary>
    /// Represents an Entity in a Grid Block.
    /// </summary>
    public class GridEntity : MonoBehaviour
    {
        GridBlock _block;
        public GridBlock block { get { return _block; } }
        public GridPosition position { get { return block.gridPosition; } }

        public bool isStaticBlocker = false;
        public bool isMovingBlocker = false;

        List<GridBlock> _grid;
        List<GridBlock> grid
        {
            get
            {
                if (_grid == null)
                {
                    _grid = GridManager.Instance.grid.Cast<GridBlock>().ToList();
                }
                return _grid;
            }
        }

        public void GetClosestBlock()
        {
            GridBlock block;

            if (isStaticBlocker)
            {
                block = grid.OrderBy(x => Vector3.Distance(transform.position, x.position)).First();
            }
            else
            {
                block = grid.Where(x => x.isBlocked == false).OrderBy(x => Vector3.Distance(transform.position, x.position)).First();
            }

            SetBlock(block);
        }

        public void SetBlock(GridBlock block)
        {
            if (this.block != null)
            {
                this.block.RemoveEntity(this);
            }

            _block = block;
            this.block.SetEntity(this);

            if (isStaticBlocker || isMovingBlocker)
            {
                this.block.isBlocked = true;
            }
        }

        void CheckBlock()
        {
            if (!(isStaticBlocker || isMovingBlocker) && block.isBlocked)
            {
                GetClosestBlock();
            }
        }

        void Awake()
        {
            GetClosestBlock();
        }

        void OnDestroy()
        {
            block.RemoveEntity(this);
            _block = null;
        }

        void Update()
        {
            Debug.DrawLine(transform.position, block.position);
        }
    }
}