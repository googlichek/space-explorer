using UnityEngine;

namespace Game.Scripts
{
    public class PlanetController : TickBehaviour, IResource
    {
        [SerializeField] private ResourceType _type = ResourceType.None;

        private Vector3 _position;

        public GameObject GameObject => gameObject;
        public ResourceType Type => _type;

        public void Setup(int x, int y)
        {
            _position.x = x;
            _position.y = y;
            _position.z = 0;

            transform.position = _position;

            transform.localScale =
                GameManager.Instance.CameraController.Camera.orthographicSize.IsEqual(Constants.OrtographicSizeMin)
                    ? Vector3.one
                    : Vector3.one * Mathf.RoundToInt(GameManager.Instance.CameraController.Camera.orthographicSize / Constants.OrtographicSizeMin) / 3;
        }
    }
}
