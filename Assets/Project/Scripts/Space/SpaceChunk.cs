using System.Threading.Tasks;
using UnityEngine;

namespace Game.Scripts
{
    public class SpaceChunk
    {
        private readonly SpaceCell[,] _dynamicCells = new SpaceCell[Constants.DynamicChunkSize, Constants.DynamicChunkSize];

        private Vector2Int _coords;

        public SpaceCell[,] DynamicCells => _dynamicCells;

        public Vector2Int Coords => _coords;

        private int _seed;

        public SpaceChunk(int seed, int x, int y)
        {
            _seed = seed;

            Update(x, y);
        }

        public void Update(int x, int y)
        {
            _coords.x = x;
            _coords.y = y;

            Parallel.For(0, Constants.DynamicChunkSize, i =>
            {
                Parallel.For(0, Constants.DynamicChunkSize, j =>
                {
                    UpdateCell(i, j);
                });
            });
        }

        private void UpdateCell(int x, int y)
        {
            var xCoord = _seed + _coords.x * Constants.DynamicChunkSize + x - Constants.PlanetChance;
            var yCoord = _seed + _coords.y * Constants.DynamicChunkSize + y - Constants.PlanetChance;
            var noise = Mathf.PerlinNoise(xCoord, yCoord);

            var ratingNoise = Mathf.RoundToInt(noise * Constants.MaxRating);
            var rating = 0;

            var hasPlanet = (_seed + x + y + (_coords.x + _coords.y) * Constants.DynamicChunkSize) % 3 == 0;
            if (hasPlanet)
            {
                rating = Mathf.Clamp(ratingNoise, 0, Constants.MaxRating);
            }

            _dynamicCells[x, y].Update(x, y, rating);
        }
    }
}
