using DFLite.Helpers;
using System;
using UnityEngine;

namespace DFLite.Controllers
{
    public class CameraController : MonoBehaviour
    {
        // ReSharper disable once InconsistentNaming
        public const float PIXEL_PER_UNIT = 32;
        public float ZoomDesired { get; protected set; }
        public float ZoomMin { get; protected set; }
        public float ZoomMax { get; protected set; }

        public float Zoom => (Screen.height / (this.ZoomDesired * CameraController.PIXEL_PER_UNIT) * 0.5f);


        public float Sensitivity { get; protected set; }
        public Vector3 MousePosition { get; protected set; }

        public RectI viewRect;
        private Vector3 _lastMousePosition;
        private Camera _camera;
        private bool _updateViewRect;


        private void Start() {
            this._camera = Camera.main;
            this.ZoomMin = .1f;
            this.ZoomMax = 30f;
            this.Sensitivity = 1f;
            this.ZoomDesired = 1f;
            UpdateViewRect();
        }

        private void Update() {
            if (Input.GetMouseButtonDown(2))
                _lastMousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            this.UpdateCamera();

            if (_updateViewRect) {
                UpdateViewRect();
            }
        }

        private void UpdateCamera() {
            this.ZoomDesired += this.ZoomDesired * Input.GetAxis("Mouse ScrollWheel") * this.Sensitivity;
            this.ZoomDesired = Mathf.Clamp(this.ZoomDesired, this.ZoomMin, this.ZoomMax);

            if (Math.Abs(Zoom - _camera.orthographicSize) > 0.01f) {
                this._camera.orthographicSize = this.Zoom;
                _updateViewRect = true;

            }

            if (!Input.GetMouseButton(2)) return;
            MousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            var diff = this._lastMousePosition - this.MousePosition;

            if (!(Vector3.Distance(diff, Vector3.zero) > 0.01f)) return;
            _camera.transform.Translate(diff * .8f);

            var limitedPos = _camera.transform.position;
            var transform1 = _camera.transform;
            
            if (transform1.position.x < TestingGrid.Instance.pfGrid.OriginPosition().x)
                limitedPos.x = TestingGrid.Instance.pfGrid.OriginPosition().x;
            
            if (transform1.position.x > (TestingGrid.Instance.pfGrid.OriginPosition().x + TestingGrid.Instance.pfGrid.GetWidth()))
                limitedPos.x = TestingGrid.Instance.pfGrid.OriginPosition().x + TestingGrid.Instance.pfGrid.GetWidth();
            
            if (transform1.position.y < TestingGrid.Instance.pfGrid.OriginPosition().x)
                limitedPos.y = TestingGrid.Instance.pfGrid.OriginPosition().x;
            
            if (transform1.position.y > (TestingGrid.Instance.pfGrid.OriginPosition().y + TestingGrid.Instance.pfGrid.GetHeight()) )
                limitedPos.y = TestingGrid.Instance.pfGrid.OriginPosition().y + TestingGrid.Instance.pfGrid.GetHeight();
            
            transform1.position = new Vector3(limitedPos.x, limitedPos.y, transform1.position.z);
            
            _updateViewRect = true;
        }

        private void UpdateViewRect() {
            _updateViewRect = false;
            // Creating local var for efficiency
            var position = _camera.transform.position;
            var currOrthoSize = Mathf.CeilToInt(_camera.orthographicSize);

            //  Undone: When we're doing chunks, we should probably add an additional chunk size on each side
            var orthoSize = currOrthoSize + Mathf.CeilToInt(currOrthoSize / 5f);

            this.viewRect = new RectI(
                new Vector2Int(
                    Mathf.FloorToInt(position.x - orthoSize * _camera.aspect),
                    Mathf.FloorToInt(position.y - orthoSize)
                ), new Vector2Int(
                    Mathf.FloorToInt(position.x + orthoSize * _camera.aspect),
                    Mathf.FloorToInt(position.y + orthoSize)
                ));

            // Time to redraw the map!
            // TODO: Reference the overwatch unit and redraw map!
            //TestingGrid.Instance.gridVisual.UpdateMesh();
        }
    }
}
