﻿using System;
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
            // This creates a TGridObject[,] for each of the layers (depth)
            // it the inputs the generated TGridObject[,] into the _Layers(dictionary)
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

        /// <summary>
        /// Get the X dimension length of the grid.
        /// </summary>
        /// <returns>int Width</returns>
        public int GetWidth() {
            return _width;
        }

        /// <summary>
        /// Get the Y dimension length of the grid.
        /// </summary>
        /// <returns>int Height</returns>
        public int GetHeight() {
            return _height;
        }

        /// <summary>
        /// Get the Z dimension length of the grid.
        /// This is the amount of layers that are present in the grid.
        /// </summary>
        /// <returns>int Z</returns>
        public int GetDepth() {
            return _depth;
        }

        /// <summary>
        /// Get the cell size of the grid items.
        /// </summary>
        /// <returns>int CellSize</returns>
        public float GetCellSize() {
            return _cellSize;
        }

        /// <summary>
        /// Get the World Position based on grid location.
        /// </summary>
        /// <param name="x">int X Location</param>
        /// <param name="y">int Y location</param>
        /// <returns>Vector 3 world location without Z</returns>
        public Vector3 GetWorldPosition(int x, int y) {
            return new Vector3(x, y) * _cellSize + _originPosition;
        }

        /// <summary>
        /// Get the World Position based on grid location (incl. z)
        /// </summary>
        /// <param name="location">Vector3 Location</param>
        /// <returns>Vector3 Grid Location with Z</returns>
        public Vector3 GetWorldPosition(Vector3 location)
        {
            return (location * _cellSize + _originPosition);
        }

        /// <summary>
        /// Helper Function: Get XY location of a grid element based on a Vector3 (no Z)
        /// </summary>
        /// <param name="worldPosition">Vector3 WorldPosition</param>
        /// <param name="x">x is returned from the function</param>
        /// <param name="y">y is returned from the function</param>
        public void GetXy(Vector3 worldPosition, out int x, out int y) {
            x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
            y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
        }

        /// <summary>
        /// Helper function: Get XYZ location of a grid element based on a Vector3 (incl Z)
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <param name="x">x is returned from the function</param>
        /// <param name="y">y is returned from the function</param>
        /// <param name="z">z is returned from the function</param>
        public void GetXyz(Vector3 worldPosition, out int x, out int y, out int z) {
            x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
            y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
            z = Mathf.FloorToInt((worldPosition - _originPosition).z);
        }

        /// <summary>
        /// Set a grid object to a value
        /// </summary>
        /// <param name="x">int x Location</param>
        /// <param name="y">int y Location</param>
        /// <param name="z">int z Location</param>
        /// <param name="value">Value</param>
        public void SetGridObject(int x, int y, int z, TGridObject value) {
            if (x < 0 || y < 0 || z < 0 || x >= _width || y >= _height || z >= _depth) return;

            _layers[z][x, y] = value;
            OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y, z = z });
        }

        /// <summary>
        /// Set a grid object to a value based on a Vector3
        /// </summary>
        /// <param name="worldPosition">Vector3 WorldPosition</param>
        /// <param name="value">value</param>
        public void SetGridObject(Vector3 worldPosition, TGridObject value) {
            GetXyz(worldPosition, out var x, out var y, out var z);
            SetGridObject(x, y, z, value);
        }

        /// <summary>
        /// Get a grid Object based on XYZ
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns>The object in the cell, or empty by default</returns>
        public TGridObject GetGridObject(int x, int y, int z) {
            if (x >= 0 && y >= 0 && z >= 0 && x < _width && y < _height && z < _depth) {
                return _layers[z][x, y];
            }
            else 
            {
                return default(TGridObject);
            }
        }

        /// <summary>
        /// Get Grid object based on Vector3
        /// </summary>
        /// <param name="worldPosition">Vector3 World Position</param>
        /// <returns>The Object in the cell, or empty by default</returns>
        public TGridObject GetGridObject(Vector3 worldPosition) {
            if (worldPosition.x >= 0 && worldPosition.y >= 0 && worldPosition.z >= 0 && worldPosition.x < _width &&
                worldPosition.y < _height && worldPosition.z < _depth)
            {
                GetXyz(worldPosition, out var x, out var y, out var z);
                return GetGridObject(x, y, z);
            }
            else
            {
                return default(TGridObject);
            }
        }

        /// <summary>
        /// Return a TGridObject[,] based on the defined Z layer
        /// </summary>
        /// <param name="z"></param>
        /// <returns>The grid at the set depth(z)</returns>
        public TGridObject[,] GetGridLayer(int z) {
            if (z >= 0 && z < _depth && _layers.ContainsKey(z))
                return _layers[z];
            else {
                return null;
            }
        }

        /// <summary>
        /// Used to trigger an object change.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void TriggerGridObjectChanged(int x, int y, int z) {
            OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y, z = z });
        }
    }
}