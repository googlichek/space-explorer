using UnityEngine;

namespace Game.Scripts
{
    public static class Extentions
    {
        private const float EqualityThreshold = 0.0000001f;

        public static bool IsEqual(this float a, float b)
        {
            return Mathf.Abs(a - b) <= EqualityThreshold;
        }

        public static bool IsEqual(this Vector2 a, Vector2 b)
        {
            return (a - b).sqrMagnitude <= EqualityThreshold;
        }
        
        public static bool IsEqual(this Vector2Int a, Vector2Int b)
        {
            return (a - b).sqrMagnitude <= 0;
        }
    }
}
