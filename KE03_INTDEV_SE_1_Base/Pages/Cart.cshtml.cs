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

        public IActionResult OnPostUpdateQuantity(int productId, int quantity)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

            var item = cart.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                item.Quantity = Math.Max(quantity, 1);
                HttpContext.Session.SetObject("Cart", cart);
                TempData["Message"] = $"Aantal voor '{item.ProductName}' bijgewerkt.";
            }

            return RedirectToPage("Cart");
        }

        public IActionResult OnPostRemoveItem(int productId)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

            var itemToRemove = cart.FirstOrDefault(i => i.ProductId == productId);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                HttpContext.Session.SetObject("Cart", cart);
                TempData["Message"] = $"'{itemToRemove.ProductName}' is verwijderd uit je winkelmand.";
            }

            return RedirectToPage("Cart");
        }

        public IActionResult OnPostPlaceOrder()
        {
            var items = HttpContext.Session.GetObject<List<CartItem>>("Cart");
            if (items == null || !items.Any())
            {
                TempData["Message"] = "Winkelwagen is leeg!";
                return RedirectToPage("Cart");
            }

            var customerId = 1; // In echte app: haal uit sessie of inlog

            var newOrder = new Order
            {
                CustomerId = customerId,
                OrderDate = DateTime.Now
            };

            foreach (var item in items)
            {
                var product = _productRepository.GetProductById(item.ProductId);
                if (product != null)
                {
                    var orderItem = new OrderItem
                    {
                        ProductId = item.ProductId,
                        Product = product,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    newOrder.OrderItems.Add(orderItem);
                }
            }

            _orderRepository.AddOrder(newOrder);

            HttpContext.Session.Remove("Cart");

            TempData["Message"] = "Bestelling succesvol geplaatst!";
            return RedirectToPage("Cart");
        }
    }
}
