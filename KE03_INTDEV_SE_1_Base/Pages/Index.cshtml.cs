using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using KE03_INTDEV_SE_1_Base.Helpers;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        public IList<Customer> Customers { get; set; }
        public IList<Product> Products { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ICustomerRepository customerRepository, IProductRepository productRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            Customers = new List<Customer>();
            Products = new List<Product>();
        }

        public void OnGet()
        {
            Customers = _customerRepository.GetAllCustomers().ToList();
            Products = _productRepository.GetAllProducts().ToList();
        }

        public IActionResult OnPostAddToCart(int productId, string productName, decimal price)
        {
            var cart = HttpContext.Session.GetObject<List<CartModel.CartItem>>("Cart") ?? new List<CartModel.CartItem>();

            var existing = cart.FirstOrDefault(i => i.ProductId == productId);
            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                cart.Add(new CartModel.CartItem
                {
                    ProductId = productId,
                    ProductName = productName,
                    Quantity = 1,
                    UnitPrice = price
                });
            }

            HttpContext.Session.SetObject("Cart", cart);
            return RedirectToPage("Index");
        }
    }
}
