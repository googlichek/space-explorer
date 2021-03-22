using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts
{
    public class PlanetIndicatorUIController : TickBehaviour, IResource
    {
        [SerializeField] private ResourceType _type = ResourceType.None;

        [Space]

        [SerializeField] private Text _ratingText = default;

        private Vector3 _position;

        public GameObject GameObject => gameObject;
        public ResourceType Type => _type;

        public void Setup(int rating)
        {
            _ratingText.text = rating.ToString();
        }

        public void UpdatePosition(Vector3 coord)
        {
            var position = GameManager.Instance.CameraController.Camera.WorldToScreenPoint(coord);
            transform.position = position;
        }
    }
}
