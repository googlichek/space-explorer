namespace Game.Scripts
{
    public struct Constants
    {
        public const int TargetFrameRate = 60;

        public const float SecondsPerFrame = 1f / TargetFrameRate;

        public const int DynamicGridSize = 5;
        public const int DynamicChunkSize = 200;

        public const int MaxRating = 10000;
        public const int MaxPlanets = 20;

        public const int DynamicGridCorrection = DynamicGridSize / 2;

        public const float PlanetChance = 0.33f;

        public const float OrtographicSizeMin = 2.5f;
        public const float OrtographicSizeMax = 5000f;
        public const int OrtographicSizeStep = 2;
    }
}
