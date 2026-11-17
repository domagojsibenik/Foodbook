namespace Foodbook.Helpers
{
    public class QueryObject
    {
        public string? UserId { get; set; } = null;
        public string? Title { get; set; } = null;
        public int? MaxCookingTime { get; set; } = null;
        public string? SortDirection { get; set; } = null;
        public bool Liked { get; set; }
    }
}
