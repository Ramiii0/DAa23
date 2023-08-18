namespace DreamApp.Models
{
    public class Item
    {
        public Guid ItemId { get; set; }
        public string? ItemName { get; set; }
        public int ItemPrice { get; set; }
        public int ItemSize { get; set; }
        public string? ItemImage { get; set; }
        public int ItemPackage { get; set; }
        public string? Lable { get; set; }
        public Guid CategoryId { get; set; }
    }
}
