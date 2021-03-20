using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Scripts
{
    public class GridController : TickBehaviour
    {
        private static readonly List<SpaceCell> _closestPlanets = new List<SpaceCell>();

        private readonly SpaceChunk[,] _dynamicChunks = new SpaceChunk[Constants.DynamicGridSize, Constants.DynamicGridSize];

        private Vector2Int _currentChunkCoords = Vector2Int.down;
        private Vector2Int _currentPlayerCoords = Vector2Int.down;

        public Vector2Int CurrentChunkCoords => _currentChunkCoords;
        public Vector2Int CurrentPlayerCoords => _currentPlayerCoords;

        private int _seed;

        public override void Enable()
        {
            base.Enable();

            _seed = Random.Range(0, Constants.MaxRating);
        }

        public override void Tick()
        {
            base.Tick();

            UpdateState();
        }

        private void UpdateState()
        {
            var playerCoords = GetPlayerCoords();
            if (!_currentPlayerCoords.IsEqual(playerCoords))
            {
                _currentPlayerCoords = playerCoords;
            }

            var chunkCoords = GetCurrentChunkCoords();
            if (!_currentChunkCoords.IsEqual(chunkCoords))
            {
                _currentChunkCoords = chunkCoords;

                UpdateGrid();
            }
        }

        private Vector2Int GetPlayerCoords()
        {
            var playerCoordsX = Mathf.RoundToInt(GameManager.Instance.SpaceshipController.Position.x);
            var playerCoordsY = Mathf.RoundToInt(GameManager.Instance.SpaceshipController.Position.y);

            return new Vector2Int(playerCoordsX, playerCoordsY);
        }

        private Vector2Int GetCurrentChunkCoords()
        {
            var positionX =
                _currentPlayerCoords.x >= 0
                    ? _currentPlayerCoords.x / Constants.DynamicChunkSize
                    : _currentPlayerCoords.x / Constants.DynamicChunkSize - 1;
            
            var positionY =
                _currentPlayerCoords.y >= 0
                    ? _currentPlayerCoords.y / Constants.DynamicChunkSize
                    : _currentPlayerCoords.y / Constants.DynamicChunkSize - 1;

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
                        _dynamicChunks[i, j] = new SpaceChunk(_seed, i + _currentChunkCoords.x - Constants.DynamicGridCorrection, j + _currentChunkCoords.y - Constants.DynamicGridCorrection);
                    }
                    else
                    {
                        _dynamicChunks[i, j].Update(i + _currentChunkCoords.x - Constants.DynamicGridCorrection, j + _currentChunkCoords.y - Constants.DynamicGridCorrection);
                    }
                });
            });

            //Profiler.EndSample();
        }

        private void UpdateClosestPlanets()
        {
            _closestPlanets.Clear();

            foreach (var chunk in _dynamicChunks)
            {
                foreach (var cell in chunk.DynamicCells)
                {
                    if (cell.Rating > 0)
                        _closestPlanets.Add(cell);
                }
            }
        }
    }
}
