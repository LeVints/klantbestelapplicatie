using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class CartModel : PageModel
    {
        // DTO voor een item in de winkelwagen
        public class CartItem
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; } = string.Empty;
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }

        // Zorg dat Items nooit null is
        [BindProperty]
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public void OnGet()
        {
            // TODO: vervang deze voorbeelddata door echte data uit je DAL of uit de sessie
            Items = new List<CartItem>
            {
                new CartItem { ProductId = 1, ProductName = "Product A", Quantity = 2, UnitPrice = 9.99m },
                new CartItem { ProductId = 2, ProductName = "Product B", Quantity = 1, UnitPrice = 14.50m }
            };
        }

        public IActionResult OnPostRemove(int productId)
        {
            // TODO: pas deze logica aan zodat je uit je DAL of sessie verwijdert
            Items = Items.Where(i => i.ProductId != productId).ToList();

            // Blijf op dezelfde pagina en toon het aangepaste overzicht
            return Page();
        }
    }
}
