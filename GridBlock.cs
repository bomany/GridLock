using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Grid
{
    /// <summary>
    /// Representation of a Grid block.
    /// </summary>
    public class GridBlock
    {

        // Position in world space
        Vector3 _position;
        public Vector3 position { get { return _position; } }

        // Position in Grid Space
        GridPosition _gridPosition;
        public GridPosition gridPosition { get { return _gridPosition; } }

        // Entities inside block
        List<GridEntity> _entities = new List<GridEntity>();
        public List<GridEntity> entities
        {
            get
            {
                _entities.RemoveAll(x => x == null);
                return _entities;
            }
        }

        // If block is ocupied.
        public bool isBlocked = false;

        public GridBlock(GridPosition gridPosition, Vector3 position)
        {
            this._gridPosition = gridPosition;
            this._position = position;
        }

        public GridEntity EntityBlocking()
        {
            return entities.Where(x => x.isStaticBlocker || x.isMovingBlocker).FirstOrDefault();
        }

        //Check if any entities blocks this block.
        void SetBlocked()
        {
            isBlocked = false;
            if (entities.Where(x => x.isStaticBlocker || x.isMovingBlocker).ToList().Count > 0)
            {
                isBlocked = true;
            }
        }

        //Sets an Entity.
        public void SetEntity(GridEntity entity)
        {
            _entities.Add(entity);
            SetBlocked();
        }

        //Removes and Entity.
        public void RemoveEntity(GridEntity entity)
        {
            _entities.RemoveAll(x => x.GetInstanceID() == entity.GetInstanceID());
            SetBlocked();
        }


        public List<GridEntity> GetObjectWithTags(string tag)
        {
            return entities.Where(x => x.tag == tag).ToList();
        }

        public bool EntityInBlock(GridEntity entity)
        {
            if (entities.Contains(entity))
            {
                return true;
            }
            return false;
        }
    }
}