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

        public IList<Customer> Customers { get; set; }

        public OverzichtModel(ILogger<OverzichtModel> logger, ICustomerRepository customerRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            Customers = new List<Customer>();
        }

        public void OnGet()
        {
            Customers = _customerRepository.GetAllCustomers().ToList();
            _logger.LogInformation($"Aantal klanten geladen: {Customers.Count}");
        }
    }
}
