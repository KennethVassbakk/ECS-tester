using CodeMonkey.Utils;
using DFLite.World.Grid;
using UnityEngine;

public class TestingGrid : MonoBehaviour
{

    public static TestingGrid Instance { private set; get; }

    [SerializeField] public GridVisual gridVisual;
    public Grid<GridNode> pfGrid;

    public int currentLayer;
    public bool showDebug = true;

    public int gridWidth = 2;
    public int gridHeight = 2;
    public int gridDepth = 1;
    public float cellSize = 1f;

    //public TGridObject[,] CurrentGrid;
    private void Awake() {
        Instance = this;
    }
    
    // Start is called before the first frame update
    private void Start() {
        pfGrid = new Grid<GridNode>(gridWidth, gridHeight, gridDepth, cellSize, Vector3.zero, (grid, x, y, z) => new GridNode(grid, x, y, z));
        gridVisual.SetGrid(this.pfGrid);
    }


    private void Update() {

        if (!showDebug) return;

        var debugTextArray = new TextMesh[pfGrid.GetWidth(), pfGrid.GetHeight()];
        for (var x = 0; x < gridWidth; x++) {
            for (var y = 0; y < gridHeight; y++) {
                for (var z = 0; z < gridDepth; z++) {
                    debugTextArray[x, y] = UtilsClass.CreateWorldText((x, y, z).ToString(), null,
                        pfGrid.GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, (int)(cellSize * 2),
                        Color.white, TextAnchor.MiddleCenter);

                    Debug.DrawLine(pfGrid.GetWorldPosition(x, y), pfGrid.GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(pfGrid.GetWorldPosition(x, y), pfGrid.GetWorldPosition(x + 1, y), Color.white, 100f);

                }
            }
        }

        Debug.DrawLine(pfGrid.GetWorldPosition(0, gridHeight), pfGrid.GetWorldPosition(gridWidth, gridHeight), Color.white, 100f);
        Debug.DrawLine(pfGrid.GetWorldPosition(gridWidth, 0), pfGrid.GetWorldPosition(gridWidth, gridHeight), Color.white, 100f);
    }
    

}

