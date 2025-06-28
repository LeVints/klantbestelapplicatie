using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using KE03_INTDEV_SE_1_Base.Helpers;
using KE03_INTDEV_SE_1_Base.Models;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class IndexModel : PageModel
    {
        // Logger en repository dependencies
        private readonly ILogger<IndexModel> _logger;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        // Lijsten met klanten en producten voor de view
        public IList<Customer> Customers { get; set; }
        public IList<Product> Products { get; set; }

        // Constructor met dependency injection
        public IndexModel(ILogger<IndexModel> logger, ICustomerRepository customerRepository, IProductRepository productRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            Customers = new List<Customer>();
            Products = new List<Product>();
        }

        // Wordt uitgevoerd bij een GET-verzoek op de Index-pagina
        public void OnGet()
        {
            // Haalt klanten en producten op uit de repository
            Customers = _customerRepository.GetAllCustomers().ToList();
            Products = _productRepository.GetAllProducts().ToList();
        }

        // Verwerkt een POST-verzoek om een product toe te voegen aan het winkelmandje
        public IActionResult OnPostAddToCart(int productId, string productName, decimal price)
        {
            // Haal het winkelmandje op uit de sessie of maak een nieuwe lijst aan
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

            // Controleer of het product al in het winkelmandje zit
            var existing = cart.FirstOrDefault(i => i.ProductId == productId);
            if (existing != null)
            {
                // Verhoog de hoeveelheid als het product al bestaat
                existing.Quantity++;
            }
            else
            {
                // Voeg nieuw product toe aan het winkelmandje
                cart.Add(new CartItem
                {
                    ProductId = productId,
                    ProductName = productName,
                    Quantity = 1,
                    UnitPrice = price
                });
            }

            // Sla het winkelmandje opnieuw op in de sessie
            HttpContext.Session.SetObject("Cart", cart);

            // Redirect naar de Index-pagina om de update te tonen
            return RedirectToPage("Index");
        }
    }
}
