using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

namespace Game.Scripts
{
    public class GridController : TickBehaviour
    {
        private readonly SpaceChunk[,] _dynamicChunks = new SpaceChunk[Constants.DynamicGridSize, Constants.DynamicGridSize];

        private Vector2Int _currentChunkCoords = default;

        public override void Enable()
        {
            base.Enable();

            _currentChunkCoords = GetCurrentChunkCoords();
            UpdateGrid();
        }

        public override void Tick()
        {
            base.Tick();

            UpdateState();
        }

        private void UpdateState()
        {
            var currentChunkPosition = GetCurrentChunkCoords();
            if (_currentChunkCoords.IsEqual(currentChunkPosition))
                return;

            _currentChunkCoords = currentChunkPosition;

            UpdateGrid();
        }

        private Vector2Int GetCurrentChunkCoords()
        {
            var roundedPositionX = Mathf.RoundToInt(GameManager.Instance.SpaceshipController.Position.x);
            var positionX =
                roundedPositionX >= 0
                    ? roundedPositionX / Constants.ChunkSize
                    : roundedPositionX / Constants.ChunkSize - 1;
            
            var roundedPositionY = Mathf.RoundToInt(GameManager.Instance.SpaceshipController.Position.y);
            var positionY =
                roundedPositionY >= 0
                    ? roundedPositionY / Constants.ChunkSize
                    : roundedPositionY / Constants.ChunkSize - 1;

            return new Vector2Int(positionX, positionY);
        }

        private void UpdateGrid()
        {
            //Profiler.BeginSample("UpdateCells");

            Parallel.For(0, Constants.DynamicGridSize, i =>
            {
                Parallel.For(0, Constants.DynamicGridSize, j =>
                {
                    if (_dynamicChunks[i, j] == null)
                    {
                        _dynamicChunks[i, j] = new SpaceChunk(i + _currentChunkCoords.x - Constants.DynamicGridCorrection, j + _currentChunkCoords.y - Constants.DynamicGridCorrection);
                    }
                    else
                    {
                        _dynamicChunks[i, j].Update(i + _currentChunkCoords.x - Constants.DynamicGridCorrection, j + _currentChunkCoords.y - Constants.DynamicGridCorrection);
                    }
                });
            });

            //Profiler.EndSample();
        }
    }
}
