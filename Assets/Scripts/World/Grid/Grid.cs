using System;
using System.Collections.Generic;
using UnityEngine;


namespace dflike.World.Grid
{
    public class Grid<TGridObject>
    {
        public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;

        public class OnGridObjectChangedEventArgs : EventArgs
        {
            public int x, y, z;
        }

        private readonly int _width, _height, _depth;
        private readonly float _cellSize;
        private readonly Vector3 _originPosition;

        /// <summary>
        /// We're storing each grid layer in _layers,
        /// the wanted grid layer is extracted from this.
        /// </summary>
        private readonly Dictionary<int, TGridObject[,]> _layers;

        public Grid(int width, int height, int depth, float cellSize, Vector3 originPosition,
            Func<Grid<TGridObject>, int, int, int, TGridObject> createGridObject) {
            this._width = width;
            this._height = height;
            this._depth = depth;
            this._cellSize = cellSize;
            this._originPosition = originPosition;

            _layers = new Dictionary<int, TGridObject[,]>();

            // Populate grid
            for (var z = 0; z < depth; z++) {
                var temp = new TGridObject[width, height];
                for (var x = 0; x < width; x++) {
                    for (var y = 0; y < height; y++) {
                        temp[x, y] = createGridObject(this, x, y, z);
                        Debug.Log(temp[x, y]);
                    }
                }

                _layers.Add(z, temp);

            }

            Debug.Log("Grid initialized");
        }

        public int GetWidth() {
            return _width;
        }

        public int GetHeight() {
            return _height;
        }

        public int GetDepth() {
            return _depth;
        }

        public float GetCellSize() {
            return _cellSize;
        }

        public Vector3 GetWorldPosition(int x, int y) {
            return new Vector3(x, y) * _cellSize + _originPosition;
        }

        public void GetXy(Vector3 worldPosition, out int x, out int y) {
            x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
            y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
        }

        public void GetXyz(Vector3 worldPosition, out int x, out int y, out int z) {
            x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
            y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
            z = Mathf.FloorToInt((worldPosition - _originPosition).z);
        }

        public void SetGridObject(int x, int y, int z, TGridObject value) {
            if (x < 0 || y < 0 || z < 0 || x >= _width || y >= _height || z >= _depth) return;

            _layers[z][x, y] = value;
            OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y, z = z });
        }

        public void TriggerGridObjectChanged(int x, int y, int z) {
            OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y, z = z });
        }

        public void SetGridObject(Vector3 worldPosition, TGridObject value) {
            GetXyz(worldPosition, out var x, out var y, out var z);
            SetGridObject(x, y, z, value);
        }

        public TGridObject GetGridObject(int x, int y, int z) {
            if (x >= 0 && y >= 0 && z >= 0 && x < _width && y < _height && z < _depth) {

                return _layers[z][x, y];
            } else {
                return default(TGridObject);
            }
        }

        public TGridObject GetGridObject(Vector3 worldPosition) {
            GetXyz(worldPosition, out var x, out var y, out var z);
            return GetGridObject(x, y, z);
        }

        public TGridObject[,] GetGridLayer(int z) {
            if (z >= 0 && z < _depth)
                return _layers[z];
            else {
                return null;
            }
        }
    }
}