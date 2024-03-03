using MarketplaceWebApplication.Data;

namespace MarketplaceWebApplication.Models
{
    public class UserChats
    {
        public virtual ICollection<Chat> AsASeller { get; set; } = new List<Chat>();

        public virtual ICollection<Chat> AsABuyer { get; set; } = new List<Chat>();

        public virtual ICollection<ChatBuyer> ChatsBuyers { get; set; } = new List<ChatBuyer>();
    }
}
