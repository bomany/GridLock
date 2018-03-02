using UnityEngine;
using System.Collections;


namespace Grid {
    /// <summary>
    /// Position Representation for Grid System
    /// </summary>
    public class GridPosition
    {
        public int row;
        public int column;

        public GridPosition(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
    }
}
