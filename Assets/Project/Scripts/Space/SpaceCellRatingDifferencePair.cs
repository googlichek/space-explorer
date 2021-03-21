namespace Game.Scripts
{
    public struct SpaceCellRatingDifferencePair
    {
        private SpaceCell _cell;

        private int _ratingDifference;

        public SpaceCell Cell => _cell;

        public int RatingDifference => _ratingDifference;

        public SpaceCellRatingDifferencePair(int ratingDifference, SpaceCell cell)
        {
            _ratingDifference = ratingDifference;
            _cell = cell;
        }
    }
}
