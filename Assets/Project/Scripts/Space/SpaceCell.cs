using UnityEngine;

namespace Game.Scripts
{
    public struct SpaceCell
    {
        private Vector2Int _localCoords;
        private Vector2Int _coords;

        private int _rating;

        public Vector2Int LocalCoords => _localCoords;
        public Vector2Int Coords => _coords;

        public int Rating => _rating;

        public bool HasPlanet => _rating > 0;

        public void Update(int localX, int localY, int absoluteX, int absoluteY, int rating)
        {
            _localCoords.x = localX;
            _localCoords.y = localY;

            _coords.x = absoluteX;
            _coords.y = absoluteY;

            _rating = rating;
        }
    }
}
