using System;
using DFLite.Helpers;
using UnityEngine;

namespace DFLite.Controllers
{
    public class CameraController : MonoBehaviour
    {
        // ReSharper disable once InconsistentNaming
        private const int PIXEL_PER_UNIT = 32;
        private Camera _camera;
        private Vector3 _lastMousePosition;
        private Rect _bounds;
        private bool _updateViewRect;

        public RectI viewRect;
        public float zoomDesired;
        public float zoomMax = 30f;
        public float zoomMin = .4f;

        // Pixel Perfect?
        private float Zoom => Screen.height / (zoomDesired * PIXEL_PER_UNIT) * 0.5f;
        private float Sensitivity { get; set; }
        private Vector3 MousePosition { get; set; }

        private void Start()
        {
            _camera = Camera.main;
            Sensitivity = 1f;
            zoomDesired = 1f;

            _bounds.xMin = TestingGrid.Instance.pfGrid.OriginPosition().x;
            _bounds.xMax = TestingGrid.Instance.pfGrid.GetWidth() - 1;
            _bounds.yMin = TestingGrid.Instance.pfGrid.OriginPosition().y;
            _bounds.yMax = TestingGrid.Instance.pfGrid.GetHeight() - 1;
            UpdateViewRect();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(2))
                _lastMousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            UpdateCamera();

            if (_updateViewRect) UpdateViewRect();
        }

        private void UpdateCamera()
        {
            var mw = Input.GetAxis("Mouse ScrollWheel");
            zoomDesired += (zoomDesired > 0.5f ? zoomDesired : 0.51f) * mw * Sensitivity;
            zoomDesired = Mathf.Clamp(zoomDesired, zoomMin, zoomMax);
            zoomDesired = Mathf.Round(zoomDesired * 10f) / 10f;

            // Are We  zooming?
            if (Math.Abs(Zoom - _camera.orthographicSize) > 0.01f)
            {
                _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, Zoom, Time.deltaTime * 10f);
                _updateViewRect = true;
            }
            else if (Math.Abs(Zoom - _camera.orthographicSize) > 0f &&
                     Math.Abs(Zoom - _camera.orthographicSize) <= 0.01f)
            {
                _camera.orthographicSize = Zoom;
                _updateViewRect = true;
            }

            // Are we holding the 2nd mouse button down?
            if (!Input.GetMouseButton(2)) return;

            MousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            var diff = _lastMousePosition - MousePosition;

            // If the distance is miniscule, return
            if (!(Vector3.Distance(diff, Vector3.zero) > 0.01f)) return;
            Transform camTransform;
            (camTransform = _camera.transform).Translate(diff * 0.1f);

            var camPos = camTransform.position;

            // Limit camera movement
            camPos = new Vector3(Mathf.Clamp(camPos.x, _bounds.xMin, _bounds.xMax), Mathf.Clamp(camPos.y, _bounds.yMin, _bounds.yMax), camPos.z);
            _camera.transform.position = camPos;

            _updateViewRect = true;
        }

        private void UpdateViewRect()
        {
            _updateViewRect = false;

            // Creating local var for efficiency
            var position = _camera.transform.position;
            var currOrthoSize = Mathf.CeilToInt(_camera.orthographicSize);

            //  Undone: When we're doing chunks, we should probably add an additional chunk size on each side
            var orthoSize = currOrthoSize + Mathf.CeilToInt(currOrthoSize / 5f);

            viewRect = new RectI(
                new Vector2Int(
                    Mathf.FloorToInt(position.x - orthoSize * _camera.aspect),
                    Mathf.FloorToInt(position.y - orthoSize)
                ), new Vector2Int(
                    Mathf.FloorToInt(position.x + orthoSize * _camera.aspect),
                    Mathf.FloorToInt(position.y + orthoSize)
                ));

            // Time to redraw the map!
            // TODO: Reference the over-watch unit and redraw map!
            //TestingGrid.Instance.gridVisual.UpdateMesh();
        }
    }
}