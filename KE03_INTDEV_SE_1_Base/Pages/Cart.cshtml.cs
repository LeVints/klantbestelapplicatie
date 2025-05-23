using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using KE03_INTDEV_SE_1_Base.Helpers;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class CartModel : PageModel
    {
        public class CartItem
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; } = string.Empty;
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }

        public List<CartItem> Items { get; set; } = new();

        public void OnGet()
        {
            Items = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();
        }
    }
}
