//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using CodeMonkey.Utils;

//public class Testing : MonoBehaviour
//{
//    public int gridX = 5;
//    public int gridY = 5;
//    public float cellSize = 6f;

//    private Grid _grid;

//    private void Start()
//    {
//        _grid = new Grid(Vector3.zero, gridX, gridY, cellSize);
//    }

//    private void Update()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            _grid.SetValue((UtilsClass.GetMouseWorldPosition()), 15);
//        }
//    }
//}
