using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts
{
    public class GridHUD : TickBehaviour
    {
        private const string ChunkCoordsText = "Chunk Coords: ";
        private const string PlayerCoordsText = "Player Coords: ";

        [SerializeField] private Text _chunkCoordsText = default;
        [SerializeField] private Text _playerCoordsText = default;

        [Space]

        [SerializeField] private PlanetIndicatorUIController _planetIndicatorTemplate = default;

        private readonly List<PlanetIndicatorUIController> _planetIndicators = new List<PlanetIndicatorUIController>();

        private PlanetIndicatorUIController _spaceshipIndicator;

        public override void CameraTick()
        {
            base.CameraTick();

            UpdateHelperTexts();
            UpdateSpaceshipPosition();
        }

        public void CreateSpaceshipIndicator()
        {
            _spaceshipIndicator =
                GameManager
                    .Instance
                    .PoolManager
                    .Spawn(
                        _planetIndicatorTemplate,
                        transform,
                        GameManager.Instance.SpaceshipController.Position,
                        Quaternion.identity)
                    .GameObject
                    .GetComponent<PlanetIndicatorUIController>();

            _spaceshipIndicator.Setup(GameManager.Instance.SpaceshipController.Rating);
        }

        public void ClearPlanetIndicators()
        {
            foreach (var indicator in _planetIndicators)
            {
                GameManager.Instance.PoolManager.Despawn(indicator);
            }

            _planetIndicators.Clear();
        }

        public void SpawnPlanetIndicator(int x, int y, int rating)
        {
            var indicator =
                GameManager
                    .Instance
                    .PoolManager
                    .Spawn(
                        _planetIndicatorTemplate,
                        transform,
                        Vector3.zero,
                        Quaternion.identity)
                    .GameObject
                    .GetComponent<PlanetIndicatorUIController>();

            indicator.Setup(rating);

            var coord = new Vector3(x, y, 0);
            indicator.UpdatePosition(coord);

            _planetIndicators.Add(indicator);
        }

        private void UpdateHelperTexts()
        {
            _chunkCoordsText.text =
                string.Format($"{ChunkCoordsText}{GameManager.Instance.GridController.CurrentChunkCoords}");

            _playerCoordsText.text =
                string.Format($"{PlayerCoordsText}{GameManager.Instance.GridController.CurrentPlayerCoords}");
        }

        private void UpdateSpaceshipPosition()
        {
            if (_spaceshipIndicator == null)
                return;

            _spaceshipIndicator.UpdatePosition(GameManager.Instance.SpaceshipController.Position);
        }
    }
}
