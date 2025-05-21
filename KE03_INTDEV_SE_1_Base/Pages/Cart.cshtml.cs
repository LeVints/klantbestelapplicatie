using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

public class CartModel : PageModel
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    [BindProperty]
    public List<CartItem> Items { get; set; } = new List<CartItem>();

    public void OnGet()
    {
        // TODO: haal echte winkelwagen-items op uit je DAL of sessie
        Items = new List<CartItem>
        {
            new CartItem { ProductId = 1, ProductName = "Product A", Quantity = 2, UnitPrice = 9.99m },
            new CartItem { ProductId = 2, ProductName = "Product B", Quantity = 1, UnitPrice = 14.50m }
        };
    }

    public IActionResult OnPostRemove(int productId)
    {
        Items = Items.Where(i => i.ProductId != productId).ToList();
        return Page();
    }
}
