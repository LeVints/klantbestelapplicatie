using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class OverzichtModel : PageModel
    {
        private readonly ILogger<OverzichtModel> _logger;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public IList<Customer> Customers { get; set; }
        public IList<Order> Orders { get; set; }
        public IList<Product> Products { get; set; }

        public OverzichtModel(
            ILogger<OverzichtModel> logger,
            ICustomerRepository customerRepository,
            IOrderRepository orderRepository,
            IProductRepository productRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;

            Customers = new List<Customer>();
            Orders = new List<Order>();
            Products = new List<Product>();
        }

        public void OnGet()
        {
            Customers = _customerRepository.GetAllCustomers().ToList();
            Orders = _orderRepository.GetAllOrders().ToList();
            Products = _productRepository.GetAllProducts().ToList();

            _logger.LogInformation($"Aantal klanten geladen: {Customers.Count}");
            _logger.LogInformation($"Aantal orders geladen: {Orders.Count}");
            _logger.LogInformation($"Aantal producten geladen: {Products.Count}");
        }
    }
}
