namespace MarketplaceWebApplication.Models
{
    public class CountFeedbacksByRatingResponseItem
    {
        public string Rating { get; set; }

        public int Count { get; set; } 

        public CountFeedbacksByRatingResponseItem(string groupRating, int groupCount)
        {
            Rating = groupRating;
            Count = groupCount;
        }
    }
}