using Microsoft.AspNetCore.Http;

namespace Application.DTOs
{
    public class PurchaseReadDto 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Seller { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public double PriceInDollar { get; set; }
        public string Description { get; set; }
        public string PaymentMethod { get; set; }
        public string Reccuring { get; set; }
        public ICollection<PhotoReadDto> Invoice { get; set; } 

    }

    public class PurchaseWriteDto
    {
        public string Name { get; set; }
        public string Seller { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public double PriceInDollar { get; set; }
        public string Description { get; set; }
        public string PaymentMethod { get; set; }
        public string Reccuring { get; set; }  
    }

    public class PurchaseUpdateDto
    {
        public string Name { get; set; }
        public string Seller { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public double PriceInDollar { get; set; }
        public string Description { get; set; }
        public string PaymentMethod { get; set; }
        public string Reccuring { get; set; }
        public ICollection<IFormFile> Files { get; set; }
    }
}