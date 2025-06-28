using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System;
using KE03_INTDEV_SE_1_Base.Helpers;
using KE03_INTDEV_SE_1_Base.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class CartModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        // Constructor voor dependency injection van repositories
        public CartModel(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        // Lijst van items in het winkelmandje
        public List<CartItem> Items { get; set; } = new();

        // Wordt uitgevoerd bij GET-verzoek; haalt winkelmandje op uit sessie
        public void OnGet()
        {
            Items = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();
        }

        // Handler voor bijwerken van de hoeveelheid van een product
        public IActionResult OnPostUpdateQuantity(int productId, int quantity)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

            var item = cart.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                // Zorg ervoor dat de hoeveelheid minimaal 1 is
                item.Quantity = Math.Max(quantity, 1);
                HttpContext.Session.SetObject("Cart", cart);
                TempData["Message"] = $"Aantal voor '{item.ProductName}' bijgewerkt.";
            }

            return RedirectToPage("Cart");
        }

        // Handler voor verwijderen van een product uit het winkelmandje
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

        // Handler voor het plaatsen van een bestelling
        public IActionResult OnPostPlaceOrder()
        {
            var items = HttpContext.Session.GetObject<List<CartItem>>("Cart");
            if (items == null || !items.Any())
            {
                TempData["Message"] = "Winkelwagen is leeg!";
                return RedirectToPage("Cart");
            }

            // Dit is hardcoded klant-id voor demo
            var customerId = 1;

            var newOrder = new Order
            {
                CustomerId = customerId,
                OrderDate = DateTime.Now
            };

            // Voeg elk item toe aan de nieuwe bestelling
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

            // Bestelling opslaan via de repository
            _orderRepository.AddOrder(newOrder);

            // Winkelwagen leegmaken na succesvolle bestelling
            HttpContext.Session.Remove("Cart");

            TempData["Message"] = "Bestelling succesvol geplaatst!";
            return RedirectToPage("Cart");
        }
    }
}