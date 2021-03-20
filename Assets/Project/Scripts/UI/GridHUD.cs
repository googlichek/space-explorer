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

        public override void Tick()
        {
            base.Tick();

            _chunkCoordsText.text =
                string.Format($"{ChunkCoordsText}{GameManager.Instance.GridController.CurrentChunkCoords}");

            _playerCoordsText.text =
                string.Format($"{PlayerCoordsText}{GameManager.Instance.GridController.CurrentPlayerCoords}");
        }
    }
}
