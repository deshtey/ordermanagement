namespace ordermanagement.domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int SKU { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }

        public Product()
        {
            
        }
        public Product(string name, decimal unitPrice, int sku, int quantity)
        {
            Name = name;
            UnitPrice = unitPrice;
            SKU = sku;
            Quantity = quantity;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
