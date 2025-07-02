namespace WebApplication1.Models
{
    public class VEGETABLES
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string img { get; set; }
        public decimal Price { get; set; }


        // Foreign key
        public int CategoryId { get; set; }

        // Navigation property
        public Category Category { get; set; }
    }
}
