using UnityEngine;

namespace Game.Scripts
{
    public struct SpaceCell
    {
        private Vector2Int _coords;

        private int _rating;

        public Vector2Int Coords => _coords;

        public int Rating => _rating;

        public bool HasPlanet => _rating > 0;

        public void Update(int x, int y, int rating)
        {
            _coords.x = x;
            _coords.y = y;

            _rating = rating;
        }
    }
}
