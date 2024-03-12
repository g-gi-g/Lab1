namespace MarketplaceWebApplication.Models
{
    public class CountOffersByCategoriesResponseItem
    {
        public string Category { get; set; }

        public int Count { get; set; } 

        public CountOffersByCategoriesResponseItem(string groupRating, int groupCount)
        {
            Category = groupRating;
            Count = groupCount;
        }
    }
}