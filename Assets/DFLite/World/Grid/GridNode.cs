using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine;

namespace DFLite.World.Grid
{
    public class GridNode
    {
        private Grid<GridNode> _grid;

        private int _x, _y, _z;

        private bool _isWalkable;

        private Entity ent;

        public GridNode(Grid<GridNode> grid, int x, int y, int z)
        {
            this._grid = grid;
            this._x = x;
            this._y = y;
            this._z = z;
            //this.ent = Entity.Null;
            _isWalkable = true;
        }

        public bool IsWalkable()
        {
            return _isWalkable;
        }

        public void SetIsWalkable(bool isWalkable)
        {
            this._isWalkable = isWalkable;
            _grid.TriggerGridObjectChanged(_x, _y, _z);
        }

        public void SetEntity(Entity ent)
        {
            this.ent = ent;
            _grid.TriggerGridObjectChanged(_x, _y, _z);
        }

        public Entity GetEntity()
        {
            return ent;
        }
    }

}