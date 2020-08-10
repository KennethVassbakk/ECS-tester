using UnityEngine;
using DFLite.Helpers;

namespace DFLite.World.Grid
{
    public class GridVisual : MonoBehaviour
    {
        private Grid<GridNode> _grid;
        private static Mesh _mesh;
        private bool _updateMesh;

        private static int _currentDepthIndex;

        private void Awake()
        {
            _mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = _mesh;
            _currentDepthIndex = 0;
        }

        public void SetGrid(Grid<GridNode> grid)
        {
            this._grid = grid;
            _updateMesh = true;

            grid.OnGridObjectChanged += Grid_OnGridValueChanged;
        }

        private void Grid_OnGridValueChanged(object sender, Grid<GridNode>.OnGridObjectChangedEventArgs e)
        {
            _updateMesh = true;
        }

        public void UpdateMesh()
        {
            _updateMesh = true;
        }

        private void LateUpdate() {
            if (!_updateMesh) return;

            _updateMesh = false;
            UpdateVisual();
        }

        private void UpdateVisual() {

            // UNDONE: We should check if the current visuals are still inside the camera.
            // If they are inside the camera view, we can leave them as is.
            // if they are not inside the camera view, we can eliminate them.
            //
            // We should only draw clusters that are inside the camera view.


            MeshUtils.CreateEmptyMeshArrays(_grid.GetWidth() * _grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

            for (int x = 0; x < _grid.GetWidth(); x++) {
                for (int y = 0; y < _grid.GetHeight(); y++) {

                    int index = x * _grid.GetHeight() + y;
                    Vector3 quadSize = new Vector3(1, 1) * _grid.GetCellSize();

                    GridNode gridNode = _grid.GetGridObject(x, y, _currentDepthIndex);


                    Vector2 uv00 = new Vector2(0, 0);
                    Vector2 uv11 = new Vector2(0.5f, 0.5f);

                    if (!gridNode.IsWalkable()) {
                        uv00 = new Vector2(.5f, .5f);
                        uv11 = new Vector2(1f, 1f);
                    }

                    MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, _grid.GetWorldPosition(x, y) + quadSize * .0f, 0f, quadSize, uv00, uv11);
                }
            }

            _mesh.vertices = vertices;
            _mesh.uv = uv;
            _mesh.triangles = triangles;
            Debug.Log(_mesh.vertexCount);
        }
    }

}
