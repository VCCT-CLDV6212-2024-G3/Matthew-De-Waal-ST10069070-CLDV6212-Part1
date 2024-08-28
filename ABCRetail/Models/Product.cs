namespace ABCRetail.Models
{
    public sealed class Product
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public float Price { get; set; }
        public string? Category { get; set; }
        public int? TotalStock { get; set; }
        public string? ImagePath { get; set; }

    }
}
