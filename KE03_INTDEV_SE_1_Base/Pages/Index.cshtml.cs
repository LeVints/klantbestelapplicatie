using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICustomerRepository _customerRepository;

        public IList<Customer> Customers { get; set; }

        // Voor binding van formulierdata
        [BindProperty]
        public string? Name { get; set; }
        [BindProperty]
        public string? Address { get; set; }
        [BindProperty]
        public bool Active { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ICustomerRepository customerRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            Customers = new List<Customer>();
        }

        public void OnGet()
        {
            Customers = _customerRepository.GetAllCustomers().ToList();
            _logger.LogInformation($"getting all {Customers.Count} customers");
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Address))
            {
                ModelState.AddModelError(string.Empty, "Naam en Adres zijn verplicht.");
                OnGet(); // Reload customers
                return Page();
            }

            var newCustomer = new Customer
            {
                Name = Name,
                Address = Address,
                Active = Active
            };

            _customerRepository.AddCustomer(newCustomer);
            _logger.LogInformation($"Klant toegevoegd: {Name}");

            return RedirectToPage(); // Herlaad pagina met nieuwe klant
        }
    }
}
