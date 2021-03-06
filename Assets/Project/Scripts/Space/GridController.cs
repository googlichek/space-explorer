using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;
using Random = UnityEngine.Random;

namespace Game.Scripts
{
    public class GridController : TickBehaviour
    {
        [SerializeField] private PlanetController _planetTemplate = default;

        private readonly List<SpaceCellRatingDifferencePair> _closestPlanets = new List<SpaceCellRatingDifferencePair>();

        private readonly SpaceChunk[,] _dynamicChunks = new SpaceChunk[Constants.DynamicGridSize, Constants.DynamicGridSize];

        private readonly List<IResource> _planetsOnScreen = new List<IResource>();

        private Task _updateGridTask;
        private Task _updateClosestPlanetsTask;

        private Vector2Int _currentChunkCoords = Vector2Int.down;
        private Vector2Int _currentPlayerCoords = Vector2Int.down;

        private Vector3 _visibleBoundsSize;
        private Vector3 _visibleBoundsCenter;
        private Vector3 _visibleBoundsCheckPoint;
        private Bounds _visibleBounds;

        private int _seed;
        private int _currentVisibleHeight;

        private bool _shouldUpdateClosestPlanets = false;
        private bool _shouldRedrawClosestPlanets = false;

        public Vector2Int CurrentChunkCoords => _currentChunkCoords;
        public Vector2Int CurrentPlayerCoords => _currentPlayerCoords;

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
            UpdatePlayerCoords();
            UpdateCurrentCellHeight();
            UpdateGrid();
            UpdateVisiblePlanets();
            RedrawPlanets();
        }

        private void UpdateGrid()
        {
            var chunkCoords = GetCurrentChunkCoords();
            if ((_updateGridTask == null || _updateGridTask.IsCompleted) &&
                !_currentChunkCoords.IsEqual(chunkCoords))
            {
                _currentChunkCoords = chunkCoords;

                UpdateGridTask().WrapErrors();
            }
        }

        private void UpdatePlayerCoords()
        {
            var playerCoords = GetPlayerCoords();
            if (!_currentPlayerCoords.IsEqual(playerCoords))
            {
                _currentPlayerCoords = playerCoords;
                _shouldUpdateClosestPlanets = true;
            }
        }

        private Vector2Int GetPlayerCoords()
        {
            var playerCoordsX = Mathf.RoundToInt(GameManager.Instance.SpaceshipController.Position.x);
            var playerCoordsY = Mathf.RoundToInt(GameManager.Instance.SpaceshipController.Position.y);

            return new Vector2Int(playerCoordsX, playerCoordsY);
        }

        private void UpdateCurrentCellHeight()
        {
            var currentHeight = GameManager.Instance.CameraController.Height;
            if (_currentVisibleHeight != currentHeight)
            {
                _currentVisibleHeight = currentHeight;
                _shouldUpdateClosestPlanets = true;
            }
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

        private async Task UpdateGridTask()
        {
            //Profiler.BeginSample("UpdateCells");

            _updateGridTask =
                Task
                    .Run(() =>
                    {
                        Parallel.For(0, Constants.DynamicGridSize, i =>
                        {
                            Parallel.For(0, Constants.DynamicGridSize, j =>
                            {
                                UpdateChunk(i, j);
                            });
                        });
                    })
                    .ContinueWith(dirtyFlagTask => { _shouldUpdateClosestPlanets = true; });

            await _updateGridTask;

            //Profiler.EndSample();
        }

        private void UpdateChunk(int x, int y)
        {
            if (_dynamicChunks[x, y] == null)
            {
                _dynamicChunks[x, y] = new SpaceChunk(_seed, x + _currentChunkCoords.x - Constants.DynamicGridCorrection, y + _currentChunkCoords.y - Constants.DynamicGridCorrection);
            }
            else
            {
                _dynamicChunks[x, y].Update(x + _currentChunkCoords.x - Constants.DynamicGridCorrection, y + _currentChunkCoords.y - Constants.DynamicGridCorrection);
            }
        }

        private void UpdateVisiblePlanets()
        {
            if ((_updateClosestPlanetsTask == null || _updateClosestPlanetsTask.IsCompleted) &&
                _shouldUpdateClosestPlanets)
            {
                _shouldUpdateClosestPlanets = false;

                UpdateVisibleBounds();
                UpdateClosestPlanetsTask().WrapErrors();
            }
        }

        private void UpdateVisibleBounds()
        {
            _visibleBoundsSize.x = GameManager.Instance.CameraController.Height;
            _visibleBoundsSize.y = GameManager.Instance.CameraController.Height;

            _visibleBoundsCenter.x = _currentPlayerCoords.x;
            _visibleBoundsCenter.y = _currentPlayerCoords.y;

            _visibleBounds.size = _visibleBoundsSize;
            _visibleBounds.center = _visibleBoundsCenter;
        }

        private async Task UpdateClosestPlanetsTask()
        {
            //Profiler.BeginSample("UpdateClosestPlanets");

            _closestPlanets.Clear();
            _updateClosestPlanetsTask =
                Task
                    .Run(UpdateClosestPlanets)
                    .ContinueWith(dirtyFlagTask => { _shouldRedrawClosestPlanets = true; });

            await _updateClosestPlanetsTask;

            //Profiler.EndSample();
        }

        private void UpdateClosestPlanets()
        {
            var index = 0;
            foreach (var chunk in _dynamicChunks)
            {
                foreach (var cell in chunk.DynamicCells)
                {
                    _visibleBoundsCheckPoint.x = cell.Coords.x;
                    _visibleBoundsCheckPoint.y = cell.Coords.y;

                    if (cell.HasPlanet && _visibleBounds.Contains(_visibleBoundsCheckPoint))
                    {
                        var difference = Mathf.Abs(cell.Rating - GameManager.Instance.SpaceshipController.Rating);

                        if (index >= Constants.MaxPlanets)
                        {
                            if (difference > _closestPlanets[0].RatingDifference)
                                continue;

                            _closestPlanets.RemoveAt(0);
                            _closestPlanets.Add(new SpaceCellRatingDifferencePair(difference, cell));
                            _closestPlanets.Sort((x, y) => x.RatingDifference.CompareTo(y.RatingDifference));
                            _closestPlanets.Reverse();
                        }
                        else
                        {
                            _closestPlanets.Add(new SpaceCellRatingDifferencePair(difference, cell));
                        }

                        index++;
                    }
                }
            }
        }

        private void RedrawPlanets()
        {
            if (_shouldRedrawClosestPlanets)
            {
                _shouldRedrawClosestPlanets = false;

                foreach (var planet in _planetsOnScreen)
                {
                    GameManager.Instance.PoolManager.Despawn(planet);
                }
                _planetsOnScreen.Clear();

                GameManager.Instance.GridHUD.ClearPlanetIndicators();

                for (var i = 0; i < _closestPlanets.Count; i++)
                {
                    var cell = _closestPlanets[i].Cell;
                    var planet = GameManager.Instance.PoolManager.Spawn(_planetTemplate, null, Vector3.zero, Quaternion.identity).GameObject.GetComponent<PlanetController>();
                    planet.Setup(cell.Coords.x, cell.Coords.y);

                    _planetsOnScreen.Add(planet);
                    GameManager.Instance.GridHUD.SpawnPlanetIndicator(cell.Coords.x, cell.Coords.y, cell.Rating);
                }
            }
        }
    }
}
