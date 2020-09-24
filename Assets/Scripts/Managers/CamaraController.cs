using UnityEngine;

namespace Scripts.Managers
{
    public class CamaraController : MonoBehaviour
    {
        [SerializeField]
        private float _panSensitivity, _zoomSensitivity, _xMax, _xMin, _zMax, _zMin, _zoomMax, _zoomMin;

        private Camera _camera;

        void Start()
        {
            _camera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            float zoomInput = Input.GetAxis("Mouse ScrollWheel");

            TranslateCamera(horizontalInput, verticalInput);
            ZoomCamera(zoomInput);
        }

        private void TranslateCamera(float horizontalInput, float verticalInput)
        {
            transform.Translate(new Vector3(verticalInput, 0, -horizontalInput) * _panSensitivity * Time.deltaTime, Space.Self);

            var clampPos = transform.position;
            clampPos.x = Mathf.Clamp(clampPos.x, _xMin, _xMax);
            clampPos.z = Mathf.Clamp(clampPos.z, _zMin, _zMax);

            transform.position = clampPos;
        }

        private void ZoomCamera(float zoomInput)
        {
            var zoom = _camera.fieldOfView - (zoomInput * _zoomSensitivity);
            _camera.fieldOfView = Mathf.Clamp(zoom, _zoomMin, _zoomMax);
        }
    }
}
