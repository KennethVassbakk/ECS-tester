using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
    private Grid _grid;

    private void Start()
    {
        _grid = new Grid(4, 8, 6f);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _grid.SetValue((UtilsClass.GetMouseWorldPosition()), 15);
        }
    }
}
