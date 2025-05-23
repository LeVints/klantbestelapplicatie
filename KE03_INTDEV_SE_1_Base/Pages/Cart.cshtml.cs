using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System;
using KE03_INTDEV_SE_1_Base.Helpers;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class CartModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public CartModel(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

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

        public IActionResult OnPostPlaceOrder()
        {
            var items = HttpContext.Session.GetObject<List<CartItem>>("Cart");
            if (items == null || !items.Any())
            {
                TempData["Message"] = "Winkelwagen is leeg!";
                return RedirectToPage("Cart");
            }

            // Hardcoded klant-ID; in productie kies je de ingelogde gebruiker
            var customerId = 1;

            var newOrder = new Order
            {
                CustomerId = customerId,
                OrderDate = DateTime.Now
                // Products-lijst wordt hieronder gevuld
            };

            foreach (var item in items)
            {
                var product = _productRepository.GetProductById(item.ProductId);
                if (product != null)
                {
                    newOrder.Products.Add(product); // ✅ lijst is al geïnitialiseerd in het model
                }
            }

            _orderRepository.AddOrder(newOrder);

            HttpContext.Session.Remove("Cart");

            TempData["Message"] = "Bestelling succesvol geplaatst!";
            return RedirectToPage("Cart");
        }
    }
}
