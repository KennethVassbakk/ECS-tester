using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;
using dflike.World;
using dflike.World.Grid;
using Unity.Entities;



public class TestingGrid : MonoBehaviour
{
   
    public static TestingGrid Instance { private set; get;  }

    [SerializeField] public GridVisual GridVisual;
    public Grid<GridNode> pfGrid;

    public int CurrentLayer = 0;
    public bool ShowDebug = true;

    public int GridWidth = 2;
    public int GridHeight = 2;
    public int GridDepth = 1;
    public float CellSize = 16f;

    private bool runTest = false;

    //public TGridObject[,] CurrentGrid;
    private void Awake() {
        Instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        pfGrid = new Grid<GridNode>(GridWidth, GridHeight, GridDepth, CellSize, Vector3.zero, (Grid<GridNode> grid, int x, int y, int z) => new GridNode(grid, x, y, z));
        GridVisual.SetGrid(this.pfGrid);
    }

 
    private void Update()
    {

        if (!ShowDebug) return;

        var debugTextArray = new TextMesh[pfGrid.GetWidth(), pfGrid.GetHeight()];
        for (var x = 0; x < GridWidth; x++)
        {
            for (var y = 0; y < GridHeight; y++)
            {
                for (var z = 0; z < GridDepth; z++)
                {
                    debugTextArray[x, y] = UtilsClass.CreateWorldText((x, y, z).ToString(), null,
                        pfGrid.GetWorldPosition(x, y) + new Vector3(CellSize, CellSize) * .5f, (int) (CellSize * 2),
                        Color.white, TextAnchor.MiddleCenter);

                    Debug.DrawLine(pfGrid.GetWorldPosition(x, y), pfGrid.GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(pfGrid.GetWorldPosition(x, y), pfGrid.GetWorldPosition(x + 1, y), Color.white, 100f);

                  //  Debug.Log(message: Grid.GetGridObject(2,2,1).IsWalkable());

                }
            }
        }

        Debug.DrawLine(pfGrid.GetWorldPosition(0, GridHeight), pfGrid.GetWorldPosition(GridWidth, GridHeight), Color.white, 100f);
        Debug.DrawLine(pfGrid.GetWorldPosition(GridWidth, 0), pfGrid.GetWorldPosition(GridWidth, GridHeight), Color.white, 100f);
    }

}

