namespace KE03_INTDEV_SE_1_Base.Models
{
    // View model voor een item in het winkelmandje
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
} 