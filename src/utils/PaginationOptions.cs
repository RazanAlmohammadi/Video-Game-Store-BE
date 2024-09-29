namespace FusionTech.src.Utils
{
    public class PaginationOptions
    {
        public int Limit { get; set; } = 2;
        public int Offset { get; set; } = 0;

        public string Search { get; set; } = string.Empty;
    }
}