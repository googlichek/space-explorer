using System.Threading.Tasks;
using UnityEngine;

namespace Game.Scripts
{
    public class SpaceChunk
    {
        private readonly SpaceCell[,] _cells = new SpaceCell[Constants.ChunkSize, Constants.ChunkSize];

        private Vector2Int _coords;

        public SpaceCell[,] Cells => _cells;

        public Vector2Int Coords => _coords;

        public SpaceChunk(int x, int y)
        {
            Update(x, y);
        }

        public void Update(int x, int y)
        {
            _coords.x = x;
            _coords.y = y;

            Parallel.For(0, Constants.DynamicGridSize, i =>
            {
                Parallel.For(0, Constants.DynamicGridSize, j =>
                {
                    UpdateCell(i, j);
                });
            });
        }

        private void UpdateCell(int x, int y)
        {
            var noise = Mathf.PerlinNoise(_coords.x + x, _coords.y + y);
            var hasPlanet = noise <= Constants.PlanetChance;

            var rating = -1;
            if (hasPlanet)
                rating = Mathf.Clamp(Mathf.RoundToInt(noise * Constants.MaxRating), 0, Constants.MaxRating);

            _cells[x, y].Update(x, y, rating);
        }
    }
}
