using DFLite.Helpers;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace DFLite.Controllers
{
    public class CameraController : MonoBehaviour
    {
        public const float PixelPerUnit = 32;
        public float zoomDesired { get; protected set; }
        public float ZoomMin { get; protected set; }
        public float ZoomMax { get; protected set; }

        //public float zoom => (Screen.height / (this.zoomDesired * CameraController.PIXEL_PER_UNIT) * 0.5f);
        public int Zoom => Mathf.FloorToInt(zoomDesired * PixelPerUnit);

        private PixelPerfectCamera _ppc;

        public float Sensitivity { get; protected set; }
        public Vector3 MousePosition { get; protected set; }

        public RectI viewRect;
        private Vector3 _lastMousePosition;
        private Camera _camera;

        private void Start() {
            this._camera = Camera.main;
            this.ZoomMin = .1f;
            this.ZoomMax = 10f;
            this.Sensitivity = 1f;
            this.zoomDesired = 1f;
            this._ppc = GetComponent<PixelPerfectCamera>();
        }

        private void Update() {
            if (Input.GetMouseButtonDown(2))
                _lastMousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            this.UpdateCamera();
        }

        private void UpdateCamera() {
            if (Input.GetMouseButton(2)) {
                MousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                var diff = this._lastMousePosition - this.MousePosition;

                if (Vector3.Distance(diff, Vector3.zero) > 0.05f) {
                    this._camera.transform.Translate(diff * .8f);
                    this.UpdateViewRect();
                }
            }

            this.zoomDesired += this.zoomDesired * Input.GetAxis("Mouse ScrollWheel") * this.Sensitivity;
            this.zoomDesired = Mathf.Clamp(this.zoomDesired, this.ZoomMin, this.ZoomMax);

            if (Zoom - _ppc.assetsPPU == 0) return;
            _ppc.assetsPPU = Mathf.FloorToInt(Zoom);
            this.UpdateViewRect();
        }

        private void UpdateViewRect() {
            // Creating local var for efficiency
            var position = _camera.transform.position;

            this.viewRect = new RectI(
                new Vector2Int(
                    Mathf.FloorToInt(position.x - _camera.orthographicSize * _camera.aspect),
                    Mathf.FloorToInt(position.y - _camera.orthographicSize)
                ), new Vector2Int(
                    Mathf.FloorToInt(position.x - _camera.orthographicSize * _camera.aspect),
                    Mathf.FloorToInt(position.y - _camera.orthographicSize)
                ));

            // Time to redraw the map!
            // TODO: Reference the overwatch unit and redraw map!
            //TestingGrid.Instance.gridVisual.UpdateMesh();
        }
    }
}
