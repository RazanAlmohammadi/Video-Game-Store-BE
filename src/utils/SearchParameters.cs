namespace FusionTech.src.Utils
{
    public class SearchParameters : PaginationOptions
    {
        public float? MinPrice { get; set; } = 0.0f;
        public float? MaxPrice { get; set; } = 10000;
        public SortingTypes SortBy { get; set; } = SortingTypes.None;
        public bool Descending { get; set; } = false;

        public new void IsValid()
        {
            base.IsValid();

            if (MinPrice < 0)
                throw CustomException.BadRequest("MinPrice cannot be negative.");

            if (MaxPrice > 10000)
                throw CustomException.BadRequest("MaxPrice cannot be greater than 10000.");

            // Validate MinPrice is not greater than MaxPrice
            if (MinPrice > MaxPrice)
                throw CustomException.BadRequest("MinPrice cannot be greater than MaxPrice.");
        }
    }
}
