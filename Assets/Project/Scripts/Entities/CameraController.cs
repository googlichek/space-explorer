using UnityEngine;

namespace Game.Scripts
{
    public class CameraController : TickBehaviour
    {
        private Camera _camera = default;

        private Vector3 _position;

        public override void Init()
        {
            base.Init();

            _camera = GetComponent<Camera>();
        }

        public override void Enable()
        {
            base.Enable();

            _camera.orthographicSize = Constants.OrtographicSizeMin;
        }

        public override void Tick()
        {
            base.Tick();

            HandleInput();
        }

        public override void CameraTick()
        {
            base.CameraTick();

            UpdatePosition();
        }

        private void HandleInput()
        {
            if (GameManager.Instance.InputWrapper.IsZoomUpPressed)
            {
                _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize * Constants.OrtographicSizeStep, Constants.OrtographicSizeMin, Constants.OrtographicSizeMax);
            }

            if (GameManager.Instance.InputWrapper.IsZoomDownPressed)
            {
                _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize / Constants.OrtographicSizeStep, Constants.OrtographicSizeMin, Constants.OrtographicSizeMax);
            }
        }

        private void UpdatePosition()
        {
            _position.x = GameManager.Instance.SpaceshipController.transform.position.x;
            _position.y = GameManager.Instance.SpaceshipController.transform.position.y;
            _position.z = _camera.transform.position.z;

            transform.position = _position;
        }
    }
}
