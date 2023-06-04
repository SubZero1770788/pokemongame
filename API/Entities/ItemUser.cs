namespace API.Entities.HelperEntities
{
    public class ItemUser
    {
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int UserId { get; set; }
        public UserData User { get; set; }
        public int Amount { get; set; }
    }
}