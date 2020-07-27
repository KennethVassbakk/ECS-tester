using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Color = UnityEngine.Color;

public class Grid {
    private Vector3 _origin;
    private int _width;
    private int _height;
    private float _cellSize;
    private int[,] _gridArray;
    private TextMesh[,] _debugTextArray;

    public Grid(Vector3 origin, int width, int height, float cellSize)
    {
        this._origin = origin;
        this._width = width;
        this._height = height;
        this._cellSize = cellSize;

        _gridArray = new int[width, height];
        _debugTextArray = new TextMesh[width, height];
        
        for (var x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (var y = 0; y < _gridArray.GetLength(1); y++)
            {
                _debugTextArray[x,y] = UtilsClass.CreateWorldText(_gridArray[x, y].ToString(), null, GetWorldPosition(x,y) + new Vector3(cellSize, cellSize) * 0.5f, ((int)this._cellSize * 4), Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x,y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width,height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

        SetValue(2, 1, 56);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * this._cellSize + this._origin;
    }

    public void SetValue(int x, int y, int value)
    {
        if (x < 0 || y < 0 || x >= _width || y >= _height) return;
        _gridArray[x, y] = value;
        _debugTextArray[x, y].text = _gridArray[x, y].ToString();
    }

    private void GetXy(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - this._origin).x / _cellSize);
        y = Mathf.FloorToInt((worldPosition - this._origin).y / _cellSize);
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXy(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public int GetValue(int x, int y)
    {
        if (x < 0 || y < 0 || x >= _width || y >= _height) return -1;
        return _gridArray[x, y];
    }

    public int GetValue(Vector3 worldPosition)
    {
        GetXy(worldPosition, out var x, out var y);
        return GetValue(x, y);
    }
}
