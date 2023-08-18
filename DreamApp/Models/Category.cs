namespace DreamApp.Models
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public byte[]? CategoryImage { get; set; }
    }
}
