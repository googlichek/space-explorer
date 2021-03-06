using UnityEngine;

namespace Game.Scripts
{
    public class SpaceshipController : TickBehaviour
    {
        private Vector3 _position;

        private int _rating;

        public Vector3 Position => _position;

        public int Rating => _rating;

        public override void Init()
        {
            base.Init();

            priority = TickPriority.High;

            _position = transform.position;
            _rating = Random.Range(0, Constants.MaxRating);
        }

        public override void Enable()
        {
            base.Enable();

            GameManager.Instance.GridHUD.CreateSpaceshipIndicator();
        }

        public override void Tick()
        {
            base.CameraTick();

            if (GameManager.Instance.InputWrapper.IsMoveUpPressed)
            {
                Move(0, 1);
                Rotate(0);
            }

            if (GameManager.Instance.InputWrapper.IsMoveDownPressed)
            {
                Move(0, -1);
                Rotate(180);
            }
            
            if (GameManager.Instance.InputWrapper.IsMoveRightPressed)
            {
                Move(1, 0);
                Rotate(270);
            }

            if (GameManager.Instance.InputWrapper.IsMoveLeftPressed)
            {
                Move(-1, 0);
                Rotate(90);
            }

            transform.localScale =
                GameManager.Instance.CameraController.Camera.orthographicSize.IsEqual(Constants.OrtographicSizeMin)
                    ? Vector3.one
                    : Vector3.one * Mathf.RoundToInt(GameManager.Instance.CameraController.Camera.orthographicSize / Constants.OrtographicSizeMin) / 3;
        }

        private void Move(int x, int y)
        {
            _position.x = Mathf.RoundToInt(transform.position.x + x);
            _position.y = Mathf.RoundToInt(transform.position.y + y);
            _position.z = Mathf.RoundToInt(transform.position.z);

            transform.position = _position;
        }

        private void Rotate(int angle)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
